﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class ShiftsSelectQuery
    {
        public Guid IdCurrentShift { get; set; }
        public String Filter { get; set; }
    }

    public class ShiftsSelectDTO
    {
        public IEnumerable<Shift> Shifts { get; set; }
        public int Count { get; set; }

        public class Shift
        {
            public Guid Id { get; set; }
            public int Pier { get; set; }
            public DateOnly Date { get; set; }
            public TimeOnly StartHour { get; set; }
            public TimeOnly EndHour { get; set; }
            public string ShipName { get; set; }
            public DateTime ShipDateArrival { get; set; }
        }
    }

    public class ShiftDetailQuery
    {
        public Guid Id { get; set; }
    }

    public class ShiftDetailDTO
    {
        public Guid Id { get; set; }
        public int Pier { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
        public string Workers { get; set; }
        public string ShipName { get; set; }
        public DateTime ShipDateArrival { get; set; }
    }

    public class CustomShift
    {
        public Guid Id { get; set; }
        public int Pier { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartHour { get; set; }
        public TimeOnly EndHour { get; set; }
    }


    public partial class SharedService
    {
        public async Task<List<CustomShift>> GetShiftsByIdsAsync(List<Guid> shiftIds)
        {
            return await _dbContext.Shifts
                .Where(s => shiftIds.Contains(s.Id))
                .Select(s => new CustomShift
                {
                    Id = s.Id,
                    Pier = s.Pier,
                    Date = s.Date,
                    StartHour = s.StartHour,
                    EndHour = s.EndHour
                })
                .ToListAsync();
        }
    }
}
