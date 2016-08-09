using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Services;
using Promact.Oauth.Server.Seed;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Newtonsoft.Json.Serialization;
using Promact.Oauth.Server.Repository.OAuthRepository;

namespace Promact.Oauth.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var builder = new ContainerBuilder();
            //builder.RegisterType<AuthMessageSender>().As<IEmailSender>().InstancePerDependency();
            //builder.RegisterType<AuthMessageSender>().As<ISmsSender>().InstancePerDependency();

            // Add framework services.
            services.AddDbContext<PromactOauthDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<PromactOauthDbContext>()
                .AddDefaultTokenProviders();

            //Register application services
            services.AddScoped<IEnsureSeedData, EnsureSeedData>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IConsumerAppRepository, ConsumerAppRepository>();
            services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));
            services.AddScoped<IOAuthRepository, OAuthRepository>();

            services.AddMvc();
            //.AddJsonOptions(opt =>
            //{
            //    var resolver = opt.SerializerSettings.ContractResolver;
            //    if (resolver != null)
            //    {
            //        var res = resolver as DefaultContractResolver;
            //        res.NamingStrategy = null;  // <<!-- this removes the camelcasing
            //    }
            //});

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IEnsureSeedData seeder, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Call the Seed method in (Seed.EnsureSeedData) to create initial Admin
            seeder.Seed(serviceProvider);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    //template: "{controller=Account}/{action=Login}");
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
