using System;

namespace FisTracker.Data
{
    public class SimpleTime
    {
        public SimpleTime(TimeSpan t)
        {
            Hours = t.Hours;
            Minutes = t.Minutes;
        }

        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string Formatted
        {
            get { return $"{Hours:00}:{Minutes:00}"; }
            set {
                if (TimeSpan.TryParse(value, out TimeSpan ts)){
                    this.Minutes = ts.Minutes;
                    this.Hours = ts.Hours;
                }
            }
        }
    }
}