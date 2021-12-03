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
            TimeInputs = timeInputs.Select<TimeInput, TimeInputResponse>(t => new(t));

            _totalTime = TimeSpan.Zero;
            foreach (var input in timeInputs)
            {
                var dayTime = TimeSpan.Zero;
                if (input.Out.HasValue && input.In.HasValue)
                    dayTime += input.Out.Value - input.In.Value;
                if (input.LunchIn.HasValue && input.LunchOut.HasValue)
                {
                    dayTime -= input.LunchIn.Value - input.LunchOut.Value;
                }
                else if (input.Date.IsWorkDay())
                {
                    dayTime -= TimeSpan.FromMinutes(30);
                }
                _totalTime += dayTime;
            }

            var t = TimeSpan.Zero;
            foreach (var d in Helpers.EachDay(From, To))
            {
                if (d.IsWorkDay())
                {
                    t += TimeSpan.FromHours(8);
                }
            }
            t -= TimeSpan.FromHours(8) * timeInputs.Count(t => t.HomeOffice);
            this.TimeNeeded = t;

        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<TimeInputResponse> TimeInputs { get; set; }
        private TimeSpan _totalTime;
        /// <summary>
        /// Total time deduced by lunch time (30 minutes/day if not explicitly set)
        /// </summary>
        public TimeSpan TotalTime => _totalTime;

        /// <summary>
        /// Time needed for given date range. 
        /// 8 hours for each day except weekend, holidays and days marked as homeoffice
        /// </summary>
        public TimeSpan TimeNeeded { get; private set; }

    }
}
