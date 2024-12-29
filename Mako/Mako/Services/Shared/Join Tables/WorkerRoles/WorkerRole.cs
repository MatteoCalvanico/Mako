using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mako.Services.Shared
{
    public class WorkerRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Worker")]
        public string WorkerCf { get; set; }
        public Worker Worker { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
