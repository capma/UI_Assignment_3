using System;
using System.Windows.Forms;

namespace TriPham_Assignment_3
{
    public partial class frmScada : Form
    {

        #region VARIABLES

        ListTank listTank = null;        

        #endregion

        #region CONSTRUCTOR

        public frmScada()
        {
            InitializeComponent();
        }

        #endregion

        #region METHODS

        private void InitAll()
        {
            Common.LEVEL_ACTUAL_HEIGHT = lblLevelAlarm1.Height;

            listTank = new ListTank();

            Tank tank1 = new Tank(TankName.Tank_1, lblLevel1, lblGasNumber1, pictValve1, lblValveStatus1, pictGauge1, lblLevelAlarm1, btnAck1, pictAlarm1, lblPressureStatus1);
            Tank tank2 = new Tank(TankName.Tank_2, lblLevel2, lblGasNumber2, pictValve2, lblValveStatus2, pictGauge2, lblLevelAlarm2, btnAck2, pictAlarm2, lblPressureStatus2);
            Tank tank3 = new Tank(TankName.Tank_3, lblLevel3, lblGasNumber3, pictValve3, lblValveStatus3, pictGauge3, lblLevelAlarm3, btnAck3, pictAlarm3, lblPressureStatus3);
            Tank tank4 = new Tank(TankName.Tank_4, lblLevel4, lblGasNumber4, pictValve4, lblValveStatus4, pictGauge4, lblLevelAlarm4, btnAck4, pictAlarm4, lblPressureStatus4);
            Tank tank5 = new Tank(TankName.Tank_5, lblLevel5, lblGasNumber5, pictValve5, lblValveStatus5, pictGauge5, lblLevelAlarm5, btnAck5, pictAlarm5, lblPressureStatus5);
            Tank tank6 = new Tank(TankName.Tank_6, lblLevel6, lblGasNumber6, pictValve6, lblValveStatus6, pictGauge6, lblLevelAlarm6, btnAck6, pictAlarm6, lblPressureStatus6);

            listTank.Add(tank1);
            listTank.Add(tank2);
            listTank.Add(tank3);
            listTank.Add(tank4);
            listTank.Add(tank5);
            listTank.Add(tank6);

            listTank.InitControl();

            timerPressureChange.Interval = Common.TIMER_PRESSURE_CHANGE;
            timerPressureChange.Enabled = true;
            timerPressureChange.Stop();

            timerAlarmBlink.Interval = Common.TIMER_ALARM_BLINK;
            timerAlarmBlink.Enabled = true;
            timerAlarmBlink.Stop();

            timerAlarmSound.Interval = Common.TIMER_ALARM_SOUND;
            timerAlarmSound.Enabled = true;
            timerAlarmSound.Stop();

            hScrollBarTimerValue.Value = Common.TIMER_PRESSURE_CHANGE;
            lblTimerValue.Text = hScrollBarTimerValue.Value.ToString();
            chkSoundOn.Checked = true;
            lvLog.Items.Clear();
            chartSCADA.ChartAreas[0].AxisY.Maximum = 150;
            UpdateChart();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void DisplayLog()
        {
            ListViewItem listViewItem = null;

            foreach (Tank eachTank in listTank.TankList)
            {
                if (eachTank.Log != null)
                {
                    listViewItem = new ListViewItem(eachTank.Log)
                    {
                        BackColor = eachTank.PressureStatusBackColor,
                        ForeColor = eachTank.PressureStatusForeColor
                    };

                    lvLog.Items.Insert(0, listViewItem);
                }
            }
        }

        private void UpdateChart()
        {
            for(int i = 0; i < listTank.Count(); i++)
            {
                chartSCADA.Series[0].Points[i].SetValueY(listTank[i].Level_100);
                chartSCADA.Series[0].Points[i].Label = listTank[i].Name + "\n" + listTank[i].Level_100.ToString();
                chartSCADA.Series[0].Points[i].Color = listTank[i].PressureStatusBackColor;
                chartSCADA.Annotations[i].BackColor = listTank[i].PressureStatusBackColor;
                chartSCADA.Annotations[i].Height = (listTank[i].IsOpenValve) ? 15 : -15;
                chartSCADA.Annotations[i].AnchorOffsetY = (listTank[i].IsOpenValve) ? -40 : -25;
                chartSCADA.Refresh();
            }
        }

        #endregion

        #region EVENTS

        private void frmScada_Load(object sender, EventArgs e)
        {
            try
            {
                InitAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;

                // Start to rumble!
                timerPressureChange.Start();
                timerAlarmBlink.Start();
                timerAlarmSound.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                btnStop.Enabled = false;
                btnStart.Enabled = true;

                listTank.SetAlarmSound(false);

                // Pause rumbling!
                timerPressureChange.Stop();
                timerAlarmBlink.Stop();
                timerAlarmSound.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Stop before confirming with user
            btnStop_Click(sender, e);

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to reset?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                InitAll();                
            }
            else
            {
                // Restart if user refuses to reset
                btnStart_Click(sender, e);
            }
        }

        private void Valve_Click(object sender, EventArgs e)
        {
            int currentTankIndex = 0;

            try
            {
                PictureBox pict = (PictureBox)sender;
                currentTankIndex = (int)pict.Tag;

                listTank[currentTankIndex].Is_ACK_Clicked = true;

                if (pict.BackgroundImage == Common.IMG_VALVE_OPEN)
                {
                    pict.BackgroundImage = Common.IMG_VALVE_CLOSE;
                    listTank[currentTankIndex].IsOpenValve = false;
                }
                else
                {
                    pict.BackgroundImage = Common.IMG_VALVE_OPEN;
                    listTank[currentTankIndex].IsOpenValve = true;
                }

                // Update caption of the valve label: OPEN or CLOSED
                listTank[currentTankIndex].SetValveStatus();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Acknowledge_Click(object sender, EventArgs e)
        {
            int currentTankIndex = 0;

            try
            {
                Button btnAck = sender as Button;
                currentTankIndex = (int)btnAck.Tag;

                btnAck.Visible = false;
                listTank[currentTankIndex].Is_ACK_Clicked = true;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void timerPressure_Tick(object sender, EventArgs e)
        {
            try
            {
                listTank.UpdateTank();                
                DisplayLog();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void timerAlarm_Tick(object sender, EventArgs e)
        {
            try
            {
                listTank.SetAlarmBlink();
                UpdateChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void timerAlarmSound_Tick(object sender, EventArgs e)
        {
            try
            {
                listTank.SetAlarmSound(chkSoundOn.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void hScrollBarTimerValue_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                lblTimerValue.Text = hScrollBarTimerValue.Value.ToString();
                btnApply.Enabled = (hScrollBarTimerValue.Value == timerPressureChange.Interval) ? false : true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                timerPressureChange.Interval = hScrollBarTimerValue.Value;
                btnApply.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        
    }
}