using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

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
