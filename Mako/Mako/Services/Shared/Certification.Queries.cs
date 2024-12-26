using Mako.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mako.Services.Shared.CertificationsSelectDTO;

namespace Mako.Services.Shared
{
    public class CertificationsSelectQuery
    {
        public int idCurrentCertification { get; set; }
        public string Filter { get; set; }
    }

    public class CertificationsSelectDTO
    {
        public IEnumerable<Certification> Certifications { get; set; }
        public int Count { get; set; }

        public class Certification
        {
            public int Id { get; set; }
            public enum type { explosives, weapons, chemicals }
        }
    }

    public class CertificationsDetailQuery
    {
        public int Id { get; set; }
    }

    public class CertificationsDetailDTO
    {
        public int Id { get; set; }
        public enum type { explosives, weapons, chemicals }
    }

    public partial class SharedService
    {
        public async Task<CertificationsSelectDTO> Query(CertificationsSelectQuery qry)
        {
            var queryable = _dbContext.Certifications
                .Where(x => x.id != qry.idCurrentCertification);
            if (string.IsNullOrWhiteSpace(qry.Filter) == false)
            {
                queryable = queryable.Where(x => x.id.ToString() == qry.Filter);
            }

            return new CertificationsSelectDTO
            {
                Certifications = await queryable
                .Select(x => new CertificationsSelectDTO.Certification
                {
                    Id = x.id
                })
                .ToArrayAsync(),
                Count = await queryable.CountAsync(),
            };
        }
        public async Task<CertificationsDetailDTO> Query(CertificationsDetailQuery qry)
        {
            return await _dbContext.Certifications
                .Where(x => x.id == qry.Id)
                .Select(x => new CertificationsDetailDTO
                {
                    Id = x.id
                })
                .FirstOrDefaultAsync();
        }
    } 

}
