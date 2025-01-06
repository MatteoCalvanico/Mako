using Mako.Services.Shared;
using Mako.Web.Areas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mako.Web.Features.Shifts
{
    public partial class ShiftsController : AuthenticatedBaseController
    {
        private readonly SharedService _sharedService;

        public ShiftsController(SharedService sharedService)
        {
            _sharedService = sharedService;
        }

        public virtual async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var userDetail = await _sharedService.Query(new UserDetailQuery { Id = Guid.Parse(userId) });
                ViewData[Mako.Web.Areas.IdentitaViewModel.VIEWDATA_IDENTITACORRENTE_KEY] = new IdentitaViewModel
                {
                    EmailUtenteCorrente = userDetail.Email,
                };
            }

            try
            {
                var shiftsViewModel = await GetAllShiftsAndShips();
                return View("Shifts", shiftsViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving shifts: " + ex.Message);
                return View("Shifts", new ShiftsViewModel());
            }
        }

        [HttpPost]
        public virtual IActionResult ChangeLanguageTo(string cultureName)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureName)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), Secure = true }
            );

            return Redirect(Request.GetTypedHeaders().Referer?.ToString() ?? "/");
        }

        public async Task<ShiftsViewModel> GetAllShiftsAndShips()
        {
            var viewModel = new ShiftsViewModel();

            try
            {
                // Retrieve shifts
                var shiftsQuery = new ShiftsSelectQuery();
                var shifts = await _sharedService.SelectShiftsQuery(shiftsQuery);
                viewModel.Shifts = shifts.Shifts.Select(s => new ShiftViewModel
                {
                    Id = s.Id,
                    Pier = s.Pier,
                    Date = s.Date,
                    StartHour = s.StartHour,
                    EndHour = s.EndHour,
                    ShipName = s.ShipName,
                    ShipDateArrival = s.ShipDateArrival
                }).ToList();

                // Retrieve ships
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
                // Handle the exception as needed, e.g., log it
                throw new Exception("An error occurred while retrieving shifts and ships", ex);
            }

            return viewModel;
        }
    }
}