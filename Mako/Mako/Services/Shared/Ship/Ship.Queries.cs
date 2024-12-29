using System;
using System.Collections.Generic;

namespace Mako.Services.Shared
{

    public class ShipsSelectQuery
    {
        public string CurrentShipName;
        public DateTime CurrentShipDateArrival;
    }

    public class ShipSelectDTO
    {
        public IEnumerable<Shift> Ships { get; set; }
        public int Count { get; set; }

        public class Ship 
        {
            public string Name { get; set; }
            public DateTime DateArrival { get; set; }
            public DateTime DateDeparture { get; set; }
            public int Pier { get; set; }
            public TimeSpan TimeEstimation { get; set; }
            public string CargoManifest { get; set; }
        }
    }

    public class ShipDetailQuery 
    {
        public string ShipName { get; set; }
        public DateTime DateArrival { get; set; }
    }

    public class ShipDetailDTO
    {
        public string Name { get; set; }
        public DateTime DateArrival { get; set; }
        public DateTime DateDeparture { get; set; }
        public int Pier { get; set; }
        public TimeSpan TimeEstimation { get; set; }
        public string CargoManifest { get; set; }
    }
}