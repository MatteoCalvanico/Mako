using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateShiftCommand
    {
        public Guid id { get; set; }
        public int pier { get; set; }
        public DateOnly date { get; set; }
        public TimeOnly startHour { get; set; }
        public TimeOnly endHour { get; set; }
        //public Worker workers { get; set; }
        public Ship ship { get; set; }
    }

    public partial class SharedService
    {
        public async Task<Guid> Handle(AddOrUpdateShiftCommand cmd)
        {
            var shift = await _dbContext.Shifts
                .Where(x => x.id == cmd.id)
                .FirstOrDefaultAsync();

            // Add a new shift if it doesn't exist
            if (shift == null)
            {
                shift = new Shift
                {
                    id = Guid.NewGuid(),
                    pier = cmd.pier,
                    date = cmd.date,
                    start_hour = cmd.startHour,
                    end_hour = cmd.endHour,
                };
                _dbContext.Shifts.Add(shift);
            }
            else
            {
                // Update existing shift details
                shift.pier = cmd.pier;
                shift.date = cmd.date;
                shift.start_hour = cmd.startHour;
                shift.end_hour = cmd.endHour;
            }

            await _dbContext.SaveChangesAsync();

            return shift.id;
        }

    }

}
