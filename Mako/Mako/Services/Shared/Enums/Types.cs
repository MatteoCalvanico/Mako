using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared.Enums
{
    public enum RoleTypes
    {
        Worker, 
        CraneOperator, 
        ShiftAdmin, 
        Driver,
        None = -1
    }

    public enum CertificationTypes
    {
        Explosives, 
        Weapons, 
        Chemicals,
        None = -1,
    }
}
