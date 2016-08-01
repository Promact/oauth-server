using Promact.Oauth.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server;
using Promact.Oauth.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

//Seeds tha database with the initial user Admin
namespace Promact.Oauth.Server.Seed
{
    public class EnsureSeedData : IEnsureSeedData
    {
        private PromactOauthDbContext context;
        public EnsureSeedData(PromactOauthDbContext _context)
        {
            context = _context;
        }

        public void Seed(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Add Roles
            if (!roleManager.Roles.Any())
            {
                List<IdentityRole> roles = new List<IdentityRole>();
                roles.Add(new IdentityRole { Name = "Developer", NormalizedName = "DEVELOPER" });
                roles.Add(new IdentityRole { Name = "Web Designer", NormalizedName = "WEB DESIGNER" });
                roles.Add(new IdentityRole { Name = "QA Tester", NormalizedName = "QA TESTER" });

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
            var adminUser = userManager.FindByEmailAsync("admin@promactinfo.com").Result;
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser()
                {
                    UserName = "admin@promactinfo.com",
                    Email = "admin@promactinfo.com",
                    FirstName = "Admin",
                    LastName = "Promact",
                    Status = true
                };
                userManager.CreateAsync(newAdmin, "Admin@123").Wait();

                //Assign role to Admin
                //var role = roleManager.FindByNameAsync("Admin").Result;
                userManager.AddToRoleAsync(newAdmin, "Admin").Wait();
            }
        }
    }
}
