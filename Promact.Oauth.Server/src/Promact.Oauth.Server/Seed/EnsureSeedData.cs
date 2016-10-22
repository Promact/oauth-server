using Promact.Oauth.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Promact.Oauth.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

            //Add Roles
            if (!roleManager.Roles.Any())
            {
                List<IdentityRole> roles = new List<IdentityRole>();
                roles.Add(new IdentityRole { Name = "Employee", NormalizedName = "EMPLOYEE" });
                roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                roles.Add(new IdentityRole { Name = "Management", NormalizedName = "Management" });

                foreach (var role in roles)
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

            var adminUser = userManager.FindByEmailAsync("roshni@promactinfo.com").Result;
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser()
                {
                    UserName = "roshni@promactinfo.com",
                    Email = "roshni@promactinfo.com",
                    FirstName = "Admin",
                    LastName = "Promact",
                    IsActive = true,
                    CreatedDateTime = DateTime.UtcNow,
                    SlackUserName="roshni"
                };
                userManager.CreateAsync(newAdmin, "Admin@123").Wait();

                //Assign role to Admin
                //var role = roleManager.FindByNameAsync("Admin").Result;
                userManager.AddToRoleAsync(newAdmin, "Admin").Wait();
            }
        }
    }
}
