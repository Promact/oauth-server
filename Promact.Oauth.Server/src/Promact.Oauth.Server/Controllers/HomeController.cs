using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Constants;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringConstant _stringConstant;
        private readonly AppConstant _appConstant;
        public HomeController(UserManager<ApplicationUser> userManager, IStringConstant stringConstant)
        {
            _userManager = userManager;
            _stringConstant = stringConstant;
            _appConstant = _stringConstant.JsonDeserializeObject();
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                UserRoleAc userRole = new UserRoleAc();
                string roleAdmin;
                _appConstant.CommanStringConstant.TryGetValue("RoleAdmin", out roleAdmin);
                if (User.IsInRole(roleAdmin))
                {
                    userRole.Role = roleAdmin;
                    userRole.UserId = user.Id;
                    ViewData["UserRole"] = userRole;
                }
                else
                {
                    string roleEmployee;
                    _appConstant.CommanStringConstant.TryGetValue("RoleAdmin", out roleEmployee);
                    userRole.Role = roleEmployee;
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
