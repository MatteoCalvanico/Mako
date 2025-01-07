using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mako.Services.Shared
{
    public class AddOrUpdateRequestChangeCommand
    {
        public Guid? Id { get; set; }
        [Required]
        public Guid ShiftId { get; set; }
        [Required]
        public string Motivation { get; set; }
        [Required]
        public string WorkerCf { get; set; }
        public RequestState State { get; set; } = RequestState.Unmanaged;
    }

    public partial class SharedService
    {
        public async Task<Guid> Handle(AddOrUpdateRequestChangeCommand cmd)
        {
            var requestChange = await _dbContext.RequestsChanges
                .Where(x => x.Id == cmd.Id)
                .FirstOrDefaultAsync();

            if (requestChange == null)
            {
                requestChange = new RequestChange
                {
                    ShiftId = cmd.ShiftId,
                    Motivation = cmd.Motivation,
                    WorkerCf = cmd.WorkerCf,
                    State = cmd.State,
                    SentDate = DateTime.UtcNow
                };
                _dbContext.RequestsChanges.Add(requestChange);
            }
            else
            {
                requestChange.ShiftId = cmd.ShiftId;
                requestChange.Motivation = cmd.Motivation;
                requestChange.WorkerCf = cmd.WorkerCf;
                requestChange.State = cmd.State;
            }

            await _dbContext.SaveChangesAsync();

            return requestChange.Id;
        }
    }
}
