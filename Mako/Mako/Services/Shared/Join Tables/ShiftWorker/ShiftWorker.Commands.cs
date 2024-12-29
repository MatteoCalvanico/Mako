using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateShiftWorkerCommand
    {
        public Guid ShiftId { get; set; }
        public string WorkerCf { get; set; }
    }

    public partial class SharedService
    {
        public async Task Handle(AddOrUpdateShiftWorkerCommand cmd)
        {
            var join = await _dbContext.ShiftWorker
                .Where(x => x.ShiftId == cmd.ShiftId && x.WorkerCf == cmd.WorkerCf)
                .FirstOrDefaultAsync();
            if (join == null)
            {
                join = new ShiftWorker
                {
                    ShiftId = cmd.ShiftId,
                    WorkerCf = cmd.WorkerCf
                };
                _dbContext.ShiftWorker.Add(join);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}

