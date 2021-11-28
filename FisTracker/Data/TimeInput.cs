﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FisTracker.Data
{
    public class TimeInput
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan In { get; set; }
        public TimeSpan Out { get; set;}
        public TimeSpan LunchIn { get; set; }
        public TimeSpan LunchOut { get; set; }
        public bool HomeOffice { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}