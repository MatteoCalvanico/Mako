using Mako.Services.Shared;
using Mako.Web.Areas;
using Mako.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var shipDetails = await GetShipDetailsById(shipName, shipDateArrival);
            if (shipDetails == null)
            {
                Alerts.AddError(this, "Ship not found.");
                return RedirectToAction("Index", "Shifts");
            }
            return View("ShiftDetails", shipDetails);
        }

        private async Task<ShiftDetailsViewModel> GetShipDetailsById(string shipName, DateTime shipDateArrival)
        {
            var shipSelectDTO = await _sharedService.SelectShipsQuery(new ShipsSelectQuery { CurrentShipName = shipName, CurrentShipDateArrival = shipDateArrival });
            var ship = shipSelectDTO.Ships.FirstOrDefault();

            if (ship == null)
            {
                return null;
            }

            return new ShiftDetailsViewModel
            {
                Name = ship.Name,
                DateArrival = ship.DateArrival,
                DateDeparture = ship.DateDeparture,
                Pier = ship.Pier,
                TimeEstimation = ship.TimeEstimation,
                CargoManifest = ship.CargoManifest
            };
        }
    }
}