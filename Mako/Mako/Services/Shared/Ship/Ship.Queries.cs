using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{

    public class ShipsSelectQuery
    {
        public string CurrentShipName;
        public DateTime CurrentShipDateArrival;
    }

    public class ShipSelectDTO
    {
        public IEnumerable<Ship> Ships { get; set; }
        public int Count { get; set; }

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
        public TimeOnly TimeEstimation { get; set; }
        public string CargoManifest { get; set; }
    }

    public partial class  SharedService
    {
        // Recupero tutte le navi
        public async Task<List<Ship>> GetShipsAsync()
        {
            return await _dbContext.Ships
                                   .AsNoTracking()  // Non traccia le entità restituite
                                   .OrderBy(s => s.DateArrival) //  Ordina le navi per data di arrivo
                                   .ToListAsync();
        }
    }
}