using Mako.Services.Shared;
using Mako.Web.Areas.Worker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mako.Web.Areas.Worker.Controllers
{
    [Area("Worker")]
    public partial class ShiftController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public ShiftController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index()
        {
            var model = new ShiftViewModel
            {
                Message = "Hello, World!"
            };
            return View(model);
        }
    }
}
