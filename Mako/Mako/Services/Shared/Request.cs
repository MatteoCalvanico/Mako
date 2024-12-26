using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mako.Services.Shared
{
    public abstract class RequestBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime SentDate { get; set; }

        [Required]
        [MaxLength(150)]
        public string Motivation { get; set; }

        [Required]
        public RequestState State { get; set; }

        [Required]
        [ForeignKey("Worker")]
        public string Sender { get; set; }

        public Worker Worker { get; set; }
    }

    public enum RequestState
    {
        Accepted,
        Declined,
        Unmanaged
    }
}
