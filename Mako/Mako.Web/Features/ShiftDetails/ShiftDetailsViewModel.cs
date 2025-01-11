using Mako.Services.Shared;
using Mako.Web.Areas.Worker.Models;
using System;
using System.Collections.Generic;

namespace Mako.Web.Features.ShiftDetails
{
    public class CombinedViewModel
    {
        public List<ShiftDetailsViewModel> ShiftViewModel { get; set; }
        public ShipClickedDetailsViewModel ShipViewModel { get; set; }
    }

    public class ShiftDetailsViewModel
    {
        public Guid Id { get; set; }

        public int Pier { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
        public string ShipName { get; set; }
        public DateTime ShipDateArrival { get; set; }
        public List<Worker> Workers { get; set; }
    }

    public class ShipClickedDetailsViewModel
    {
        public string Name { get; set; }
        public DateTime DateArrival { get; set; }
        public DateTime DateDeparture { get; set; }
        public int Pier { get; set; }
        public TimeSpan TimeEstimation { get; set; }
        public string CargoManifest { get; set; }
    }
}
