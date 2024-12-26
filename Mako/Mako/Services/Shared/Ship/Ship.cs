using Microsoft.EntityFrameworkCore;
using System;

namespace Mako.Services.Shared
{
    [PrimaryKey(nameof(Name), nameof(DateArrival))]
    public class Ship
    {
        public string Name { get; set; }
        public DateTime DateArrival { get; set; }
        public DateTime DateDeparture { get; set; }
        public int Pier { get; set; }
        public TimeOnly TimeEstimation { get; set; }
        public string CargoManifest { get; set; }
    }
}
