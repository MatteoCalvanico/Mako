using System;
using System.ComponentModel.DataAnnotations;

namespace Mako.Web.Areas.Worker.Models
{
    public class RequestViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Motivation cannot be longer than 500 characters.")]
        public string Motivation { get; set; }

        //[Required] <- This is not required in the form
        public string WorkerCf { get; set; }
    }
}