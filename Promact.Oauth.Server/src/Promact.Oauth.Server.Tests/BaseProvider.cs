﻿using AutoMapper;
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
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using Promact.Oauth.Server.Constants;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Tests
{
    public class BaseProvider
    {
        public IServiceProvider serviceProvider { get; set; }

        private MapperConfiguration _mapperConfiguration { get; set; }

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
            services.AddScoped<IHttpClientRepository, HttpClientRepository>();
            services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));

            //Register Mapper
            services.AddSingleton<IMapper>(sp => _mapperConfiguration.CreateMapper());
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddDbContext<PromactOauthDbContext>(options => options.UseInMemoryDatabase(randomString), ServiceLifetime.Transient);

            serviceProvider = services.BuildServiceProvider();
            RoleSeedFake(serviceProvider).Wait();
        }
        public async Task RoleSeedFake(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (!_roleManager.Roles.Any())
            {
                List<IdentityRole> roles = new List<IdentityRole>();
                roles.Add(new IdentityRole { Name = StringConstant.Employee, NormalizedName = StringConstant.NormalizedName });
                roles.Add(new IdentityRole { Name = StringConstant.Admin, NormalizedName = StringConstant.NormalizedSecond });

                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
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
