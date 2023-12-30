using GeneratorApi.Utilities;
using System;
using System.Linq;

namespace GeneratorApi.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        /// <param name="date"></param>
        /// <param name="convertToIranTimeZone">اگر تاریخ و زمان با فرمت UTC باشند، ابتدا آن‌ها را به منطقه‌ی زمانی ایران تبدیل می‌کند</param
        public static string ToPersian(this DateTime date, string format = "yyyy/MM/dd HH:mm:ss", bool convertToIranTimeZone = true, bool convertToPersianString = true)
        {
            try
            {
                if (date.Kind == DateTimeKind.Utc && convertToIranTimeZone)
                {
                    date = date.ToIranTimeZoneDateTime();
                }

                var persianDate = date.ToString(format, PersianCultureUtils.Instance);

                return convertToPersianString ? persianDate.ToPersianString() : persianDate;
            }
            catch (Exception)
            {
                return format.IsNullOrEmpty() ? date.ToString() : date.ToString(format);
            }
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// </summary>
        /// <returns>تاریخ شمسی</returns>
        /// <param name="date"></param>
        /// <param name="convertToIranTimeZone">اگر تاریخ و زمان با فرمت UTC باشند، ابتدا آن‌ها را به منطقه‌ی زمانی ایران تبدیل می‌کند</param
        public static string ToPersian(this DateTime? date, string format = "yyyy/MM/dd HH:mm:ss", bool convertToIranTimeZone = true, bool convertToPersianString = true) => date.HasValue ? date.Value.ToPersian(format, convertToIranTimeZone, convertToPersianString) : null;

        public static string ToPersianDate(this DateTime? date) => date.HasValue ? date.Value.ToPersian("yyyy/MM/dd") : null;
        public static string ToPersianDate(this DateTime date) => date.ToPersian("yyyy/MM/dd");

        public static string ToPersianTime(this DateTime? date) => date.HasValue ? date.Value.ToPersian("HH:mm:ss") : null;
        public static string ToPersianTime(this DateTime date) => date.ToPersian("HH:mm:ss");

        /// <summary>
        /// تبدیل منطقه زمانی این وهله به منطقه زمانی ایران
        /// </summary>
        public static DateTime ToIranTimeZoneDateTime(this DateTime dateTime)
        {
            var iranStandardTime = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(timeZoneInfo =>
 timeZoneInfo.StandardName.Contains("Iran") ||
 timeZoneInfo.StandardName.Contains("Tehran") ||
 timeZoneInfo.Id.Contains("Iran") ||
 timeZoneInfo.Id.Contains("Tehran"));

            if (iranStandardTime == null)
            {
                TimeZoneInfo.GetSystemTimeZones().FirstOrDefault();
            }

            return TimeZoneInfo.ConvertTime(dateTime, iranStandardTime);
        }
    }
}
