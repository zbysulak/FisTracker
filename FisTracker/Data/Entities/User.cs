
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FisTracker.Data
{
    [Table("users")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public IEnumerable<TimeInput> TimeInputs { get; set; }
    }
}