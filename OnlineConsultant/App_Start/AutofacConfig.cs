using System.Linq;
using System.Reflection;
using Autofac.Integration.Mvc;
using Autofac;
using System.Web.Mvc;
using Owin;

namespace OnlineConsultant
{
    using Data;
    using Microsoft.AspNet.SignalR;
    using Services;


    public static class AutofacConfig
    {
        public static void ConfigureAutofac(this IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
           

            builder.RegisterType<DbConsultantContext>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>))
                   .As(typeof(IRepository<>));

            builder.RegisterType<UsersService>().As<IUsersService>();
            builder.RegisterType<QuestionService>().As<IQuestionService>();

            var container = builder.Build();
           
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            GlobalHost.DependencyResolver.Register(typeof(ChatHub), () => new ChatHub(container.Resolve<IQuestionService>()));
        }
    }
}