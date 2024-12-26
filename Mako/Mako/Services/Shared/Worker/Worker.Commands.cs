using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateWorkerCommand
    {
        public string Cf { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<Role> Roles { get; set; }
    }

    public partial class SharedService
    {
        public async Task<string> Handle(AddOrUpdateWorkerCommand cmd)
        {
            var worker = await _dbContext.Workers
                .Where(x => x.Cf == cmd.Cf)
                .FirstOrDefaultAsync();

            if (worker == null)
            {
                worker = new Worker
                {
                    Cf = cmd.Cf
                };
                _dbContext.Workers.Add(worker);
            }

            worker.Name = cmd.Name;
            worker.Surname = cmd.Surname;
            worker.Roles = cmd.Roles;

            await _dbContext.SaveChangesAsync();

            return worker.Cf;
        }
    }
}
