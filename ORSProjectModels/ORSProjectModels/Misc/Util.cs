using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ORSProjectModels
{
    public static class Util
    {

        public static double AbsoluteValue(double value)
        {
            if (value < 0)
            {
                return value * -1;
            }
            return value;
        }

        public static double Round(double value, int decimalPlaces = 3)
        {
            string rawDecimal = GetDecimalPart(value).ToString();
            if (rawDecimal.Length <= decimalPlaces)
            {
                return value;
            }
            string roundedDecimal = rawDecimal.Substring(0, decimalPlaces);
            string integerPart = GetIntegerPart(value).ToString();
            double firstExcludedDigit = double.Parse(rawDecimal[decimalPlaces].ToString());
            if (firstExcludedDigit >= 5)
            {
                double lastDigit = double.Parse(roundedDecimal[decimalPlaces - 1].ToString()) + 1;
                roundedDecimal = roundedDecimal.Substring(0, decimalPlaces - 1) + lastDigit;
            }
            string convertedValue = string.Format("{0}{1}{2}", integerPart, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, roundedDecimal);
            return double.Parse(convertedValue);
        }

        public static double GetDecimalPart(double number)
        {
            string[] parts = number.ToString().Split(',', '.');
            double decimalPart = double.Parse(parts[1]);
            return decimalPart;
        }

        public static double GetIntegerPart(double number)
        {
            string[] parts = number.ToString().Split(',', '.');
            double integerPart = double.Parse(parts[0]);
            return integerPart;
        }

        /// <summary>
        /// Gets the next highest integer
        /// </summary>
        /// <param name="number">The value to ceiling</param>
        /// <returns>The next highest integer</returns>
        public static double Ceiling(double number)
        {
            return GetIntegerPart(number) + 1;
        }

        /// <summary>
        /// Gets the next lowest integer
        /// </summary>
        /// <param name="number">The value to floor</param>
        /// <returns>The next lowest integer</returns>
        public static double Floor(double number)
        {
            return GetIntegerPart(number);
        }

        public static double[][] CheckForSmallValues(double[][] table)
        {
            for (int i = 0; i < table.Length; i++)
            {
                for (int j = 0; j < table[i].Length; j++)
                {
                    if (table[i][j] <= double.Epsilon && table[i][j] > 0)
                    {
                        table[i][j] = 0;
                    }
                }
            }
            return table;
        }
    }
}