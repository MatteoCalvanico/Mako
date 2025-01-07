using Mako.Services.Shared;
using Mako.Web.Areas.Worker.Models;
using Mako.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var email = Identita.EmailUtenteCorrente;
            var workerCf = await _sharedService.GetWorkerCfByEmailAsync(email);
            var shiftIds = await _sharedService.Handle(new GetShiftIdsByWorkerCommand { WorkerCf = workerCf });
            var model = new CombinedViewModel
            {
                ShiftViewModel = new ShiftViewModel
                {
                    Shifts = await _sharedService.Handle(new GetShiftsByIdsCommand { ShiftIds = shiftIds })
                },
                ChangeViewModel = new ChangeViewModel()
            };
            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> AddRequest(CombinedViewModel model)
        {
            if (model == null || model.ChangeViewModel == null)
            {
                Alerts.AddError(this, "You need to select something");
                return View("Index", model);
            }

            model.ChangeViewModel.Id = Guid.NewGuid();
            var email = Identita?.EmailUtenteCorrente;

            if (string.IsNullOrEmpty(email))
            {
                Alerts.AddError(this, "Impossible to obtain user information");
                return View("Index", model);
            }

            model.ChangeViewModel.WorkerCf = await _sharedService.GetWorkerCfByEmailAsync(email);

            if (ModelState.IsValid)
            {
                var command = new AddOrUpdateRequestChangeCommand
                {
                    Id = model.ChangeViewModel.Id,
                    Motivation = model.ChangeViewModel.Operation + ' ' + model.ChangeViewModel.Motivation,
                    WorkerCf = model.ChangeViewModel.WorkerCf,
                    State = RequestState.Unmanaged,
                    ShiftId = model.ChangeViewModel.ShiftId
                };

                try
                {
                    await _sharedService.Handle(command);
                    Alerts.AddSuccess(this, email + ", your request has been added");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Alerts.AddError(this, "Error: " + ex.Message);
                }
            }
            else
            {
                Alerts.AddError(this, "Model is invalid");
            }

            return View("Index", model);
        }
    }
}
