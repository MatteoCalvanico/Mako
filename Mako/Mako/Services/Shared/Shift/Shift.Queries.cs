using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class ShiftsSelectQuery
    {
        public Guid IdCurrentShift { get; set; }
        public string Filter { get; set; }
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

        public async Task<ShiftDetailDTO> GetShiftByIdAsync(Guid shiftId)
        {
            return await _dbContext.Shifts
                .Where(s => s.Id == shiftId)
                .Select(s => new ShiftDetailDTO
                {
                    Id = s.Id,
                    Pier = s.Pier,
                    Date = s.Date,
                    StartHour = s.StartHour,
                    EndHour = s.EndHour,
                    Workers = string.Join(", ", _dbContext.ShiftWorker
                        .Where(sw => sw.ShiftId == s.Id)
                        .Select(sw => sw.WorkerCf)
                        .ToList()),
                    ShipName = s.ShipName,
                    ShipDateArrival = s.ShipDateArrival
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ShiftsSelectDTO> SelectShiftsQuery(ShiftsSelectQuery query)
        {
            var shiftsQuery = _dbContext.Shifts.AsQueryable();

            if (query.IdCurrentShift != Guid.Empty)
            {
                shiftsQuery = shiftsQuery.Where(s => s.Id == query.IdCurrentShift);
            }

            if (!string.IsNullOrEmpty(query.Filter))
            {
                shiftsQuery = shiftsQuery.Where(s => s.ShipName.Contains(query.Filter));
            }

            var shifts = await shiftsQuery.Select(s => new ShiftsSelectDTO.Shift
            {
                Id = s.Id,
                Pier = s.Pier,
                Date = s.Date,
                StartHour = s.StartHour,
                EndHour = s.EndHour,
                ShipName = s.ShipName,
                ShipDateArrival = s.ShipDateArrival
            }).ToListAsync();

            return new ShiftsSelectDTO
            {
                Shifts = shifts,
                Count = shifts.Count
            };
        }

        public async Task<List<ShiftDetailDTO>> GetShiftsByShipAsync(string shipName, DateTime shipDateArrival)
        {
            return await _dbContext.Shifts
                .Where(s => s.ShipName == shipName && s.ShipDateArrival.Date == shipDateArrival.Date)
                .Select(s => new ShiftDetailDTO
                {
                    Id = s.Id,
                    Pier = s.Pier,
                    Date = s.Date,
                    StartHour = s.StartHour,
                    EndHour = s.EndHour,
                    Workers = string.Join(", ", _dbContext.ShiftWorker
                        .Where(sw => sw.ShiftId == s.Id)
                        .Select(sw => sw.WorkerCf)
                        .ToList()), //TODO: Add join with Worker table
                    ShipName = s.ShipName,
                    ShipDateArrival = s.ShipDateArrival
                })
                .ToListAsync();
        }
    }
}