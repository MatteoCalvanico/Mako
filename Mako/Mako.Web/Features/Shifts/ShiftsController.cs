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
                var shiftsViewModel = await GetAllShifts();
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

        public async Task<ShiftsViewModel> GetAllShifts()
        {
            var viewModel = new ShiftsViewModel();

            try
            {
                // Create a new ShiftsSelectQuery without any filters to retrieve all shifts
                var shiftsQuery = new ShiftsSelectQuery();
                var shifts = await _sharedService.SelectShiftsQuery(shiftsQuery);

                // Map the retrieved shifts to the view model
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
            }
            catch (Exception ex)
            {
                // Handle the exception as needed, e.g., log it
                throw new Exception("An error occurred while retrieving shifts", ex);
            }

            return viewModel;
        }
    }
}