using System;
using System.Globalization;

namespace WellData.Core.Extensions
{
    public static class StringExtensions
    {
        public static int ToNumber(this string value)
        {
            return Int32.TryParse(value, out int n) ? n : 0;
        }

        public static double ToDouble(this string value)
        {
            return double.TryParse(value, out double n) ? n : 0;
        }


        public static decimal ToDecimal(this string value)
        {
            return decimal.TryParse(value, NumberStyles.AllowCurrencySymbol | NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, out decimal n) ? n : 0.00M;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
