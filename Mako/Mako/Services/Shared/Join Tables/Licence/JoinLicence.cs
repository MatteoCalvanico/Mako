using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
        [PrimaryKey(nameof(WorkerCf), nameof(LicenceId))] // Define composite primary key
        public class JoinLicence : JoinBase
        {
            [ForeignKey("Licence")]
            public int LicenceId { get; set; } // Foreign key to Licence
            public Licence Licence { get; set; } // Navigation property to Licence
        }
}
