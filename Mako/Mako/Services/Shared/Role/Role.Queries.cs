using Mako.Services.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class RolesSelectQuery
    {
        public string IdCurrentRole { get; set; }
        public string Filter {  get; set; }
    }

    public class RolesSelectDTO
    {
        public IEnumerable<Role> Roles { get; set; }
        public int Count { get; set; }

        public class Role
        {
            public int Id { get; set; }
            public RoleTypes Type { get; set; }
        }
    }

    public class RolesDetailQuery
    {
        public int Id { get; set; }
    }

    public class RolesDetailDTO
    {
        public int Id { get; set; }
        public RoleTypes Type { get; set; }
    }


    public partial class SharedService
    {
        /// <summary>
        /// Returns Role for a select field
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public async Task<RolesSelectDTO> Query(RolesSelectQuery qry)
        {
            var queryable = _dbContext.Roles
                .Where(x => x.Id.ToString() != qry.IdCurrentRole);

            if (string.IsNullOrWhiteSpace(qry.Filter) == false)
            {
                queryable = queryable.Where(x => x.Id.ToString().Contains(qry.Filter, StringComparison.OrdinalIgnoreCase));
            }

            return new RolesSelectDTO
            {
                Roles = await queryable
                .Select(x => new RolesSelectDTO.Role
                {
                    Id = x.Id

                })
                .ToArrayAsync(),
                Count = await queryable.CountAsync()
            };
        }

        /// <summary>
        /// Returns the detail of the role who matches the Id passed in the qry parameter
        /// </summary>
        /// <param name="qry"></param>
        /// <returns></returns>
        public async Task<RolesDetailDTO> Query(RolesDetailQuery qry)
        {
            return await _dbContext.Roles
                .Where(x => x.Id.ToString() == qry.Id.ToString())
                .Select(x => new RolesDetailDTO
                {
                    Id = x.Id,
                    Type = x.Type,
                })
                .FirstOrDefaultAsync();
        }
    }
}
