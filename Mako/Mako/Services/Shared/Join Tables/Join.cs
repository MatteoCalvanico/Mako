using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mako.Services.Shared
{
    // Base class for all join tables.
    // Remember to use [PrimaryKey] attribute to define composite primary key in derived classes
    public abstract class JoinBase
    {
        [ForeignKey("Worker")]
        public string WorkerCf { get; set; } // Foreign key to Worker
        public Worker Worker { get; set; } // Navigation property to Worker

        [Required]
        public DateOnly ExpireDate { get; set; }
    }
}
