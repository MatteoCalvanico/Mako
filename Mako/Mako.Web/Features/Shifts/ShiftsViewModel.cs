using System;
using System.Collections.Generic;

namespace Mako.Web.Features.Shifts
{
    public class ShiftViewModel
    {
        public Guid Id { get; set; }
        public int Pier { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
        public string ShipName { get; set; }
        public DateTime ShipDateArrival { get; set; }
    }

    public class ShiftsViewModel
    {
        public List<ShiftViewModel> Shifts { get; set; } = new List<ShiftViewModel>();
        
    }
}
