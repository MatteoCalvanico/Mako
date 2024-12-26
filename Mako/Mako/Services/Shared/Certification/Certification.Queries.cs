using Mako.Services.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class CertificationsSelectQuery
    {
        public int IdCurrentCertification { get; set; }
        public string Filter { get; set; }
    }

    public class CertificationsSelectDTO
    {
        public IEnumerable<Certification> Certifications { get; set; }
        public int Count { get; set; }

        public class Certification
        {
            public int Id { get; set; }
            public CertificationTypes Types { get; set; }
        }
    }

    public class CertificationsDetailQuery
    {
        public int Id { get; set; }
    }

    public class CertificationsDetailDTO
    {
        public int Id { get; set; }
        public CertificationTypes Types { get; set; }
    }

    public partial class SharedService
    {
        public async Task<CertificationsSelectDTO> Query(CertificationsSelectQuery qry)
        {
            var queryable = _dbContext.Certifications
                .Where(x => x.Id != qry.IdCurrentCertification);
            if (string.IsNullOrWhiteSpace(qry.Filter) == false)
            {
                queryable = queryable.Where(x => x.Id.ToString() == qry.Filter);
            }

            return new CertificationsSelectDTO
            {
                Certifications = await queryable
                .Select(x => new CertificationsSelectDTO.Certification
                {
                    Id = x.Id
                })
                .ToArrayAsync(),
                Count = await queryable.CountAsync(),
            };
        }
        public async Task<CertificationsDetailDTO> Query(CertificationsDetailQuery qry)
        {
            return await _dbContext.Certifications
                .Where(x => x.Id == qry.Id)
                .Select(x => new CertificationsDetailDTO
                {
                    Id = x.Id,
                    Types = x.Types
                })
                .FirstOrDefaultAsync();
        }
    } 

}
