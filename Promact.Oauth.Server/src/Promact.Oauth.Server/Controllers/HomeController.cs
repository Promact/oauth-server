using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Constants;

namespace Promact.Oauth.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringConstant _stringConstant;
        public HomeController(UserManager<ApplicationUser> userManager, IStringConstant stringConstant)
        {
            _userManager = userManager;
            _stringConstant = stringConstant;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                UserRoleAc userRole = new UserRoleAc();
                if (User.IsInRole(_stringConstant.RoleAdmin))
                {
                    userRole.Role = _stringConstant.RoleAdmin;
                    userRole.UserId = user.Id;
                    ViewData["UserRole"] = userRole;
                }
                else
                {
                    userRole.Role = _stringConstant.RoleEmployee;
                    userRole.UserId = user.Id;
                    ViewData["UserRole"] = userRole;
                    

                }
                    return View("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact page";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
