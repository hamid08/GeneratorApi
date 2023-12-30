using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GeneratorApi.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static bool HasValue(this string str) => !string.IsNullOrEmpty(str);

        public static IEnumerable<string> Separate(this string str, char separator)
        {
            try
            {
                return str.IsNullOrEmpty() ? new string[] { } : str.Split(separator);
            }
            catch (Exception ex)
            {
                return new string[] { };
            }
        }

        public static int? ToInt(this string str)
        {
            int? num = null;

            try
            {
                if (int.TryParse(str, out _))
                {
                    num = int.Parse(str);
                }
            }
            catch (Exception ex) { }

            return num;
        }

        public static long? ToLong(this string str)
        {
            long? num = null;

            try
            {
                if (long.TryParse(str, out _))
                {
                    num = long.Parse(str);
                }
            }
            catch (Exception ex) { }

            return num;
        }

        public static string AsNullAlternate(this string str, string nullAlternate = "")
        {
            try
            {
                return !str.IsNullOrEmpty() ? str : nullAlternate;
            }
            catch (Exception ex)
            {
                return str;
            }
        }

        public static string AsSummary(this string str, int len, string suffix = "", string nullAlternate = "")
        {
            try
            {
                return !str.IsNullOrEmpty() ? str.Length > len ? str.Substring(0, len) + suffix : str : nullAlternate;
            }
            catch (Exception ex)
            {
                return str;
            }
        }

        public static byte[] GetBytes(this string value) => System.Text.Encoding.UTF8.GetBytes(value);

        public static bool IsJsonString(this string str)
        {
            try
            {
                if (str.IsNullOrEmpty())
                {
                    return false;
                }

                var obj = JsonConvert.DeserializeObject(str);

                return obj != null;
            }
            catch (Exception ex) { return false; }
        }

        public static string Join(this IEnumerable<string> values, string seperator) => string.Join(seperator, values);
    }
}
