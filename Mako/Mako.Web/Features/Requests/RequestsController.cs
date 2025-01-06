using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mako.Services.Shared;
using Mako.Web.Areas;

namespace Mako.Web.Features.Requests
{
    public partial class RequestsController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public RequestsController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        public virtual async Task<IActionResult> Index()
        {
            

            return View("Requests");
        }
    }
}
