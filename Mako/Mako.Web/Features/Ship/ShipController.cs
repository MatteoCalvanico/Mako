using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mako.Services.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mako.Web.Areas;

namespace Mako.Web.Features.ShipName
{
    public partial class ShipController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public ShipController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        public virtual async Task<IActionResult> Index()
        {
            // // Recupera le navi dal database tramite il metodo GetShipsAsync()
            var ships = await _sharedService.GetShipsAsync();

            // Ritorna la view "Default.cshtml" con la lista di Ship
            return View("Default", ships);
        }
    }
}
