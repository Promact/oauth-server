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
        private readonly IConsumerAppRepository _appRepository;
        private readonly IOAuthRepository _oAuthRepository;
        HttpClient client;

        public OAuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConsumerAppRepository appRepository, IOAuthRepository oAuthRepository)
        {
            client = new HttpClient();
            _signInManager = signInManager;
            _userManager = userManager;
            _appRepository = appRepository;
            _oAuthRepository = oAuthRepository;
        }

        /// <summary>
        /// External Login
        /// </summary>
        /// <param name="model">It contain Email Password to Login to the server & contain redirectUrl and clientId used after LogIn</param>
        /// <returns></returns>
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
                    var oAuth = _oAuthRepository.OAuthClientChecking(model.Email, model.ClientId);
                    var clientResponse = _oAuthRepository.GetAppDetailsFromClient(model.RedirectUrl, oAuth.RefreshToken);
                    // Checking whether request client is equal to response client
                    if (model.ClientId == clientResponse.ClientId)
                    {
                        //Getting app details from clientId or AuthId
                        var app = _appRepository.GetAppDetails(clientResponse.ClientId);
                        // Refresh token and app's secret is checking if match then accesstoken will be send
                        if (app.AuthSecret == clientResponse.ClientSecret && clientResponse.RefreshToken == oAuth.RefreshToken)
                        {
                            var returnUrl = string.Format("{0}?accessToken={1}&email={2}", clientResponse.ReturnUrl, oAuth.AccessToken, oAuth.userEmail);
                            return Redirect(returnUrl);
                        }
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
        public async Task<IActionResult> ExternalLogin(string clientId)
        {
            var result = _appRepository.GetAppDetails(clientId);
            //await _signInManager.SignOutAsync();
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var oAuth = _oAuthRepository.OAuthClientChecking(user.Email,clientId);
                var clientResponse = _oAuthRepository.GetAppDetailsFromClient(result.CallbackUrl, oAuth.RefreshToken);
                var returnUrl = string.Format("{0}?accessToken={1}&email={2}",clientResponse.ReturnUrl, oAuth.AccessToken, oAuth.userEmail);
                return Redirect(returnUrl);
            }
            if (result != null)
            {
                // RedirectUrl is assign to ViewBag and on client side it will bind with OAuthLogin data
                ViewBag.returnUrl = result.CallbackUrl;
                ViewBag.clientId = result.AuthId;
                return View();
            }
            return BadRequest();
        }
    }
}
