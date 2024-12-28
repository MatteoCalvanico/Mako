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

    public enum LicenceTypes
    {
        A,
        A1,
        A2,
        AM,
        B,
        B1,
        BE,
        C,
        C1,
        CE,
        C1E,
        D,
        D1,
        DE,
        D1E,
        K,
        None = -1,
    }
}
