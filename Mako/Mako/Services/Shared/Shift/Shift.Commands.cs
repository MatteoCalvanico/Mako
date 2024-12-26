﻿using Microsoft.EntityFrameworkCore;
using System;
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
        public Worker Workers { get; set; }
        public Ship Ship { get; set; }
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

    }

}