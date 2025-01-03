﻿using Mako.Services.Shared;
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
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Motivation = model.Motivation,
                    WorkerCf = model.WorkerCf,
                    State = RequestState.Unmanaged
                };

                await _sharedService.Handle(command);

                TempData["SuccessMessage"] = "Request added successfully!";
                return RedirectToAction("Index");
            }

            // Log the errors in the ModelState
            //foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //{
            //    Console.WriteLine(error.ErrorMessage);
            //}

            TempData["ErrorMessage"] = "There was an error adding the request.";
            return View("Index", model);
        }
    }
}