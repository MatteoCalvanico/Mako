using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateWorkerCommand
    {
        public string? cf { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        // TODO: public ICollection<Role> roles { get; set; };
    }

    public partial class SharedService
    {
        public async Task<string> Handle(AddOrUpdateWorkerCommand cmd)
        {
            var worker = await _dbContext.Workers
                .Where(x => x.cf == cmd.cf)
                .FirstOrDefaultAsync();

            if (worker == null)
            {
                worker = new Worker
                {
                    cf = cmd.cf
                };
                _dbContext.Workers.Add(worker);
            }

            worker.name = cmd.name;
            worker.surname = cmd.surname;
            // worker.roles = cmd.roles;

            await _dbContext.SaveChangesAsync();

            return worker.cf;
        }
    }
}
