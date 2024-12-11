using Mako.Web.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Template.Services.Shared;

namespace Mako.Web
{
    public class Container
    {
        public static void RegisterTypes(IServiceCollection container)
        {
            // Registration of all the database services you have
            container.AddScoped<SharedService>();

            // Registration of SignalR events
            container.AddScoped<IPublishDomainEvents, SignalrPublishDomainEvents>();
        }
    }
}
