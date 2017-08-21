using OnlineConsultant.Models;
using System.Web.Mvc;

namespace OnlineConsultant.Controllers
{
    using Services;
 
    public class AccountController : BaseController
    {
        private readonly IUsersService _userSvc;

        public AccountController(IUsersService userSvc)
        {
            _userSvc = userSvc;
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.IsError = false;
            return View();
        }

        [HttpPost]
        public ActionResult Login(AccountModel model)
        {
            var user = _userSvc.FindUser(model.Username, model.Password);
            if (user == null)
            {
                ViewBag.IsError = true;
                return View();
            }

            SingInCookie(model.Username, user.Id, true);
            var s = User.Identity.IsAuthenticated;

            return RedirectToAction("Chat", "Home");
        }

        [HttpGet]
        public ActionResult Signout()
        {
            SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}