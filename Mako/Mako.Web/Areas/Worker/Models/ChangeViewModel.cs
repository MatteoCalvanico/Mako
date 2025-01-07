using System.ComponentModel.DataAnnotations;
using System;

namespace Mako.Web.Areas.Worker.Models
{
    public class ChangeViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Motivation cannot be longer than 500 characters.")]
        public string Motivation { get; set; }

        public string Operation { get; set; }

        //[Required] <- This is not required in the form
        public string WorkerCf { get; set; }
        public Guid ShiftId { get; set; }
    }
}
