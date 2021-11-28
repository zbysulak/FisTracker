using System;

namespace FisTracker.Data
{
    public enum SesstionState { 
        Valid, Expired
    }
    public class Session
    {
        public string Id { get; set; }
        public SesstionState State { get; set; }
        public DateTime ValidTo { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

    }
}