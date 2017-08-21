using Microsoft.Owin;
using OnlineConsultant;
using Owin;

[assembly:OwinStartup(typeof(OnlineConsultant.Startup))]
namespace OnlineConsultant
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.ConfigureRoutes();
            app.ConfigureAuth();
            app.ConfigureAutofac();
           

            app.MapSignalR();
        }
    }
}