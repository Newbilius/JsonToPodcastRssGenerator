using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JsonToRssGenerator
{
    public static class Helpers
    {
        public static bool IsEmpty(this Array array)
        {
            if (array == null)
                return true;
            return array.Length == 0;
        }

        public static bool IsFilled(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        private static readonly CultureInfo CultureInfo = new CultureInfo("EN-US");

        public static string ToStringRFC2822(this DateTime date)
        {
            return date.ToString("ddd, dd MMM yyyy HH:mm:ss ", CultureInfo) + "GMT";
        }
    }
}