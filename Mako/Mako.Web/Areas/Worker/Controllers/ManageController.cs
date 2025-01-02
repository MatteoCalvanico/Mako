using Mako.Services.Shared;
using Mako.Web.Areas.Worker.Models;
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
            return View(new RequestViewModel());
        }

        [HttpPost]
        public virtual async Task<IActionResult> AddRequest(RequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddOrUpdateRequestHolidayCommand
                {
                    Id = model.Id,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Motivation = model.Motivation,
                    WorkerCf = model.WorkerCf,
                    State = RequestState.Unmanaged
                };

                await _sharedService.Handle(command);

                TempData["SuccessMessage"] = "Request added!";
                return RedirectToAction("Index");
            }

            return View("Index", model);
        }
    }
}