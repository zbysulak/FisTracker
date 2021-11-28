
using System.Collections.Generic;

namespace FisTracker.Data
{
    public class User
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Password { get; set; }

        public IEnumerable<TimeInput> TimeInputs { get; set; }
    }
}