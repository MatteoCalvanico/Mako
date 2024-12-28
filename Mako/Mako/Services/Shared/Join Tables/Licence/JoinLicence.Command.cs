using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateJoinLicenceCommand
    {
        public Worker Worker {  get; set; }
        public DateOnly ExpireDate { get; set; }
        public Licence Licence { get; set; }
    }

    public partial class SharedService
    {
        public async Task Handle(AddOrUpdateJoinLicenceCommand cmd)
        {
            var join = await _dbContext.JoinLicences
                .Where(x => x.Worker == cmd.Worker && x.Licence == cmd.Licence)
                .FirstOrDefaultAsync();
            if (join == null)
            {
                join = new JoinLicence
                {
                    Worker = cmd.Worker,
                    Licence = cmd.Licence,
                };
                _dbContext.JoinLicences.Add(join);
            }
            join.ExpireDate = cmd.ExpireDate;
            await _dbContext.SaveChangesAsync();
        }
    }
}
