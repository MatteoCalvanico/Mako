using System;

namespace Mako.Web.Features.ShiftDetails
{
    public class ShiftDetailsViewModel
    {
        public string Name { get; set; }
        public DateTime DateArrival { get; set; }
        public DateTime DateDeparture { get; set; }
        public int Pier { get; set; }
        public TimeSpan TimeEstimation { get; set; }
        public string CargoManifest { get; set; }
    }
}
