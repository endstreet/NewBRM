using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace SASSADirectCapture.BL
{
    public static class Extentions
    {
        #region Public Methods

        public static string Add60Days(this string date)
        {
            DateTime dt = DateTime.ParseExact(date.Replace("/", ""), "yyyyMMdd", CultureInfo.InvariantCulture);
            return dt.AddDays(60).ToString("yyyyMMdd");
        }

        public static string ToOracleDate(this string date)
        {
            string[] parts = date.Split('/');
            string monthpart = parts[0].PadLeft(3 - parts[0].Length, '0');
            string daypart = parts[1].PadLeft(3 - parts[0].Length, '0');
            string yearpart = parts[2].Substring(0, 4);

            return monthpart + "/" + daypart + "/" + yearpart;
        }
        public static String ToEncodedString(this Stream stream, Encoding enc = null)
        {
            enc = enc ?? Encoding.UTF8;

            byte[] bytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(bytes, 0, (int)stream.Length);
            string data = enc.GetString(bytes);

            return enc.GetString(bytes);
        }

        public static int ParseOrZero(this string source)
        {
            int result = 0;
            int.TryParse(source, out result);
            return result;

        }
        #endregion Public Methods
    }
}