using Mako.Services.Shared;
using Mako.Web.Areas.Worker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mako.Web.Areas.Worker.Controllers
{
    [Area("Worker")]
    public partial class AccountController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public AccountController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index()
        {
            var email = Identita.EmailUtenteCorrente;
            var workerCf = await _sharedService.GetWorkerCfByEmailAsync(email);
            bool isAdmin = await _sharedService.IsShiftAdminAsync(workerCf);

            ViewData["IsShiftAdmin"] = isAdmin;
            ViewBag.IsShiftAdmin = isAdmin;

            return View();
        }
    }
}
