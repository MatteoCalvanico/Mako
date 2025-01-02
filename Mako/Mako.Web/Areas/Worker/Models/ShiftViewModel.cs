using Mako.Services.Shared;
using System;
using System.Collections.Generic;

namespace Mako.Web.Areas.Worker.Models
{
    public class ShiftViewModel
    {
        public List<CustomShift> Shifts { get; set; } = new List<CustomShift>();
    }
}
