using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

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
