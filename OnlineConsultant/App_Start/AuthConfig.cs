using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin;

namespace OnlineConsultant
{
    public static class AuthCookieConfig
    {
        public static void ConfigureAuth(this IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login")
            });

            
        }
    }
}