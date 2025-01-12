using Mako.Services.Shared;
using Mako.Web.Areas;
using Mako.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mako.Web.Features.ShiftDetails
{
    public partial class ShiftDetailsController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;
        public ShiftDetailsController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        public virtual async Task<IActionResult> Index(string shipName, DateTime shipDateArrival, string filterType, string searchTerm)
        {
            var model = new CombinedViewModel
            {
                ShipViewModel = await GetShipDetailsById(shipName, shipDateArrival),
                ShiftViewModel = await GetShiftsDetailsByShip(shipName, shipDateArrival)
            };

            if (model.ShipViewModel == null)
            {
                Alerts.AddError(this, "Ship not found.");
                return RedirectToAction("Index", "Shifts");
            }
            else if (model.ShiftViewModel == null)
            {
                Alerts.AddError(this, "Shift not found.");
            }

            // Apply filters if they exist
            if (!string.IsNullOrEmpty(searchTerm) && model.ShiftViewModel != null)
            {
                model.ShiftViewModel = filterType?.ToLower() switch
                {
                    "date" => model.ShiftViewModel
                        .Where(s => s.Date.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList(),
                    "pier" => model.ShiftViewModel
                        .Where(s => s.Pier.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList(),
                    "worker" => model.ShiftViewModel
                        .Where(s => s.Workers.Any(w =>
                            w.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            w.Surname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .ToList(),
                    "time" => model.ShiftViewModel
                        .Where(s => s.StartHour.ToString("HH:mm").Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   s.EndHour.ToString("HH:mm").Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList(),
                    _ => model.ShiftViewModel
                };
            }

            return View("ShiftDetails", model);
        }

        private async Task<ShipClickedDetailsViewModel> GetShipDetailsById(string shipName, DateTime shipDateArrival)
        {
            var shipSelectDTO = await _sharedService.SelectShipsQuery(new ShipsSelectQuery { CurrentShipName = shipName, CurrentShipDateArrival = shipDateArrival });
            var ship = shipSelectDTO.Ships.FirstOrDefault();

            if (ship == null)
            {
                return null;
            }

            return new ShipClickedDetailsViewModel
            {
                Name = ship.Name,
                DateArrival = ship.DateArrival,
                DateDeparture = ship.DateDeparture,
                Pier = ship.Pier,
                TimeEstimation = ship.TimeEstimation,
                CargoManifest = ship.CargoManifest
            };
        }

        private async Task<List<ShiftDetailsViewModel>> GetShiftsDetailsByShip(string shipName, DateTime shipDateArrival)
        {
            var shifts = await _sharedService.GetShiftsByShipAsync(shipName, shipDateArrival);
            var shiftsList = new List<ShiftDetailsViewModel>();

            foreach (var s in shifts)
            {
                var workerCfs = s.Workers.Split(',').Select(w => w.Trim()).ToList();
                var workerDetails = new List<Worker>();

                // Fetch worker details for each CF
                foreach (var cf in workerCfs)
                {
                    var workerDetail = await _sharedService.QueryWorkerDetail(new WorkersDetailQuery { Cf = cf });
                    if (workerDetail != null)
                    {
                        workerDetails.Add(new Worker
                        {
                            Cf = workerDetail.Cf,
                            Name = workerDetail.Name,
                            Surname = workerDetail.Surname
                        });
                    }
                }

                shiftsList.Add(new ShiftDetailsViewModel
                {
                    Id = s.Id,
                    Pier = s.Pier,
                    Date = s.Date,
                    StartHour = s.StartHour,
                    EndHour = s.EndHour,
                    ShipName = s.ShipName,
                    ShipDateArrival = s.ShipDateArrival,
                    Workers = workerDetails
                });
            }

            // Sort shifts by their date, putting closest dates first
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return shiftsList
                .OrderBy(s => Math.Abs((s.Date.DayNumber - today.DayNumber))) // Sort by absolute difference from today
                .ThenBy(s => s.Date) // Secondary sort by actual date
                .ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AddWorkerToShift(Guid shiftId, [FromForm(Name = "workerCfs[]")] List<string> workerCfs)
        {
            if (shiftId == Guid.Empty)
            {
                return BadRequest("Invalid shift ID.");
            }

            if (workerCfs == null || !workerCfs.Any())
            {
                return BadRequest("No workers selected.");
            }

            try
            {
                // First get the shift details to get the ship information
                var shift = await _sharedService.GetShiftByIdAsync(shiftId);
                if (shift == null)
                {
                    return NotFound("Shift not found.");
                }

                foreach (var cf in workerCfs.Where(cf => !string.IsNullOrEmpty(cf)))
                {
                    var command = new AddOrUpdateShiftWorkerCommand
                    {
                        ShiftId = shiftId,
                        WorkerCf = cf
                    };

                    await _sharedService.Handle(command);
                }

                // Get the updated shift details using the ship information
                var updatedShifts = await GetShiftsDetailsByShip(shift.ShipName, shift.ShipDateArrival);
                var updatedShift = updatedShifts.FirstOrDefault(s => s.Id == shiftId);

                return Json(new
                {
                    success = true,
                    message = "Workers added successfully",
                    workers = updatedShift?.Workers ?? new List<Worker>()
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { success = false, message = "An error occurred while adding workers to the shift." });
            }
        }

        [HttpGet("api/workers/free/{shiftId}")]
        public virtual async Task<IActionResult> GetFreeWorkers(Guid shiftId)
        {
            var freeWorkers = await _sharedService.GetFreeWorkersForShiftAsync(shiftId);
            return Ok(freeWorkers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AddShift(AddShiftViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = new AddOrUpdateShiftCommand
                {
                    Pier = model.Pier,
                    Date = model.Date,
                    StartHour = model.StartHour,
                    EndHour = model.EndHour,
                    ShipName = model.ShipName,
                    ShipDateArrival = model.ShipDateArrival
                };

                try
                {
                    await _sharedService.Handle(command);
                    return RedirectToAction("Index", new { shipName = model.ShipName, shipDateArrival = model.ShipDateArrival });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                }
            }

            return View("ShiftDetails", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> RemoveWorkerFromShift(WorkerToRemove worker)
        {
            if (worker.shiftId == Guid.Empty || string.IsNullOrEmpty(worker.workerCf))
            {
                return BadRequest("Invalid shift ID or worker CF.");
            }

            try
            {
                await _sharedService.RemoveShiftWorker(worker.shiftId, worker.workerCf);
                return Json(new { success = true, message = "Worker removed successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, new { success = false, message = "An error occurred while removing the worker from the shift." });
            }
        }
    }
}