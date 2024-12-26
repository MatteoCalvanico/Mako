using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class JoinCertificationsSelectQuery
    {
        public Worker IdCurrentJoinW { get; set; }
        public Certification IdCurrentJoinC { get; set; }
        public string Filter { get; set; }
    }

    public class JoinCertificationsSelectDTO
    {
        public IEnumerable<JoinCertification> JoinCertifications { get; set; }
        public int Count { get; set; }
        public class JoinCertification
        {
            public Worker Worker { get; set; }
            public Certification Certification { get; set; }
            public DateOnly ExpireDate { get; set; }
        }
    }

    public class JoinCertificationsDetailQuery
    {
        public Worker Worker { get; set; }
        public Certification Certification { get; set; }
    }

    public class JoinCertificationsDetailDTO
    {
        public Worker Worker { get; set; }
        public Certification Certification { get; set; }
        public DateOnly ExpireDate { get; set; }
    }


    public partial class SharedService
    {
        public async Task<JoinCertificationsSelectDTO> Query(JoinCertificationsSelectQuery qry)
        {
            var queryable = _dbContext.JoinCertifications
                .Where(x => x.Worker != qry.IdCurrentJoinW && x.Certification != qry.IdCurrentJoinC);

            if (string.IsNullOrEmpty(qry.Filter) == false)
            {
                queryable = queryable.Where(x => x.Worker.Cf.Contains(qry.Filter, StringComparison.OrdinalIgnoreCase) && x.Certification.Id.ToString().Contains(qry.Filter, StringComparison.OrdinalIgnoreCase));
            }

            return new JoinCertificationsSelectDTO
            {
                JoinCertifications = await queryable
                .Select(x => new JoinCertificationsSelectDTO.JoinCertification
                {
                    Worker = x.Worker,
                    Certification = x.Certification,
                    ExpireDate = x.ExpireDate
                })
                .ToArrayAsync(),
                Count = await queryable.CountAsync()
            };
        }
    }
}
