using Mako.Services.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    public class Role
    {
        public int Id { get; set; }
        public RoleTypes Type { get; set; }
    }
}
