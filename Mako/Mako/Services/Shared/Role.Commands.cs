using Mako.Services.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class AddOrUpdateRoleCommand
    {
        public int? Id { get; set; }
        public RoleTypes Type { get; set; }
    }

    public partial class SharedService
    {
        public async Task<int> Handle(AddOrUpdateRoleCommand cmd)
        {
            var role = await _dbContext.Roles
                .Where(x => x.Id == cmd.Id)
                .FirstOrDefaultAsync();

            if (role == null)
            {
                role = new Role
                {
                    Id = cmd.Id ?? 999
                };
                _dbContext.Roles.Add(role);
            }

            role.Type = cmd.Type;

            await _dbContext.SaveChangesAsync();

            return role.Id;
        }
    }
}
