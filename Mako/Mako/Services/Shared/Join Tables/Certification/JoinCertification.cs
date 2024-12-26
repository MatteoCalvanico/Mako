using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    [PrimaryKey(nameof(Certification))] // The other primary key is Worker (see BaseJoin)
    public class JoinCertification : JoinBase
    {
        public Certification Certification { get; set; }
    }
}
