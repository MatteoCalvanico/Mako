using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class WorkerRoleSelectQuery
    {
        public string WorkerCf { get; set; }
        public int? RoleId { get; set; }
        public string Filter { get; set; }
    }

    public class WorkerRoleSelectDTO
    {
        public IEnumerable<WorkerRoleDTO> WorkerRoles { get; set; }
        public int Count { get; set; }

        public class WorkerRoleDTO
        {
            public string WorkerCf { get; set; }
            public int RoleId { get; set; }
        }
    }

    public class WorkerRoleDetailQuery
    {
        public string WorkerCf { get; set; }
        public int RoleId { get; set; }
    }

    public class WorkerRoleDetailDTO
    {
        public string WorkerCf { get; set; }
        public int RoleId { get; set; }
    }

    public partial class SharedService
    {
        public async Task<WorkerRoleSelectDTO> Query(WorkerRoleSelectQuery qry)
        {
            var queryable = _dbContext.WorkerRoles.AsQueryable();

            if (!string.IsNullOrEmpty(qry.WorkerCf))
            {
                queryable = queryable.Where(x => x.WorkerCf == qry.WorkerCf);
            }

            if (qry.RoleId.HasValue)
            {
                queryable = queryable.Where(x => x.RoleId == qry.RoleId.Value);
            }

            if (!string.IsNullOrEmpty(qry.Filter))
            {
                queryable = queryable.Where(x => x.WorkerCf.Contains(qry.Filter));
            }

            return new WorkerRoleSelectDTO
            {
                WorkerRoles = await queryable
                    .Select(x => new WorkerRoleSelectDTO.WorkerRoleDTO
                    {
                        WorkerCf = x.WorkerCf,
                        RoleId = x.RoleId
                    })
                    .ToArrayAsync(),
                Count = await queryable.CountAsync()
            };
        }

        public async Task<WorkerRoleDetailDTO> Query(WorkerRoleDetailQuery qry)
        {
            var join = await _dbContext.WorkerRoles
                .Where(x => x.WorkerCf == qry.WorkerCf && x.RoleId == qry.RoleId)
                .FirstOrDefaultAsync();

            if (join == null)
            {
                return null;
            }

            return new WorkerRoleDetailDTO
            {
                WorkerCf = join.WorkerCf,
                RoleId = join.RoleId
            };
        }
    }
}
