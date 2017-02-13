using Promact.Oauth.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Promact.Oauth.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

//Seeds tha database with the initial user Admin
namespace Promact.Oauth.Server.Seed
{
    public class EnsureSeedData : IEnsureSeedData
    {
        private readonly PromactOauthDbContext context;
        public EnsureSeedData(PromactOauthDbContext _context)
        {
            context = _context;
            context.Database.EnsureCreated();
        }

        public void Seed(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var seedData = serviceProvider.GetService<IOptions<SeedData>>();
           
            //Add Roles
            if (!roleManager.Roles.Any())
            {
                foreach (var role in seedData.Value.Roles)
                {
                    var roleExit = roleManager.RoleExistsAsync(role.Name).Result;
                    if (!roleExit)
                    {
                        context.Roles.Add(role);
                        context.SaveChanges();
                    }
                }
            }


            //Add Admin
            var adminUser = userManager.FindByEmailAsync(seedData.Value.Email).Result;
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser()
                {
                    UserName = seedData.Value.UserName,
                    Email = seedData.Value.Email,
                    FirstName = seedData.Value.FirstName,
                    LastName = seedData.Value.LastName,
                    IsActive = seedData.Value.IsActive,
                    CreatedDateTime = DateTime.UtcNow
                };
                userManager.CreateAsync(newAdmin, seedData.Value.Password).Wait();

                //Assign role to Admin
                //var role = roleManager.FindByNameAsync("Admin").Result;
                userManager.AddToRoleAsync(newAdmin, seedData.Value.Role).Wait();
            }
        }
    }
}
