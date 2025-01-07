using System;
using System.ComponentModel.DataAnnotations;

namespace Mako.Services.Shared
{
    public class RequestHoliday : RequestBase
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        public string WorkerName { get; set; }
        public string WorkerSurname { get; set; }
    }
}
