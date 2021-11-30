using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FisTracker.Data
{
    public enum SessionState { 
        Valid, Expired
    }
    [Table("sessions")]
    public class Session
    {
        public string Id { get; set; }
        public SessionState State { get; set; }
        public DateTime ValidTo { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

    }
}