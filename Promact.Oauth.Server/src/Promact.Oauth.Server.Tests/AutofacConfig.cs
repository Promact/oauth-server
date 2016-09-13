//using Autofac;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using Promact.Oauth.Server.Data;
//using Promact.Oauth.Server.Data_Repository;
//using Promact.Oauth.Server.Models;
//using Promact.Oauth.Server.Repository;
//using Promact.Oauth.Server.Repository.ProjectsRepository;
//using Promact.Oauth.Server.Services;

//namespace Promact.Oauth.Server.Tests
//{
//    public class AutofacConfig
//    {
//        public static IComponentContext RegisterDependancies()
//        {
//            var builder = new ContainerBuilder();
//            var dataContext = new PromactOauthDbContext();
//            builder.RegisterInstance(dataContext).As<DbContext>().SingleInstance();
//            builder.RegisterType<UserManager<ApplicationUser>>().AsSelf();
//            builder.RegisterType<RoleManager<IdentityRole>>().AsSelf();
//            //builder.Register<AuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication);
//            builder.RegisterGeneric(typeof(DataRepository<>)).As(typeof(IDataRepository<>));
//            builder.RegisterType<UserRepository>().As<IUserRepository>();
//            builder.RegisterType<AuthMessageSender>().As<IEmailSender>();
//            builder.RegisterType<ProjectRepository>().As<IProjectRepository>();
//            var container = builder.Build();
//            return container;
//        }
//    }
//}
