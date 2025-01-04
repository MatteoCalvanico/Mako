using Mako.Services.Shared;
using Mako.Web.Areas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mako.Web.Features.Shifts
{
    public partial class ShiftsController : Controller
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

            // Create and populate the model
            var shiftsViewModel = new ShiftsViewModel
            {
                Shifts = new List<ShiftViewModel>
                {
                    new ShiftViewModel
                    {
                        Id = Guid.NewGuid(),
                        Pier = 1,
                        Date = DateTime.Today,
                        StartHour = TimeSpan.FromHours(8),
                        EndHour = TimeSpan.FromHours(16),
                        ShipName = "Ship A",
                        ShipDateArrival = DateTime.Today.AddDays(1)
                    }
                }
            };

            return View("Shifts", shiftsViewModel);
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

        [HttpPost]
        public async virtual Task<IActionResult> GetShifts(ShiftsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Shifts", viewModel);
            }

            try
            {
                var shifts = await _sharedService.Query(new ShiftsSelectQuery
                {
                    IdCurrentShift = viewModel.Shifts.FirstOrDefault()?.Id ?? Guid.Empty,
                });

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

                return View("Shifts", viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while retrieving shifts.");
                return View("Shifts", viewModel);
            }
        }
    }
}
