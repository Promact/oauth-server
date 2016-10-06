using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models;

namespace Promact.Oauth.Server.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                
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
