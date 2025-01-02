using Mako.Services.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mako.Web.Areas.Worker.Controllers
{
    [Area("Worker")]
    public partial class ManageController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public ManageController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        public virtual async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
