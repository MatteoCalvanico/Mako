using Mako.Services.Shared;
using Microsoft.AspNetCore.Mvc;

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

        public virtual IActionResult Index()
        {
            return View();
        }
    }
}
