using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.AutoMapper;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Repository.HttpClientRepository;
using Promact.Oauth.Server.Repository.OAuthRepository;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Seed;
using Promact.Oauth.Server.Services;
using System;
using System.Net.Http;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using System.Collections.Generic;
using Promact.Oauth.Server.Constants;

namespace Promact.Oauth.Server.Tests
{
    public class BaseProvider
    {
        public IServiceProvider serviceProvider { get; set; }

        private MapperConfiguration _mapperConfiguration { get; set; }

        //private readonly StringConstant _stringConstant;

        public BaseProvider()
        {

            var randomString = Guid.NewGuid().ToString();

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });

            var testHostingEnvironment = new MockHostingEnvironment();

            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase();

            services.AddSingleton<IHostingEnvironment>(testHostingEnvironment);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<PromactOauthDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IEnsureSeedData, EnsureSeedData>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IConsumerAppRepository, ConsumerAppRepository>();
            services.AddScoped<IOAuthRepository, OAuthRepository>();
            services.AddScoped<HttpClient>();
            services.AddScoped<IStringConstant,StringConstant>();
            services.AddScoped<IHttpClientRepository, HttpClientRepository>();
            services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));

            //Register Mapper
            services.AddSingleton<IMapper>(sp => _mapperConfiguration.CreateMapper());
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddDbContext<PromactOauthDbContext>(options => options.UseInMemoryDatabase(randomString), ServiceLifetime.Transient);

            serviceProvider = services.BuildServiceProvider();
            RoleSeedFake(serviceProvider);
        }
        public void RoleSeedFake(IServiceProvider serviceProvider)
        {
            var _db = serviceProvider.GetService<PromactOauthDbContext>();
            var _stringConstant = serviceProvider.GetService<IStringConstant>();
            if (!_db.Roles.Any())
            {
                List<IdentityRole> roles = new List<IdentityRole>();
                roles.Add(new IdentityRole { Name = _stringConstant.Employee, NormalizedName = _stringConstant.NormalizedName });
                roles.Add(new IdentityRole { Name = _stringConstant.Admin, NormalizedName = _stringConstant.NormalizedSecond });

                foreach (var role in roles)
                {
                    _db.Roles.Add(role);
                }
                _db.SaveChanges();
            }
        }
    }

    public class MockHostingEnvironment : IHostingEnvironment
    {
        public string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IFileProvider ContentRootFileProvider
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string ContentRootPath
        {
            get
            {
                return "test";
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string EnvironmentName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IFileProvider WebRootFileProvider
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string WebRootPath
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
