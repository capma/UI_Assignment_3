using System;
using System.Collections.Generic;
using System.Media;

namespace TriPham_Assignment_3
{
    class ListTank
    {
        #region PROPERTIES

        public List<Tank> TankList { get; set; }
        public int Index { get; set; }
        public SoundPlayer Alarm;
        private Random random;

        #endregion

        #region CONSTRUCTOR

        public ListTank()
        {
            TankList = new List<Tank>();
            Index = 0;
            random = new Random();
            Alarm = new SoundPlayer();
            Alarm.Stream = Common.SOUND_ALARM;            
        }

        #endregion

        #region METHODS

        public void Add(Tank tank)
        {
            tank.Valve.Tag = Index;
            tank.Acknowledge.Tag = Index;
            TankList.Add(tank);
            Index++;
        }

        public int Count()
        {
            return TankList.Count;
        }

        public Tank this[int index]
        {
            get { return TankList[index];  }
            set { TankList[index] = value; }
        }

        public void InitControl()
        {
            foreach (Tank eachTank in TankList)
            {
                eachTank.RandomValveStatus(random);
                eachTank.InitPressureLevel();
                eachTank.SetPressureLevel();
                eachTank.SetPressureNumber();
                eachTank.SetValveStatus();
                eachTank.SetGauge();
                eachTank.SetPressureColor();
                eachTank.CheckPressureStatus();
                eachTank.SetPressureStatus();
                eachTank.Alarm.Visible = false;
                eachTank.Acknowledge.Visible = false;
            }
        }

        public void UpdateTank()
        {
            foreach (Tank eachTank in TankList)
            {
                eachTank.RandomPressureChange(random);
                eachTank.SetPressureLevel();
                eachTank.SetPressureNumber();
                eachTank.SetValveStatus();
                eachTank.SetGauge();
                eachTank.SetPressureColor();
                eachTank.CheckPressureStatus();
                eachTank.SetPressureStatus();
                eachTank.WriteLog();
            }
        }

        public void SetAlarmBlink()
        {
            foreach (Tank eachTank in TankList)
            {
                eachTank.CheckIfShouldBlink();
                eachTank.CheckIfShouldAlarm();

                if (eachTank.ShouldBlink)
                {
                    eachTank.Alarm.Visible = !eachTank.Alarm.Visible;
                }
                else
                {
                    eachTank.Alarm.Visible = false;
                }
            }
        }

        public void SetAlarmSound(bool IsSouldCheckedOn)
        {
            if (!IsSouldCheckedOn)
            {
                Alarm.Stop();
            }
            else
            {
                bool isAlarm = false;
                foreach (Tank eachTank in TankList)
                {
                    if (eachTank.ShouldSoundOn)
                    {
                        isAlarm = true;
                        break;
                    }
                }

                if (isAlarm)
                {
                    Common.SOUND_ALARM.Position = 0;    // Manually rewind stream  
                    Alarm.Stream = null;                // Then we have to set stream to null 
                    Alarm.Stream = Common.SOUND_ALARM;  // And set it again, to force it to be loaded again... 
                    Alarm.Play();
                }
                else
                {
                    Alarm.Stop();
                }
            }
        }

        #endregion
    }
}