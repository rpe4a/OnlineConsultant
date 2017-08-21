using System.Collections.Generic;
using System.Linq;

namespace OnlineConsultant.Services
{
    using System;
    using Data;
    using Data.Entitys;

    public interface IQuestionService
    {
        void SetConsulant(int fromUser, string question);

        Question FindQuestion(int fromUser);

        void CloseQuestion(int fromUser);

        IEnumerable<User> GetUnansweredUser();
    }

    public class QuestionService : IQuestionService
    {
        private readonly IRepository<User> _userRep;

        private readonly IRepository<Question> _questionRep;

        public QuestionService(IRepository<User> userRep,
                               IRepository<Question> questionRep)
        {
            _userRep = userRep;
            _questionRep = questionRep;
        }

        public void SetConsulant(int fromUser, string question)
        {
            var user = _userRep.All.SingleOrDefault(x => x.Id == fromUser);
            user.Questions.Add(new Question
            {
                Description = question,
                CreateTime = DateTime.Now,
                State = QuestionOfState.Open
            });
            _userRep.Update(user);
            _userRep.Commit();
        }

        public IEnumerable<User> GetUnansweredUser()
        {
            var result = (from user in _userRep.All
                         join question in _questionRep.All on user.Id equals question.User.Id into quest
                         where quest.Any()
                         select new
                         {
                             user.Id,
                             user.Surname,
                             user.Name,
                             user.Firstname,
                             user.Email,
                             Questions = quest
                         })
                        .ToList();

            return result.Select(x => new User
            {
                Id = x.Id,
                Surname = x.Surname,
                Name = x.Name,
                Firstname = x.Firstname,
                Email = x.Email,
                Questions = x.Questions.ToList()
            });
        }

        public void CloseQuestion(int fromUser)
        {
            var questio = _questionRep.All.SingleOrDefault(x => x.User.Id == fromUser);
            if(questio != null)
            {
                questio.State = QuestionOfState.Close;
                _questionRep.Update(questio);
                _questionRep.Commit();
            }

        }

        public Question FindQuestion(int fromUser)
        {
            var result = _questionRep.All.SingleOrDefault(x => x.User.Id == fromUser);

            return result;
        }
    }
}