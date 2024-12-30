using Mako.Infrastructure.AspNetCore;
using Mako.Services.Shared;
using Mako.Web.Areas.Example.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Mako.Web.Areas.Worker.Controllers
{
    [Area("Worker")]
    public partial class ShiftController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public ShiftController(SharedService sharedService, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _sharedService = sharedService;
            _sharedLocalizer = sharedLocalizer;

            ModelUnbinderHelpers.ModelUnbinders.Add(typeof(IndexViewModel), new SimplePropertyModelUnbinder());
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index(IndexViewModel model)
        {
            var users = await _sharedService.Query(model.ToUsersIndexQuery());
            model.SetUsers(users);

            return View();
        }
    }
}
