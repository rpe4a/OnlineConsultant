using System.Collections.Generic;

namespace OnlineConsultant.Data.Entitys
{
    public class User
    {
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Firstname { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public User()
        {
            Questions = new List<Question>();
        }
    }
}