using System;
using System.Collections.Generic;

namespace FisTracker
{
    public static class Helpers
    {
        public static IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }

        public static bool IsWorkDay(this DateTime d) {
            return d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday && !Data.Holidays.IsHoliday(d);
        }
    }
}
