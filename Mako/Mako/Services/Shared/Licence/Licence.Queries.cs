using Mako.Services.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class LicencesSelectQuery
    {
        public int IdCurrentLicence { get; set; }
        public string Filter { get; set; }
    }

    public class LicencesSelectDTO
    {
        public IEnumerable<Licence> Licences { get; set; }
        public int Count { get; set; }

        public class Licence
        {
            public int Id { get; set; }
            public LicenceTypes Types { get; set; }
        }
    }

    public class LicencesDetailQuery
    {
        public int Id { get; set; }
    }

    public class LicencesDetailDTO
    {
        public int Id { get; set; }
        public LicenceTypes Types { get; set; }
    }

    public partial class SharedService
    {
        public async Task<LicencesSelectDTO> Query(LicencesSelectQuery qry)
        {
            var queryable = _dbContext.Licences
                .Where(x => x.Id != qry.IdCurrentLicence);
            if (string.IsNullOrWhiteSpace(qry.Filter) == false)
            {
                queryable = queryable.Where(x => x.Id.ToString() == qry.Filter);
            }

            return new LicencesSelectDTO
            {
                Licences = await queryable
                .Select(x => new LicencesSelectDTO.Licence
                {
                    Id = x.Id,
                })
                .ToArrayAsync(),
                Count = await queryable.CountAsync(),
            };
        }
        public async Task<LicencesDetailDTO> Query(LicencesDetailQuery qry)
        {
            return await _dbContext.Licences
                .Where(x => x.Id == qry.Id)
                .Select(x => new LicencesDetailDTO
                {
                    Id = x.Id,
                    Types = x.Types,
                })
                .FirstOrDefaultAsync();
        }
    }
}
