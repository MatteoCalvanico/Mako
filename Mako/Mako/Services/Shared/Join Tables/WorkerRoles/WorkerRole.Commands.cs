using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateWorkerRoleCommand
    {
        public string WorkerCf { get; set; }
        public int RoleId { get; set; }
    }

    public partial class SharedService
    {
        public async Task Handle(AddOrUpdateWorkerRoleCommand cmd)
        {
            var join = await _dbContext.WorkerRoles
                .Where(x => x.WorkerCf == cmd.WorkerCf && x.RoleId == cmd.RoleId)
                .FirstOrDefaultAsync();
            if (join == null)
            {
                join = new WorkerRole
                {
                    WorkerCf = cmd.WorkerCf,
                    RoleId = cmd.RoleId
                };
                _dbContext.WorkerRoles.Add(join);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
