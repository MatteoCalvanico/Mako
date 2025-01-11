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

        public virtual async Task<IActionResult> Index(string shipName, DateTime shipDateArrival)
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
            } else if (model.ShiftViewModel == null)
            {
                Alerts.AddError(this, "Shift not found.");
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
            return null;
        }
    }
}