using System.Linq;

namespace OnlineConsultant.Services
{
    using Data;
    using Data.Entitys;

    public interface IUsersService
    {
        int Create(User user);

        User FindUser(string login, string pass);
    }

    public class UsersService : IUsersService
    {
        private readonly IRepository<User> _repository;

        public UsersService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public int Create(User user)
        {
            _repository.Insert(user);
            _repository.Commit();

            return user.Id;
        }

        public User FindUser(string login, string pass)
        {
            var user = _repository.All.SingleOrDefault(x => x.Login == login);
            if (user == null) return null;

            return PasswordHash.IsEqual(pass, user.Password) ? user : null;
        }
    }
}