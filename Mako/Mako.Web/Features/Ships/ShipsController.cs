using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mako.Services.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mako.Web.Areas;
using Mako.Web.Features.Ship;

namespace Mako.Web.Features.ShipName
{
    public partial class ShipsController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public ShipsController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        public virtual async Task<IActionResult> Index(string searchTerm = null)
        {
            var shipsViewModel = await GetAllShips();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Example: filtering by Ship Name or Pier
                shipsViewModel.Ships = shipsViewModel.Ships
                    .Where(s => s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                             || s.Pier.ToString().Contains(searchTerm))
                    .ToList();
            }

            return View("Index", shipsViewModel);
        }

        public async Task<ShipsViewModel> GetAllShips()
        {
            var viewModel = new ShipsViewModel();
            try
            {
                var shipsQuery = new ShipsSelectQuery();
                var ships = await _sharedService.SelectShipsQuery(shipsQuery);
                viewModel.Ships = ships.Ships.Select(s => new ShipViewModel
                {
                    Name = s.Name,
                    DateArrival = s.DateArrival,
                    DateDeparture = s.DateDeparture,
                    Pier = s.Pier,
                    TimeEstimation = s.TimeEstimation,
                    CargoManifest = s.CargoManifest
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting all ships", ex);
            }
            return viewModel;
        }
    }
}
