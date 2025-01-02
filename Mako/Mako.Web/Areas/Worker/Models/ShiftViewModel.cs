using Mako.Services.Shared;
using System;
using System.Collections.Generic;

namespace Mako.Web.Areas.Worker.Models
{
    public class ShiftViewModel
    {
        public List<CustomShift> Shifts { get; set; } = new List<CustomShift>();
    }

    public class Shift
    {
        public Guid Id { get; set; }
        public int Pier { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
    }
}
