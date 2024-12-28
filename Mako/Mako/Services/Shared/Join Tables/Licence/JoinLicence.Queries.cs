using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class JoinLicencesSelectQuery
    {
        public Worker IdCurrentJoinW {  get; set; }
        public Licence IdCurrentJoinC { get; set; }
        public string Filter { get; set; }
    }

    public class JoinLicencesSelectDTO
    {
        public IEnumerable<JoinLicence> JoinLicences { get; set; }
        public int Count { get; set; }
        public class JoinLicence
        {
            public Worker Worker { get; set; }
            public Licence Licence { get; set; }
            public DateOnly ExpireDate { get; set; }
        }
    }

    public class JoinLicencesDetailQuery
    {
        public Worker Worker { get; set; }
        public Licence Licence { get;set; }
    }

    public class JoinLicencesDetailDTO
    {
        public Worker Worker { get; set; }
        public Licence Licence { get; set; }
        public DateOnly ExpireDate { get; set; }
    }

    public partial class SharedService
    {
        public async Task<JoinLicencesSelectDTO> Query(JoinLicencesSelectQuery qry)
        {
            var queryable = _dbContext.JoinLicences
                .Where(x => x.Worker != qry.IdCurrentJoinW && x.Licence != qry.IdCurrentJoinC);
            if (string.IsNullOrEmpty(qry.Filter) == false)
            {
                queryable = queryable.Where(x => x.Worker.Cf.Contains(qry.Filter, StringComparison.OrdinalIgnoreCase));
            }

            return new JoinLicencesSelectDTO
            {
                JoinLicences = await queryable
                .Select(x => new JoinLicencesSelectDTO.JoinLicence
                {
                    Worker = x.Worker,
                    Licence = x.Licence,
                    ExpireDate = x.ExpireDate
                })
                .ToArrayAsync(),
                Count = await queryable.CountAsync()
            };
        }
    }

}
