using System;
using System.Collections.Generic;

namespace Mako.Web.Features.Ship
{
    public class ShipViewModel
    {
        public string Name { get; set; }
        public DateTime DateArrival { get; set; }
        public DateTime DateDeparture { get; set; }
        public int Pier { get; set; }
        public TimeSpan TimeEstimation { get; set; }
        public string CargoManifest { get; set; }
    }

    public class ShipsViewModel
    {
        public List<ShipViewModel> Ships { get; set;} = new List<ShipViewModel>();
    }
}
