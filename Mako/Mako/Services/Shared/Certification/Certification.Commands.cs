using Mako.Services.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateCertificationsCommand
    {
        public int Id { get; set; }
        public CertificationTypes Types { get; set; }
    }

    public partial class SharedService
    {
        public async Task<int> Handle(AddOrUpdateCertificationsCommand cmd)
        {
            var certification = await _dbContext.Certifications
                .Where(x => x.Id == cmd.Id)
                .FirstOrDefaultAsync();

            if (certification == null)
            {
                certification = new Certification
                {
                    Id = cmd.Id,
                    Types = cmd.Types,
                };
                _dbContext.Certifications.Add(certification);
            }

            await _dbContext.SaveChangesAsync();

            return certification.Id;
        }
    }
}
