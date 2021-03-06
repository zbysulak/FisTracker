using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FisTracker.Data
{
    [Table("timeinputs")]
    public class TimeInput
    {
        [Required]
        public DateTime Date { get; set; }
        public TimeSpan? In { get; set; }
        public TimeSpan? Out { get; set; }
        public TimeSpan? LunchIn { get; set; }
        public TimeSpan? LunchOut { get; set; }
        public bool HomeOffice { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}