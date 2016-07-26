using Promact.Oauth.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server;
using Promact.Oauth.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

        public async void Seed()
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            if(!context.Roles.Any(role => role.Name == "Admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Admin" });
                await roleStore.CreateAsync(new IdentityRole { Name = "Developer" });
                await roleStore.CreateAsync(new IdentityRole { Name = "Web Designer" });
                await roleStore.CreateAsync(new IdentityRole { Name = "Tester" });
            }


            var user = new ApplicationUser
            {
                UserName = "admin@promactinfo.com",
                Email = "admin@promactinfo.com",
                FirstName = "Admin",
                LastName = "Admin"
            };
            

            //if (!context.Users.Any(u => u.UserName == user.UserName))
            if (!context.ApplicationUsers.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashedPassword = password.HashPassword(user, "admin");
                //user.PasswordHash = hashedPassword;
                var userStore = new UserStore<ApplicationUser>(context);
                await userStore.CreateAsync(user);
                //await userStore.AddToRoleAsync(user, "Admin");
            }
            
            await context.SaveChangesAsync();
        }
    }
}
