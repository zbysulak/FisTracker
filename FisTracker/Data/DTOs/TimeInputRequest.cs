using System;
using System.ComponentModel.DataAnnotations;

namespace FisTracker.Data
{
    public class TimeInputRequest
    {
        public DateTime Date { get; set; }
        [RegularExpression(@"^\d{2}:\d{2}$",
         ErrorMessage = "Time is not in correct format")]
        public string In { get; set; }
        [RegularExpression(@"^\d{2}:\d{2}$",
         ErrorMessage = "Time is not in correct format")]
        public string Out { get; set; }
        [RegularExpression(@"^\d{2}:\d{2}$",
         ErrorMessage = "Time is not in correct format")]
        public string LunchIn { get; set; }
        [RegularExpression(@"^\d{2}:\d{2}$",
         ErrorMessage = "Time is not in correct format")]
        public string LunchOut { get; set; }
        public bool HomeOffice { get; set; }
    }
}
