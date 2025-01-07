using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mako.Services.Shared
{
    public class RequestChange : RequestBase
    {
        [Required]
        [ForeignKey("Shift")]
        public Guid ShiftId { get; set; }

        public Shift Shift { get; set; }
        public string WorkerName { get; set; }
        public string WorkerSurname { get; set; }
        public DateTime ShiftDate { get; set; }
        public int Pier { get; set; }
    }
}
