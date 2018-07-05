using System.Drawing;
using System.IO;

namespace TriPham_Assignment_3
{
    public enum TankName
    {
        Tank_1,
        Tank_2,
        Tank_3,
        Tank_4,
        Tank_5,
        Tank_6
    }

    public enum PressureStatus
    {
        NONE,
        TOO_HIGH,
        HIGH,
        NORMAL,
        LOW,
        TOO_LOW
    }

    public static class Common
    {        
        #region PRESSURE LEVEL

        public static int LEVEL_ACTUAL_HEIGHT = 135;
        public static int LEVEL_START = 50;
        public static int LEVEL_MAX = 100;

        public static int LEVEL_INCREASE_MIN = -1;
        public static int LEVEL_INCREASE_MAX = 3;
        public static int LEVEL_DECREASE_MIN = -1;
        public static int LEVEL_DECREASE_MAX = 1;

        public static int LEVEL_TOO_LOW_MIN = 0;
        public static int LEVEL_TOO_LOW_MAX = 29;
        public static int LEVEL_LOW_MIN = 30;
        public static int LEVEL_LOW_MAX = 69;
        public static int LEVEL_NORMAL_MIN = 70;
        public static int LEVEL_NORMAL_MAX = 79;
        public static int LEVEL_HIGH_MIN = 80;
        public static int LEVEL_HIGH_MAX = 89;
        public static int LEVEL_TOO_HIGH_MIN = 90;
        public static int LEVEL_TOO_HIGH_MAX = 100;

        #endregion

        #region TIMER

        public static int TIMER_PRESSURE_CHANGE = 500;
        public static int TIMER_ALARM_BLINK = 100;
        public static int TIMER_ALARM_SOUND = 500;

        #endregion

        #region CAPTION 

        public const string VALVE_STATUS_OPEN = "OPEN";
        public const string VALVE_STATUS_CLOSED = "CLOSED";

        #endregion 

        #region IMAGE

        public static Image IMG_VALVE_OPEN = global::TriPham_Assignment_3.Properties.Resources.valve_open_1b;
        public static Image IMG_VALVE_CLOSE = global::TriPham_Assignment_3.Properties.Resources.valve_close_1b;

        #endregion

        #region COLOR

        public static Color ALARM_TOO_LOW_GREEN = Color.FromArgb(121, 194, 67);
        public static Color ALARM_LOW_ORANGE = Color.FromArgb(238, 163, 0);
        public static Color ALARM_NORMAL_RED = Color.FromArgb(241, 65, 0);
        public static Color ALARM_HIGH_BLUE = Color.FromArgb(143, 47, 190);
        public static Color ALARM_TOO_HIGH_BROWN = Color.FromArgb(151, 39, 39);

        #endregion

        #region SOUND

        public static Stream SOUND_ALARM = global::TriPham_Assignment_3.Properties.Resources.strange_alarm;

        #endregion

        #region LOG

        public const int NUMBER_OF_COLUMNS = 4;

        #endregion

    }
}
