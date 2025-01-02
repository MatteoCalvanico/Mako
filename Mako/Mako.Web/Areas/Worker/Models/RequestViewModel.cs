using System;
using System.ComponentModel.DataAnnotations;

namespace Mako.Web.Areas.Worker.Models
{
    public class RequestViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required] 
        public DateTime EndDate { get; set; }

        [Required]
        public string Motivation { get; set; }

        [Required]
        public string WorkerCf { get; set; }
    }
}