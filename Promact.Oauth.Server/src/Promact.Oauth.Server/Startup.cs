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
using System.Net.Http;
using Promact.Oauth.Server.AutoMapper;
using AutoMapper;
using Exceptionless;
using NLog.Extensions.Logging;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Utility;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using IdentityServer4;
using Promact.Oauth.Server.Configuration.DefaultAPIResource;
using Promact.Oauth.Server.Configuration.DefaultIdentityResource;

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

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            // Add framework services.
            services.AddDbContext<PromactOauthDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<PromactOauthDbContext>()
                .AddDefaultTokenProviders();

            //Register application services
            services.AddScoped<IEnsureSeedData, EnsureSeedData>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IConsumerAppRepository, ConsumerAppRepository>();
            services.AddScoped(typeof(IDataRepository<,>), typeof(DataRepository<,>));
            //services.AddScoped<IOAuthRepository, OAuthRepository>();
            services.AddScoped<IStringConstant, StringConstant>();
            services.AddScoped<HttpClient>();

            services.AddScoped<IHttpClientService, HttpClientService>();
            services.AddScoped<IEmailUtil, EmailUtil>();
            services.AddScoped<IDefaultApiResources, DefaultApiResources>();
            services.AddScoped<IDefaultIdentityResources, DefaultIdentityResources>();
            services.AddScoped<ICustomConsentService, CustomConsentService>();
            services.AddScoped<SecurityHeadersAttribute>();

            services.AddMvc();

            // Add application services.
            if (_currentEnvironment.IsDevelopment())
                services.AddTransient<IEmailSender, AuthMessageSender>();
            else if (_currentEnvironment.IsProduction())
                services.AddTransient<IEmailSender, SendGridEmailSender>();
            

            services.AddOptions();

            //Register Mapper
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });
            services.AddSingleton<IMapper>(sp => mapperConfiguration.CreateMapper());

            services.AddMvc().AddMvcOptions(x => x.Filters.Add(new GlobalExceptionFilter(_loggerFactory)));

            // X509Certificate2 for creating access token of external login user
            var cert = new X509Certificate2(Path.Combine(_currentEnvironment.ContentRootPath, "idsvr3test.pfx"), "idsrv3test");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // IdentityServer4 Details
            services.AddIdentityServer()
                .AddSigningCredential(cert)
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(builder =>
                builder.UseSqlServer(connectionString, options =>
                options.MigrationsAssembly(migrationsAssembly)))
                .AddOperationalStore(builder =>
                builder.UseSqlServer(connectionString, options =>
                options.MigrationsAssembly(migrationsAssembly)))
                .AddProfileService<CustomProfileService>();

            // Custom Policy Claim based
            services.AddAuthorization(option => option.AddPolicy("ReadUser",
                policy => policy.RequireClaim("scope", "user.read")));
            services.AddAuthorization(option =>option.AddPolicy("ReadProject",
                policy => policy.RequireClaim("scope", "project.read")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IEnsureSeedData seeder, IServiceProvider serviceProvider)
        {
            // Resolve dependency
            var appSetting = serviceProvider.GetService<IOptions<AppSettingUtil>>().Value;
            var defaultApiResource = serviceProvider.GetService<IDefaultApiResources>();
            var defaultIdentityResource = serviceProvider.GetService<IDefaultIdentityResources>();

            // Initializing default APIResource and IdentityResources
            IdentityServerInitialize databaseInitialize = new IdentityServerInitialize();
            databaseInitialize.InitializeDatabaseForPreDefinedAPIResourceAndIdentityResources(app,defaultApiResource,defaultIdentityResource);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();
            //needed for non-NETSTANDARD platforms: configure nlog.config in your project root
            loggerFactory.ConfigureNLog("nlog.config");            

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
            app.UseIdentityServer();

            // Allowing authentication for API resource of read-only with limit scope
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = appSetting.PromactOAuthUrl ?? "https://oauth.promactinfo.com",
                RequireHttpsMetadata = false,
                ApiName = "read-only",
                AllowedScopes = new List<string>()
                        {
                            IdentityServerConstants.StandardScopes.Email,
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "slack_user_id",
                            "user.read",
                            "project.read"
                        },
                ApiSecret = appSetting.AuthenticationAPISecret,
            });

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

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
