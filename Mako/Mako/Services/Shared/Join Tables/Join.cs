using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mako.Services.Shared
{
    // Base class for all join tables
    [PrimaryKey(nameof(Worker))] // Need to specify the other primary key in the derived class
    public abstract class JoinBase
    {
        [ForeignKey("Worker")]
        public Worker Worker { get; set; }

        [Required]
        public DateOnly ExpireDate { get; set; }
    }
}
