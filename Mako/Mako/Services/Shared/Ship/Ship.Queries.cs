using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class ShipsSelectQuery
    {
        public string CurrentShipName { get; set; }
        public DateTime? CurrentShipDateArrival { get; set; }
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

    public partial class SharedService
    {
        public async Task<ShipSelectDTO> SelectShipsQuery(ShipsSelectQuery query)
        {
            var shipsQuery = _dbContext.Ships.AsQueryable();

            if (!string.IsNullOrEmpty(query.CurrentShipName))
            {
                shipsQuery = shipsQuery.Where(s => s.Name == query.CurrentShipName);
            }

            if (query.CurrentShipDateArrival.HasValue)
            {
                var truncatedDateArrival = query.CurrentShipDateArrival.Value.Date; // Truncate time part of the date to match the database
                shipsQuery = shipsQuery.Where(s => s.DateArrival.Date == truncatedDateArrival);
            }

            var ships = await shipsQuery.Select(s => new ShipSelectDTO.Ship
            {
                Name = s.Name,
                DateArrival = s.DateArrival,
                DateDeparture = s.DateDeparture,
                Pier = s.Pier,
                TimeEstimation = s.TimeEstimation,
                CargoManifest = s.CargoManifest
            }).ToListAsync();

            return new ShipSelectDTO
            {
                Ships = ships,
                Count = ships.Count
            };
        }
    }
}