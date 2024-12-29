using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateShipCommand
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
        public async Task<string> Handle(AddOrUpdateShipCommand cmd)
        {
            var ship = await _dbContext.Ships
                        .Where(x => x.Name == cmd.Name && x.DateArrival == cmd.DateArrival)
                        .FirstOrDefaultAsync();

            if (ship == null)
            {
                ship = new Ship
                {
                    Name = cmd.Name,
                    DateArrival = cmd.DateArrival,
                    DateDeparture = cmd.DateDeparture,
                    Pier = cmd.Pier,
                    TimeEstimation = cmd.TimeEstimation,
                    CargoManifest = cmd.CargoManifest
                };
                _dbContext.Ships.Add(ship);
            } else
            {
                ship.Name = cmd.Name;
                ship.DateArrival = cmd.DateArrival;
                ship.DateDeparture = cmd.DateDeparture;
                ship.Pier = cmd.Pier;
                ship.TimeEstimation = cmd.TimeEstimation;
                ship.CargoManifest = cmd.CargoManifest;
            }

            await _dbContext.SaveChangesAsync();
            return ship.Name;
        }
    }

}
