using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mako.Services.Shared
{
    public class Shift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int Pier { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }

        public string ShipName { get; set; }
        public DateTime ShipDateArrival { get; set; }
        [ForeignKey(nameof(ShipName) + "," + nameof(ShipDateArrival))]
        public Ship Ship { get; set; }
    }
}
