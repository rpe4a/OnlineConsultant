using OnlineConsultant.Models;
using System.Linq;
using System.Web.Mvc;

namespace OnlineConsultant.Controllers
{
    using Services;
    using Data.Entitys;
    using System;

    [RoutePrefix("Home")]
    public class HomeController : BaseController
    {
        private readonly IUsersService _userSrv;

        private readonly IQuestionService _questionSrv;

        public HomeController(IUsersService userSrv,
                              IQuestionService questionSrv)
        {
            _userSrv = userSrv;
            _questionSrv = questionSrv;
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View(new RegistrationModel());
        }

        [HttpPost]
        public ActionResult Index(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var userId = _userSrv.Create(new User
            {
                Surname = model.Surname,
                Name = model.Name,
                Firstname = model.FirstName,
                Email = model.Email
            });
            _questionSrv.SetConsulant(userId,model.Question);
            SingInCookie(string.Format("{0} {1}", model.Surname, model.Name), userId, false);

            return RedirectToAction("Chat");
        }

        [Authorize]
        public PartialViewResult TableUserQuestions(int state = 1)
        {
            var list = _questionSrv.GetUnansweredUser()
                                    .Select(x => new QuestionUserModel
                                    {
                                        UserId = x.Id,
                                        FullName = string.Format("{0} {1} {2}", x.Surname, x.Name, x.Firstname),
                                        Email = x.Email,
                                        Description = x.Questions.FirstOrDefault().Description,
                                        IsOpen = x.Questions.FirstOrDefault().State == QuestionOfState.Open,
                                        CreateTime = x.Questions.FirstOrDefault().CreateTime.ToString("dd.MM.yyyy hh:mm")

                                    });
            switch (state)
            {
                case 2:
                    list = list.Where(x => x.IsOpen);
                    break;
                case 3:
                    list = list.Where(x => !x.IsOpen);
                    break;
            }

            return PartialView("TableUserQuestions", list.ToArray());
        }


        [HttpGet]
        [Authorize]
        public ActionResult Question(int state = 1)
        {
            if (!User.IsSpec())
                return RedirectToAction("Login", "Account");

            return View();
        }


        [HttpPost]
        [Authorize]
        public ActionResult QuestionClose(int fromUser)
        {
            _questionSrv.CloseQuestion(fromUser);

            return TableUserQuestions();
        }

        [HttpPost]
        [Authorize]
        public ActionResult FilterStateOfQuestion(int state)
        {
            return TableUserQuestions(state);
        }


        [HttpGet]
        [Authorize]
        public ActionResult Chat()
        {
            ViewBag.IsSpec = User.IsSpec();

            return View();
        }
    }
}