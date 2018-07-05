using System;
using System.Windows.Forms;
using System.Drawing;

namespace TriPham_Assignment_3
{
    class Tank
    {
        #region VARIABLES

        protected bool isOpenValve;

        #endregion

        #region PROPERTIES

        public TankName Name { get; set; }
        public int Level { get; set; }
        public int Level_100 { get; set; }
        public Label PressureLevel { get; set; }
        public Label PressureNumber { get; set; }
        public PictureBox Valve { get; set; }
        public Label ValveStatus { get; set; }
        public PictureBox Gauge { get; set; }
        public Label PressureAlarm { get; set; }
        public Button Acknowledge { get; set; }
        public PictureBox Alarm { get; set; }
        public Label LabelPressureStatus { get; set; }
        public Color PressureStatusBackColor { get; set; }
        public Color PressureStatusForeColor { get; set; }
        public bool Is_ACK_Clicked { get; set; }
        public bool ShouldSoundOn { get; set; }
        public bool ShouldBlink { get; set; }
        public bool ShouldWriteLog { get; set; }
        public bool IsOpenValve
        {
            get { return isOpenValve; }
            set
            {
                isOpenValve = value;
                Is_ACK_Clicked = !Is_ACK_Clicked;
                if (Valve != null)
                {
                    Valve.BackgroundImage = (isOpenValve) ? Common.IMG_VALVE_OPEN : Common.IMG_VALVE_CLOSE;
                }                
            }
        }

        public PressureStatus CurrentPressureStatus { get; set; }
        public string[] Log { get; set; }

        #endregion

        #region CONSTRUCTORS

        public Tank(TankName Name, Label PressureLevel, Label PressureNumber, PictureBox Valve, 
                    Label ValveStatus, PictureBox Gauge, Label PressureAlarm,
                    Button Acknowledge, PictureBox Alarm, Label PressureStatus)
        {
            this.Is_ACK_Clicked = false;
            this.ShouldSoundOn = false;
            this.ShouldBlink = false;
            this.ShouldWriteLog = false;            
            this.CurrentPressureStatus = GetNewPressureStatus();

            this.Name = Name;
            this.PressureLevel = PressureLevel;
            this.PressureNumber = PressureNumber;
            this.Valve = Valve;            
            this.ValveStatus = ValveStatus;
            this.Gauge = Gauge;
            this.PressureAlarm = PressureAlarm;
            this.Acknowledge = Acknowledge;
            this.Alarm = Alarm;
            this.LabelPressureStatus = PressureStatus;
        }

        #endregion

        #region METHODS

        public void RandomValveStatus(Random random)
        {
            int rnd = random.Next(0, 2);
            IsOpenValve = (rnd == 0) ? true : false;
        }

        private void ChangePressureToUnit100()
        {
            // Exchange actual height to a scale unit of 100
            Level_100 = (((Common.LEVEL_ACTUAL_HEIGHT - Level) * Common.LEVEL_MAX) / Common.LEVEL_ACTUAL_HEIGHT);
        }

        public void InitPressureLevel()
        {
            this.Level = (isOpenValve) ? (int)(Common.LEVEL_ACTUAL_HEIGHT * 0.7) : (int)(Common.LEVEL_ACTUAL_HEIGHT * 0.3);
            ChangePressureToUnit100();
        }

        public void RandomPressureChange(Random random)
        {
            int randomLevel = (IsOpenValve == true) ?
                                    random.Next(Common.LEVEL_DECREASE_MIN, Common.LEVEL_DECREASE_MAX)
                                  : random.Next(Common.LEVEL_INCREASE_MIN, Common.LEVEL_INCREASE_MAX);

            Level += randomLevel;

            if (Level < 0) Level = 0;
            if (Level > Common.LEVEL_ACTUAL_HEIGHT) Level = Common.LEVEL_ACTUAL_HEIGHT;

            ChangePressureToUnit100();
        }

        public void SetPressureNumber()
        {
            this.PressureNumber.Text = Level_100.ToString();
        }

        public void SetPressureLevel()
        {
            this.PressureLevel.Height = this.Level;
        }

        public void SetValveStatus()
        {
            this.ValveStatus.Text = (this.IsOpenValve) ? Common.VALVE_STATUS_OPEN : Common.VALVE_STATUS_CLOSED;
            this.ValveStatus.ForeColor = (this.IsOpenValve) ? Color.Blue : Color.Red;
        }

        public void SetGauge()
        {
            this.Gauge.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("gauge_" + Level_100);
        }

        public void SetPressureColor()
        {
            if (Level_100 >= Common.LEVEL_TOO_LOW_MIN && Level_100 <= Common.LEVEL_TOO_LOW_MAX)     { PressureAlarm.BackColor = Common.ALARM_TOO_LOW_GREEN; PressureStatusBackColor = Common.ALARM_TOO_LOW_GREEN; PressureStatusForeColor = Color.Black; }
            if (Level_100 >= Common.LEVEL_LOW_MIN && Level_100 <= Common.LEVEL_LOW_MAX)             { PressureAlarm.BackColor = Common.ALARM_LOW_ORANGE; PressureStatusBackColor = Common.ALARM_LOW_ORANGE; PressureStatusForeColor = Color.Black; }
            if (Level_100 >= Common.LEVEL_NORMAL_MIN && Level_100 <= Common.LEVEL_NORMAL_MAX)       { PressureAlarm.BackColor = Common.ALARM_NORMAL_RED; PressureStatusBackColor = Common.ALARM_NORMAL_RED; PressureStatusForeColor = Color.Yellow; }
            if (Level_100 >= Common.LEVEL_HIGH_MIN && Level_100 <= Common.LEVEL_HIGH_MAX)           { PressureAlarm.BackColor = Common.ALARM_HIGH_BLUE; PressureStatusBackColor = Common.ALARM_HIGH_BLUE; PressureStatusForeColor = Color.White; }
            if (Level_100 >= Common.LEVEL_TOO_HIGH_MIN && Level_100 <= Common.LEVEL_TOO_HIGH_MAX)   { PressureAlarm.BackColor = Common.ALARM_TOO_HIGH_BROWN; PressureStatusBackColor = Common.ALARM_TOO_HIGH_BROWN; PressureStatusForeColor = Color.Yellow; }
        }

        public void SetPressureStatus()
        {
            this.LabelPressureStatus.Text = this.CurrentPressureStatus.ToString();
            //this.lblPressureStatus.BackColor = this.PressureStatusBackColor;
            this.LabelPressureStatus.ForeColor = Color.Navy;
        }

        private PressureStatus GetNewPressureStatus()
        {
            PressureStatus newPressureStatus = PressureStatus.NONE;

            if (Level_100 >= Common.LEVEL_TOO_LOW_MIN && Level_100 <= Common.LEVEL_TOO_LOW_MAX)     newPressureStatus = PressureStatus.TOO_LOW;
            if (Level_100 >= Common.LEVEL_LOW_MIN && Level_100 <= Common.LEVEL_LOW_MAX)             newPressureStatus = PressureStatus.LOW;
            if (Level_100 >= Common.LEVEL_NORMAL_MIN && Level_100 <= Common.LEVEL_NORMAL_MAX)       newPressureStatus = PressureStatus.NORMAL;
            if (Level_100 >= Common.LEVEL_HIGH_MIN && Level_100 <= Common.LEVEL_HIGH_MAX)           newPressureStatus = PressureStatus.HIGH;
            if (Level_100 >= Common.LEVEL_TOO_HIGH_MIN && Level_100 <= Common.LEVEL_TOO_HIGH_MAX)   newPressureStatus = PressureStatus.TOO_HIGH;

            return newPressureStatus;
        }

        public void CheckPressureStatus()
        {
            PressureStatus newPressureStatus = GetNewPressureStatus();

            if (newPressureStatus != CurrentPressureStatus)
            {
                CurrentPressureStatus = newPressureStatus;

                switch (newPressureStatus)
                {
                    // Turn alarm ON for TOO_LOW, HIGH or TOO_HIGH
                    case PressureStatus.TOO_LOW:
                    case PressureStatus.TOO_HIGH:
                        Is_ACK_Clicked = false;
                        Acknowledge.Visible = true;
                        ShouldWriteLog = true;
                        break;

                    // Turn alarm OFF for LOW or NORMAL
                    case PressureStatus.LOW:
                    case PressureStatus.NORMAL:
                    case PressureStatus.HIGH:
                        Is_ACK_Clicked = true;
                        Acknowledge.Visible = false;
                        ShouldWriteLog = false;
                        break;
                }
            } 
        }

        public void CheckIfShouldAlarm()
        {
            if (Acknowledge.Visible && !Is_ACK_Clicked)
            {
                ShouldSoundOn = true;
                Acknowledge.Visible = true;
            }
            else 
            {
                ShouldSoundOn = false;
            }
        }

        public void CheckIfShouldBlink()
        {
            ShouldBlink = (Acknowledge.Visible && !Is_ACK_Clicked) ? true : false;            
        }

        public void WriteLog()
        {
            if (ShouldWriteLog)
            {
                ShouldWriteLog = false;

                string[] attr = new string[Common.NUMBER_OF_COLUMNS];

                attr[0] = DateTime.Now.ToString();
                attr[1] = this.CurrentPressureStatus.ToString();
                attr[2] = this.Name.ToString();
                attr[3] = "Pressure is " + this.Level_100;

                this.Log = attr;
            }
            else
            {
                this.Log = null;
            }
        }

        #endregion

    }
}