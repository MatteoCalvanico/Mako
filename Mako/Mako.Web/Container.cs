using Microsoft.Extensions.DependencyInjection;
using Mako.Services.Shared;

namespace Mako.Web
{
    public class Container
    {
        public static void RegisterTypes(IServiceCollection container)
        {
            // Registration of all the database services you have
            container.AddScoped<SharedService>();
        }
    }
}
