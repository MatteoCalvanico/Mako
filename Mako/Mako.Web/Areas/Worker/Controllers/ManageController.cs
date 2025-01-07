using Mako.Services.Shared;
using Mako.Web.Areas.Worker.Models;
using Mako.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;

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
            model.Id = Guid.NewGuid();
            model.WorkerCf = await _sharedService.GetWorkerCfByEmailAsync(Identita.EmailUtenteCorrente);

            if (ModelState.IsValid)
            {
                var command = new AddOrUpdateRequestHolidayCommand
                {
                    Id = model.Id,
                    StartDate = model.StartDate ?? DateTime.MinValue,
                    EndDate = model.EndDate ?? DateTime.MinValue,
                    Motivation = model.Motivation,
                    WorkerCf = model.WorkerCf,
                    State = RequestState.Unmanaged
                };

                try
                {
                    await _sharedService.Handle(command);
                    Alerts.AddSuccess(this, Identita.EmailUtenteCorrente + ", your request has been added");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Alerts.AddError(this, "Error: " + ex.Message);
                }
            }
            Alerts.AddError(this, "Model is invalid");
            return View("Index", model);
        }
    }
}