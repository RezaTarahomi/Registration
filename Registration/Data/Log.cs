using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Data
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public DateTime date { get; set; }
        public string level { get; set; }
        public string message { get; set; }
        public string machinename { get; set; }
        public string logger { get; set; }
    }
}
