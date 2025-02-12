using Mako.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Mako.Infrastructure;
using Mako.Services.Shared;
using static Mako.Services.Shared.WorkersSelectDTO;

namespace Mako.Web.Features.Login
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Alerts]
    [ModelStateToTempData]
    public partial class LoginController : Controller
    {
        public static string LoginErrorModelStateKey = "LoginError";
        private readonly SharedService _sharedService;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public LoginController(SharedService sharedService, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _sharedService = sharedService;
            _sharedLocalizer = sharedLocalizer;
        }

        private ActionResult LoginAndRedirect(UserDetailDTO utente, string returnUrl, bool rememberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utente.Id.ToString()),
                new Claim(ClaimTypes.Email, utente.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
            {
                ExpiresUtc = (rememberMe) ? DateTimeOffset.UtcNow.AddMonths(3) : null,
                IsPersistent = rememberMe,
            });

            if (string.IsNullOrWhiteSpace(returnUrl) == false)
                return Redirect(returnUrl);

            //return RedirectToAction(MVC.Example.Users.Index());
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public virtual IActionResult Login(string returnUrl)
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrWhiteSpace(returnUrl) == false)
                    return Redirect(returnUrl);

                //return RedirectToAction(MVC.Example.Users.Index());
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };

            return View(model);
        }

        [HttpPost]
        public async virtual Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var utente = await _sharedService.Query(new CheckLoginCredentialsQuery
                    {
                        Email = model.Email,
                        Password = model.Password,
                    });

                    var isAdmin = await _sharedService.IsShiftAdminAsync(utente.Cf);
                    if (isAdmin)
                    {
                        return LoginAndRedirect(utente, model.ReturnUrl, model.RememberMe); // Go to Shift admin home
                    }
                    else
                    {
                        var workerReturnUrl = Url.Action("Index", "Shift", new { area = "Worker" });
                        return LoginAndRedirect(utente, workerReturnUrl, model.RememberMe); // Go to Worker home
                    }
                }
                catch (LoginException e)
                {
                    ModelState.AddModelError(LoginErrorModelStateKey, e.Message);
                }
            }

            return RedirectToAction(MVC.Login.Login());
        }

        [HttpPost]
        public virtual IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            Alerts.AddSuccess(this, "Utente scollegato correttamente");
            return RedirectToAction(MVC.Login.Login());
        }
    }
}