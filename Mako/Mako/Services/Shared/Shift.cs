using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class Shift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public int pier { get; set; }
        public DateOnly date { get; set; }
        public TimeOnly start_hour { get; set; }
        public TimeOnly end_hour { get; set; }
        //public ICollection<Worker> workers { get; set; } = new List<Workers>();
        public Ship ship {  get; set; }
    }
}
