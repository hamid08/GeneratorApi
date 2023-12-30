using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorApi.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToCurrencyFormat(this object obj)
        {
            string result = null;

            try
            {
                long value = 0;
                long.TryParse(obj.ToString(), out value);

                result = string.Format("{0:n0}", value);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public static string ToPersianString(this object obj)
        {
            try
            {
                var str = obj.ToString();

                /* ۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹ */
                if (string.IsNullOrEmpty(str))
                {
                    return str;
                }

                str = str.Replace("0", "۰");
                str = str.Replace("1", "۱");
                str = str.Replace("2", "۲");
                str = str.Replace("3", "۳");
                str = str.Replace("4", "۴");
                str = str.Replace("5", "۵");
                str = str.Replace("6", "۶");
                str = str.Replace("7", "۷");
                str = str.Replace("8", "۸");
                str = str.Replace("9", "۹");

                return str;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
