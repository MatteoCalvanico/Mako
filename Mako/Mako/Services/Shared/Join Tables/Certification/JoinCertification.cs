using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mako.Services.Shared
{
    [PrimaryKey(nameof(WorkerCf), nameof(CertificationId))] // Define composite primary key
    public class JoinCertification : JoinBase
    {
        [ForeignKey("Certification")]
        public int CertificationId { get; set; } // Foreign key to Certification
        public Certification Certification { get; set; } // Navigation property to Certification
    }
}
