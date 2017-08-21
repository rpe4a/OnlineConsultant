using System;
using System.Collections.Generic;
using System.Data.Entity;
using OnlineConsultant.Services;
using System.Data.Entity.Migrations;

namespace OnlineConsultant.Data
{
    using Entitys;
    using System.Linq;

    public class Configuration : DbMigrationsConfiguration<DbConsultantContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DbConsultantContext context)
        {
            if(!context.User.Any())
            {
                var service = new UsersService(new Repository<User>(context));

                service.Create(new User
                {
                    Name = "Специалист",
                    Login = "spec",
                    Password = PasswordHash.Encoding("12345")
                });

                service.Create(new User
                {
                    Name = "Специалист1",
                    Login = "spec1",
                    Password = PasswordHash.Encoding("12345")
                });
            }
           
        }
    }
}