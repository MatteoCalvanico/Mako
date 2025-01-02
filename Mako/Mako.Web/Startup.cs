//using Mako.Web.Hubs;
using Mako.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.IO;
using System.Linq;
using Mako.Services;
using System.Threading.Tasks;

namespace Mako.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Env = env;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<MakoDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "MakoDB");
            });

            // SERVICES FOR AUTHENTICATION
            services.AddSession();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login/Login";
                options.LogoutPath = "/Login/Logout";
            });

            var builder = services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {                        // Enable loading SharedResource for ModelLocalizer
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

#if DEBUG
            builder.AddRazorRuntimeCompilation();
#endif

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/{2}/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");

                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
                options.ViewLocationFormats.Add("/Features/Views/{1}/{0}.cshtml");
                options.ViewLocationFormats.Add("/Features/Views/Shared/{0}.cshtml");
                options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            // SIGNALR FOR COLLABORATIVE PAGES
            services.AddSignalR();

            // CONTAINER FOR ALL EXTRA CUSTOM SERVICES
            Container.RegisterTypes(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                // Https redirection only in production
                app.UseHsts();
                app.UseHttpsRedirection();
            } else
            {
                app.UseDeveloperExceptionPage();
            }

            // Localization support if you want to
            app.UseRequestLocalization(SupportedCultures.CultureNames);

            app.UseRouting();

            // Adding authentication to pipeline
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            var node_modules = new CompositePhysicalFileProvider(Directory.GetCurrentDirectory(), "node_modules");
            var areas = new CompositePhysicalFileProvider(Directory.GetCurrentDirectory(), "Areas");
            var compositeFp = new CustomCompositeFileProvider(env.WebRootFileProvider, node_modules, areas);
            env.WebRootFileProvider = compositeFp;
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                // ROUTING PER HUB

                endpoints.MapAreaControllerRoute("Worker", "Worker", "Worker/{controller=Shift}/{action=Index}");
                endpoints.MapAreaControllerRoute("Example", "Example", "Example/{controller=Users}/{action=Index}/{id?}");

                // Order is important
                endpoints.MapControllerRoute("default", "{controller=Login}/{action=Login}");
                endpoints.MapControllerRoute("Home", "{controller=Home}/{action=Index}");
            });
        }
    }

    public static class SupportedCultures
    {
        public readonly static string[] CultureNames;
        public readonly static CultureInfo[] Cultures;

        static SupportedCultures()
        {
            CultureNames = new[] { "it-it" };
            Cultures = CultureNames.Select(c => new CultureInfo(c)).ToArray();

            //NB: attenzione nel progetto a settare correttamente <NeutralLanguage>it-IT</NeutralLanguage>
        }
    }
}
