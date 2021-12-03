using System;
using System.ComponentModel.DataAnnotations;

namespace FisTracker.Data
{
    public class TimeInputResponse
    {
        public TimeInputResponse(TimeInput ti)
        {
            this.Date = ti.Date;
            this.In = ti.In.HasValue ? new(ti.In.Value) : null;
            this.Out = ti.Out.HasValue ? new(ti.Out.Value) : null;
            this.LunchOut = ti.LunchOut.HasValue ? new(ti.LunchOut.Value) : null;
            this.LunchIn = ti.LunchIn.HasValue ? new(ti.LunchIn.Value) : null;
            this.HomeOffice = ti.HomeOffice;

        }
        public DateTime Date { get; set; }
        public SimpleTime? In { get; set; }
        public SimpleTime? Out { get; set; }
        public SimpleTime? LunchIn { get; set; }
        public SimpleTime? LunchOut { get; set; }
        public bool HomeOffice { get; set; }
        public bool WorkDay => Helpers.IsWorkDay(this.Date);
    }
}
