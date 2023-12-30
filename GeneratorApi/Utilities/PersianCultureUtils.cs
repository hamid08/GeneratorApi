using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace GeneratorApi.Utilities
{
    public static class PersianCultureUtils
    {
        /// <summary>
        /// معادل فارسی روزهای هفته میلادی
        /// </summary>
        public static readonly IDictionary<DayOfWeek, string> PersianDayWeekNames = new Dictionary<DayOfWeek, string>
             {
                {DayOfWeek.Saturday, "شنبه"},
                {DayOfWeek.Sunday,  "یک شنبه"},
                {DayOfWeek.Monday,  "دو شنبه"},
                {DayOfWeek.Tuesday, "سه شنبه"},
                {DayOfWeek.Wednesday, "چهار شنبه"},
                {DayOfWeek.Thursday, "پنج شنبه"},
                {DayOfWeek.Friday, "جمعه"}
             };

        /// <summary>
        /// عدد به حروف روزهای شمسی
        /// </summary>
        public static readonly IDictionary<int, string> PersianMonthDayNumberNames = new Dictionary<int, string>
              {
                 { 1, "یکم" },
                 { 2, "دوم" },
                 { 3, "سوم" },
                 { 4, "چهارم" },
                 { 5, "پنجم" },
                 { 6, "ششم" },
                 { 7, "هفتم" },
                 { 8, "هشتم" },
                 { 9, "نهم" },
                 { 10, "دهم" },
                 { 11, "یازدهم" },
                 { 12, "دوازدهم" },
                 { 13, "سیزدهم" },
                 { 14, "چهاردهم" },
                 { 15, "پانزدهم" },
                 { 16, "شانزدهم" },
                 { 17, "هفدهم" },
                 { 18, "هجدهم" },
                 { 19, "نوزدهم" },
                 { 20, "بیستم" },
                 { 21, "بیست یکم" },
                 { 22, "بیست دوم" },
                 { 23, "بیست سوم" },
                 { 24, "بیست چهارم" },
                 { 25, "بیست پنجم" },
                 { 26, "بیست ششم" },
                 { 27, "بیست هفتم" },
                 { 28, "بیست هشتم" },
                 { 29, "بیست نهم" },
                 { 30, "سی‌ام" },
                 { 31, "سی یکم" }
              };

        /// <summary>
        /// نام فارسی ماه‌های شمسی
        /// </summary>
        public static readonly IDictionary<int, string> PersianMonthNames = new Dictionary<int, string>
              {
                {1, "فروردین"},
                {2, "اردیبهشت"},
                {3, "خرداد"},
                {4, "تیر"},
                {5, "مرداد"},
                {6, "شهریور"},
                {7, "مهر"},
                {8, "آبان"},
                {9, "آذر"},
                {10, "دی"},
                {11, "بهمن"},
                {12, "اسفند"}
              };

        private static readonly Lazy<CultureInfo> _cultureInfoBuilder =
                        new Lazy<CultureInfo>(GetPersianCulture, LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// وهله‌ی یکتای فرهنگ فارسی سفارشی سازی شده
        /// </summary>
        public static CultureInfo Instance { get; } = _cultureInfoBuilder.Value;

        /// <summary>
        /// اصلاح تقویم فرهنگ فارسی
        /// </summary>
        public static CultureInfo GetPersianCulture()
        {
            var persianCulture = new CultureInfo("fa-IR")
            {
                DateTimeFormat =
                     {
                          AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" },
                          AbbreviatedMonthGenitiveNames =
                                new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", string.Empty },
                          AbbreviatedMonthNames =
                                new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", string.Empty },
                          AMDesignator = "ق.ظ",
                          CalendarWeekRule = CalendarWeekRule.FirstDay,
                    //DateSeparator = "؍",
                    DayNames = new[] { "یکشنبه", "دوشنبه", "سه‌شنبه", "چهار‌شنبه", "پنجشنبه", "جمعه", "شنبه" },
                          FirstDayOfWeek = DayOfWeek.Saturday,
                          FullDateTimePattern = "dddd dd MMMM yyyy",
                          LongDatePattern = "dd MMMM yyyy",
                          LongTimePattern = "h:mm:ss tt",
                          MonthDayPattern = "dd MMMM",
                          MonthGenitiveNames =
                                new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", string.Empty },
                          MonthNames =
                                new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", string.Empty },
                          PMDesignator = "ب.ظ",
                          ShortDatePattern = "yyyy/MM/dd",
                          ShortestDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" },
                          ShortTimePattern = "HH:mm",
                    //TimeSeparator = ":",
                    YearMonthPattern = "MMMM yyyy"
                     }
            };

            var persianCalendar = new PersianCalendar();
            var fieldInfo = persianCulture.GetType()
                                                    .GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo?.SetValue(persianCulture, persianCalendar);

            var info = persianCulture.DateTimeFormat.GetType()
                                                                 .GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            info?.SetValue(persianCulture.DateTimeFormat, persianCalendar);

            persianCulture.NumberFormat.NumberDecimalSeparator = "/";
            persianCulture.NumberFormat.NumberNegativePattern = 0;

            return persianCulture;
        }
    }
}
