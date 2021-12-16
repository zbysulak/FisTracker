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
            this.AverageTimeNeeded = TimeSpan.Zero;

            this.TotalTimeWorked = TimeSpan.Zero;
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
                this.TotalTimeWorked += dayTime;
            }

            var workdays = Helpers.EachDay(From, To).Count(t => t.IsWorkDay());

            this.TotalTimeNeeded = TimeSpan.FromHours(8) * (workdays - timeInputs.Count(t => t.HomeOffice));

            if (from.Month == DateTime.Now.Month && to.Month == DateTime.Now.Month)
            {
                var lastInput = this.TimeInputs.OrderBy(t => t.Date).LastOrDefault();
                var lastDate = lastInput?.Date ?? from.AddDays(-1);
                var futureHO = this.TimeInputs.Where(t => t.Date > lastDate && t.HomeOffice).Count();
                this.AverageTimeNeeded = this.RemainingTimeNeeded /
                    (Helpers.EachDay(lastDate.AddDays(1), To).Count(d => d.IsWorkDay()) + futureHO);
            }

        }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<TimeInputResponse> TimeInputs { get; set; }
        /// <summary>
        /// Total time deduced by lunch time (30 minutes/day if not explicitly set)
        /// </summary>
        public TimeSpan TotalTimeWorked { get; private set; }

        /// <summary>
        /// Time needed for given date range. 
        /// 8 hours for each day except weekend, holidays and days marked as homeoffice
        /// </summary>
        public TimeSpan TotalTimeNeeded { get; private set; }

        public TimeSpan RemainingTimeNeeded => this.TotalTimeNeeded - this.TotalTimeWorked;

        /// <summary>
        /// Time needed to work extra/less for each remaining workday of given range
        /// </summary>
        public TimeSpan AverageTimeNeeded { get; private set; }

    }
}
