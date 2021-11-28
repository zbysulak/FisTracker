using System;
using System.Collections.Generic;
using System.Linq;

namespace FisTracker.Data
{
    public class TimeSheet
    {
        public TimeSheet(DateTime from, DateTime to, IEnumerable<TimeInput> timeInputs)
        {
            From = from;
            To = to;
            TimeInputs = timeInputs;
        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<TimeInput> TimeInputs { get; set; }

        /// <summary>
        /// Total time deduced by lunch time (30 minutes/day if not explicitly set)
        /// </summary>
        public TimeSpan TotalTime { get {
                var t = TimeSpan.Zero;
                foreach (var input in TimeInputs) {
                    var dayTime = TimeSpan.Zero;
                    if (input.Out.HasValue)
                        dayTime += input.Out.Value - input.In;
                    if (input.LunchIn.HasValue && input.LunchOut.HasValue) {
                        dayTime -= input.LunchIn.Value - input.LunchOut.Value;
                    }
                    else if(input.Date.IsWorkDay())
                    {
                        dayTime -= TimeSpan.FromMinutes(30);
                    }
                    t += dayTime;
                }
                return t;
            } }

        /// <summary>
        /// Time needed for given date range. 
        /// 8 hours for each day except weekend, holidays and days marked as homeoffice
        /// </summary>
        public TimeSpan TimeNeeded
        {
            get
            {
                var t = TimeSpan.Zero;
                foreach (var d in Helpers.EachDay(From, To)) {
                    if (d.IsWorkDay()) {
                        t += TimeSpan.FromHours(8);
                    }
                }
                t -= TimeSpan.FromHours(8)*this.TimeInputs.Count(t=>t.HomeOffice);
                return t;
            }
        }
         
    }
}
