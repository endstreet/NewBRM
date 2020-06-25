namespace SASSADirectCapture.EntityModels
{
    public class SocpenStatusBase
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

    public static class SocpenPrimaryStatusBase
    {
        public readonly static string CODE_A = "A";
        public readonly static string CODE_B = "B";
        public readonly static string CODE_C = "C";
        public readonly static string CODE_D = "D";
        public readonly static string CODE_E = "E";
        public readonly static string CODE_F = "F";
        public readonly static string CODE_G = "G";
        public readonly static string CODE_H = "H";
        public readonly static string CODE_I = "I";
        public readonly static string CODE_U = "U";
        public readonly static string CODE_0 = "0";
        public readonly static string CODE_1 = "1";
        public readonly static string CODE_2 = "2";
        public readonly static string CODE_3 = "3";
        public readonly static string CODE_4 = "4";
        public readonly static string CODE_5 = "5";
        public readonly static string CODE_6 = "6";
        public readonly static string CODE_7 = "7";
        public readonly static string CODE_8 = "8";
        public readonly static string CODE_9 = "9";
    }

    public static class SocpenSecondaryStatusBase
    {
        public readonly static string CODE_B = "B";
        public readonly static string CODE_C = "C";
        public readonly static string CODE_0 = "0";
        public readonly static string CODE_1 = "1";
        public readonly static string CODE_2 = "2";
        public readonly static string CODE_3 = "3";
        public readonly static string CODE_4 = "4";
        public readonly static string CODE_6 = "6";
        public readonly static string CODE_7 = "7";
        public readonly static string CODE_9 = "9";
    }
}