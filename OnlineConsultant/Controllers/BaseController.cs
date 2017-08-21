using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OnlineConsultant.Controllers
{
    public class BaseController : Controller
    {
        protected void SingInCookie(string username, int userId, bool isSpec)
        {
            var identity = new ClaimsIdentity(new[]
                                               {
                                                    new Claim(ClaimTypes.Name, username),
                                                    new Claim(ClaimTypes.Role, isSpec.ToString()),
                                                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                                                }, "ApplicationCookie");


            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identity);
        }

       
        protected void SignOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
        }
    }
}