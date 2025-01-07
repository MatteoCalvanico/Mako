using Mako.Services.Shared;
using Mako.Web.Areas.Worker.Models;
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
            model.ChangeViewModel.Id = Guid.NewGuid();
            model.ChangeViewModel.WorkerCf = await _sharedService.GetWorkerCfByEmailAsync(Identita.EmailUtenteCorrente);

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

                await _sharedService.Handle(command);
                return RedirectToAction("Index");
            }
            return View("Index", model);
        }
    }
}
