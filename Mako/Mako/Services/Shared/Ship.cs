using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class Ship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string name { get; set; }
        // We should set date_arrival as primary key as well
        public DateTime date_arrival { get; set; }
        public DateTime date_departure { get; set; }
        public int pier { get; set; }
        public TimeOnly time_estimation { get; set; }
        public string cargo_manifest { get; set; }
    }
}
