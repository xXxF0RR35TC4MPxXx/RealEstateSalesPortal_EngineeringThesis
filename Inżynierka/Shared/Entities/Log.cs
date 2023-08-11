using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Shared.Entities
{
    [Table("Log")]
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public DateTime Datetime { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string MachineName { get; set; }
        public string Logger { get; set; }
    }
}
