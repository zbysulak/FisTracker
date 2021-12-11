using System;

namespace FisTracker.Data
{
    public class ParseResult : MessageResult
    {
        public DateTime MinDate { get; set; } = DateTime.MaxValue;
        public DateTime MaxDate { get; set; }
    }
}
