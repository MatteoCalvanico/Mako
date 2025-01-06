using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateShiftCommand
    {
        public Guid Id { get; set; }
        public int Pier { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
        public string ShipName { get; set; }
        public DateTime ShipDateArrival { get; set; }
    }
    public class GetShiftsByIdsCommand
    {
        public List<Guid> ShiftIds { get; set; }
    }

    public partial class SharedService
    {
        public async Task<Guid> Handle(AddOrUpdateShiftCommand cmd)
        {
            var shift = await _dbContext.Shifts
                .Where(x => x.Id == cmd.Id)
                .FirstOrDefaultAsync();

            // Add a new shift if it doesn't exist
            if (shift == null)
            {
                shift = new Shift
                {
                    Id = Guid.NewGuid(),
                    Pier = cmd.Pier,
                    Date = cmd.Date,
                    StartHour = cmd.StartHour,
                    EndHour = cmd.EndHour,
                    ShipName = cmd.ShipName,
                    ShipDateArrival = cmd.ShipDateArrival
                };
                _dbContext.Shifts.Add(shift);
            }
            else
            {
                // Update existing shift details
                shift.Pier = cmd.Pier;
                shift.Date = cmd.Date;
                shift.StartHour = cmd.StartHour;
                shift.EndHour = cmd.EndHour;
            }

            await _dbContext.SaveChangesAsync();

            return shift.Id;
        }
        public async Task<List<CustomShift>> Handle(GetShiftsByIdsCommand cmd)
        {
            return await GetShiftsByIdsAsync(cmd.ShiftIds);
        }
    }
}
