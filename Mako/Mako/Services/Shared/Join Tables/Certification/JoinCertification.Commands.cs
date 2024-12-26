using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateJoinCertficationCommand
    {
        public Worker Worker { get; set; }
        public DateOnly ExpireDate { get; set; }
        public Certification Certification { get; set; }
    }

    public partial class SharedService
    {
        public async Task Handle(AddOrUpdateJoinCertficationCommand cmd)
        {
            var join = await _dbContext.JoinCertifications
                .Where(x => x.Worker == cmd.Worker && x.Certification == cmd.Certification)
                .FirstOrDefaultAsync();
            if (join == null)
            {
                join = new JoinCertification
                {
                    Worker = cmd.Worker,
                    Certification = cmd.Certification
                };
                _dbContext.JoinCertifications.Add(join);
            }
            join.ExpireDate = cmd.ExpireDate;
            await _dbContext.SaveChangesAsync();
        }
    }
}
