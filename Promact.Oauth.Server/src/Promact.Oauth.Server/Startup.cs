using System;
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
using Promact.Oauth.Server.Repository.OAuthRepository;
using Promact.Oauth.Server.Repository.HttpClientRepository;
using System.Net.Http;
using Promact.Oauth.Server.AutoMapper;
using AutoMapper;
using Exceptionless;
using NLog.Extensions.Logging;
using Promact.Oauth.Server.Constants;

namespace Promact.Oauth.Server
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IHostingEnvironment _currentEnvironment;
        public IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _currentEnvironment = env;
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

            _loggerFactory = loggerFactory;

        }




        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure AppSettings using config by installing Microsoft.Extensions.Options.ConfigurationExtensions
            //services.Configure<AppSettings>(Configuration);

            //Configure EmailCrednetials using config by installing Microsoft.Extensions.Options.ConfigurationExtensions
            services.Configure<EmailCrednetials>(Configuration.GetSection("EmailCrednetials"));

            //Configure SendGridAPI
            services.Configure<SendGridAPI>(Configuration.GetSection("SendGridAPI"));

            //Configure AppSettingUtil
            services.Configure<AppSettingUtil>(Configuration.GetSection("AppSettingUtil"));


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
            services.AddScoped<IStringConstant, StringConstant>();
            services.AddScoped<HttpClient>();

            services.AddScoped<IHttpClientRepository, HttpClientRepository>();


            services.AddMvc();
            services.AddScoped<CustomAttribute>();


            // Add application services.
            if (_currentEnvironment.IsDevelopment())
                services.AddTransient<IEmailSender, AuthMessageSender>();
            else if (_currentEnvironment.IsProduction())
                services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddOptions();

            //Register Mapper
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });
            services.AddSingleton<IMapper>(sp => mapperConfiguration.CreateMapper());

            services.AddMvc().AddMvcOptions(x => x.Filters.Add(new GlobalExceptionFilter(_loggerFactory)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IEnsureSeedData seeder, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();
            //needed for non-NETSTANDARD platforms: configure nlog.config in your project root
            env.ConfigureNLog("nlog.config");

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

            //If staging or production then only use exceptionless
            if (env.IsProduction())
            {
                app.UseExceptionless(Configuration["ExceptionLess:ExceptionLessApiKey"]);
            }

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                      name: "Login",
                      template: "Login",
                      defaults: new { controller = "Account", action = "Login" });

                routes.MapRoute(
                    name: "LogOff",
                    template: "LogOff",
                    defaults: new { controller = "Account", action = "LogOff" });

                //routes.MapRoute(
                //        name: "default",
                //         template: "{*.}",
                //     defaults: new { controller = "Home", action = "Index" }
                //     );

                routes.MapRoute(
                    name: "default",
                    //template: "{controller=Account}/{action=Login}");
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
