using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mako.Services.Shared
{
    public class ShiftWorker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Shift")]
        public Guid ShiftId { get; set; }
        public Shift Shift { get; set; }

        [Required]
        [ForeignKey("Worker")]
        public string WorkerCf { get; set; }
        public Worker Worker { get; set; }
    }
}
