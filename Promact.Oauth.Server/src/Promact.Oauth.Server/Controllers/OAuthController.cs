using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Repository.OAuthRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    public class OAuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConsumerAppReposiotry _appRepository;
        private readonly IOAuthRepository _oAuthRepository;
        HttpClient client;

        public OAuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConsumerAppReposiotry appRepository, IOAuthRepository oAuthRepository)
        {
            client = new HttpClient();
            _signInManager = signInManager;
            _userManager = userManager;
            _appRepository = appRepository;
            _oAuthRepository = oAuthRepository;
        }

        // POST: /OAuth/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(OAuthLogin model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    OAuth oAuth = new OAuth();
                    oAuth = _oAuthRepository.GetDetails(model.Email);
                    if (oAuth.AccessToken == null)
                    {
                        oAuth.RefreshToken = Guid.NewGuid().ToString();
                        oAuth.userEmail = model.Email;
                        oAuth.AccessToken = Guid.NewGuid().ToString();
                        _oAuthRepository.Add(oAuth);
                    }
                    var viewBag = ViewData["ReturnUrl"];
                    //client.BaseAddress = new Uri(ViewData["ReturnUrl"].ToString());
                    client.BaseAddress = new Uri("http://localhost:28182/oAuth/RefreshToken");
                    var response = client.GetAsync("?refreshToken=" + oAuth.RefreshToken).Result;
                    var responseResult = response.Content.ReadAsStringAsync().Result;
                    var content = JsonConvert.DeserializeObject<OAuthApplication>(responseResult);
                    var app = _appRepository.GetAppDetails(content.ClientId);
                    if (app.AuthSecret == content.ClientSecret && content.RefreshToken == oAuth.RefreshToken)
                    {
                        return Redirect(content.ReturnUrl + "?accessToken=" + oAuth.AccessToken+"&email="+oAuth.userEmail);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Call will hit here at everytime for external Login with clientId and it will check for 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public IActionResult ExternalLogin(string clientId)
        {
            var result = _appRepository.GetAppDetails(clientId);
            if (result != null)
            {
                ViewData["ReturnUrl"] = result.CallbackUrl;
                return View();
            }
            return BadRequest();
        }
    }
}
