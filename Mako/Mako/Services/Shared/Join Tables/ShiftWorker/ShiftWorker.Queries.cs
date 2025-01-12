using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class ShiftWorkerSelectQuery
    {
        public Guid? ShiftId { get; set; }
        public string WorkerCf { get; set; }
        public string Filter { get; set; }
    }

    public class ShiftWorkerSelectDTO
    {
        public IEnumerable<ShiftWorkerDTO> ShiftWorkers { get; set; }
        public int Count { get; set; }

        public class ShiftWorkerDTO
        {
            public Guid ShiftId { get; set; }
            public string WorkerCf { get; set; }
        }
    }

    public class ShiftWorkerDetailQuery
    {
        public Guid ShiftId { get; set; }
        public string WorkerCf { get; set; }
    }

    public class ShiftWorkerDetailDTO
    {
        public Guid ShiftId { get; set; }
        public string WorkerCf { get; set; }
    }

    public partial class SharedService
    {
        public async Task<ShiftWorkerSelectDTO> Query(ShiftWorkerSelectQuery qry)
        {
            var queryable = _dbContext.ShiftWorker.AsQueryable();

            if (qry.ShiftId.HasValue)
            {
                queryable = queryable.Where(x => x.ShiftId == qry.ShiftId.Value);
            }

            if (!string.IsNullOrEmpty(qry.WorkerCf))
            {
                queryable = queryable.Where(x => x.WorkerCf == qry.WorkerCf);
            }

            if (!string.IsNullOrEmpty(qry.Filter))
            {
                queryable = queryable.Where(x => x.WorkerCf.Contains(qry.Filter));
            }

            return new ShiftWorkerSelectDTO
            {
                ShiftWorkers = await queryable
                    .Select(x => new ShiftWorkerSelectDTO.ShiftWorkerDTO
                    {
                        ShiftId = x.ShiftId,
                        WorkerCf = x.WorkerCf
                    })
                    .ToArrayAsync(),
                Count = await queryable.CountAsync()
            };
        }

        public async Task<ShiftWorkerDetailDTO> Query(ShiftWorkerDetailQuery qry)
        {
            var join = await _dbContext.ShiftWorker
                .Where(x => x.ShiftId == qry.ShiftId && x.WorkerCf == qry.WorkerCf)
                .FirstOrDefaultAsync();

            if (join == null)
            {
                return null;
            }

            return new ShiftWorkerDetailDTO
            {
                ShiftId = join.ShiftId,
                WorkerCf = join.WorkerCf
            };
        }

        public async Task<List<Guid>> GetShiftIdsByWorkerAsync(string workerCf)
        {
            return await _dbContext.ShiftWorker
                .Where(sw => sw.WorkerCf == workerCf)
                .Select(sw => sw.ShiftId)
                .ToListAsync();
        }

        public async Task<List<string>> GetWorkerCfsByShiftAsync(Guid shiftId)
        {
            return await _dbContext.ShiftWorker
                 .Where(sw => sw.ShiftId == shiftId)
                 .Select(sw => sw.WorkerCf)
                 .ToListAsync();
        }

        public async Task RemoveShiftWorker(Guid shiftId, string workerCf)
        {
            var shiftWorker = await _dbContext.ShiftWorker
                .FirstOrDefaultAsync(sw => sw.ShiftId == shiftId && sw.WorkerCf == workerCf);
            if (shiftWorker != null)
            {
                _dbContext.ShiftWorker.Remove(shiftWorker);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Worker>> GetFreeWorkersForShiftAsync(Guid shiftId)
        {
            // Get the shift details
            var shift = await _dbContext.Shifts
                .FirstOrDefaultAsync(s => s.Id == shiftId);

            if (shift == null)
                return new List<Worker>();

            // Get all workers that are already assigned to this shift
            var workersInCurrentShift = await _dbContext.ShiftWorker
                .Where(sw => sw.ShiftId == shiftId)
                .Select(sw => sw.WorkerCf)
                .ToListAsync();

            // Get all workers that are assigned to other shifts on the same date with overlapping times
            var busyWorkerCfs = await _dbContext.Shifts
                .Where(s => s.Date == shift.Date && s.Id != shiftId) // Exclude current shift
                .Where(s =>
                    (s.StartHour <= shift.EndHour && s.EndHour >= shift.StartHour) // Overlapping time check
                )
                .Join(_dbContext.ShiftWorker,
                    s => s.Id,
                    sw => sw.ShiftId,
                    (s, sw) => sw.WorkerCf)
                .Distinct()
                .ToListAsync();

            // Combine both lists of unavailable workers
            var unavailableWorkerCfs = workersInCurrentShift.Union(busyWorkerCfs).Distinct();

            // Get all workers that are not in either list
            var freeWorkers = await _dbContext.Workers
                .Where(w => !unavailableWorkerCfs.Contains(w.Cf))
                .ToListAsync();

            return freeWorkers;
        }
    }
}

