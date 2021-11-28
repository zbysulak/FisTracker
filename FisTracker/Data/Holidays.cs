using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace FisTracker.Data
{
    public class Holidays
    {
        private static IDictionary<int, IEnumerable<DateTime>> _easter = new Dictionary<int, IEnumerable<DateTime>>() {
            { 2021, new List<DateTime>(){
                new DateTime(2021,4,2),
                new DateTime(2021,4,5)
            }},
            { 2022, new List<DateTime>(){
                new DateTime(2022,4,15),
                new DateTime(2022,4,18)
            }}
        };

        private static IEnumerable<DateTime> _staticHolidays = new List<DateTime>() {
                new DateTime(2021,1,1),
                new DateTime(2021,5,1),
                new DateTime(2021,5,8),
                new DateTime(2021,7,5),
                new DateTime(2021,7,6),
                new DateTime(2021,9,28),
                new DateTime(2021,10,28),
                new DateTime(2021,11,17),
                new DateTime(2021,12,24),
                new DateTime(2021,12,25),
                new DateTime(2021,12,26)
        };

        private static IEnumerable<DateTime> GetEaster(int year)
        {
            if (_easter.ContainsKey(year))
            {
                return _easter[year];
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Easter holidays not set for year {year}");
                return new List<DateTime>();
            }
        }

        public static IEnumerable<DateTime> GetHolidaysInMonth(int year, int month)
        {
            return _staticHolidays.Concat(GetEaster(year)).Where(d => d.Month == month && d.Year == year);
        }

        public static bool IsHoliday(DateTime day)
        {
            return _staticHolidays.Any(d => d.Day == day.Day && d.Month == day.Month) 
                || GetEaster(day.Year).Any(d => d == day.Date);
        }
    }
}
