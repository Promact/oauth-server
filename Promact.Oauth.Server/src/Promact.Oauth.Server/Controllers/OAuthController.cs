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
                    //checking whether with this app email is register or not if  not new OAuth will be created.
                    var oAuth = _oAuthRepository.GetDetails(model.Email, model.ClientId);
                    if (oAuth == null)
                    {
                        oAuth = new OAuth();
                        oAuth.RefreshToken = Guid.NewGuid().ToString();
                        oAuth.userEmail = model.Email;
                        oAuth.AccessToken = Guid.NewGuid().ToString();
                        oAuth.ClientId = model.ClientId;
                        _oAuthRepository.Add(oAuth);
                    }
                    // Assigning Base Address with redirectUrl
                    client.BaseAddress = new Uri(model.RedirectUrl);
                    var requestUrl = string.Format("?refreshToken={0}", oAuth.RefreshToken);
                    var response = client.GetAsync(requestUrl).Result;
                    var responseResult = response.Content.ReadAsStringAsync().Result;
                    // Transforming Json String to object type OAuthApplication
                    var content = JsonConvert.DeserializeObject<OAuthApplication>(responseResult);
                    // Checking whether request client is equal to response client
                    if (model.ClientId == content.ClientId)
                    {
                        //Getting app details from clientId or AuthId
                        var app = _appRepository.GetAppDetails(content.ClientId);
                        
                        // Refresh token and app's secret is checking if match then accesstoken will be send
                        if (app.AuthSecret == content.ClientSecret && content.RefreshToken == oAuth.RefreshToken)
                        {
                            var returnUrl = string.Format("{0}?accessToken={1}&email={2}", content.ReturnUrl, oAuth.AccessToken, oAuth.userEmail);
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
        public IActionResult ExternalLogin(string clientId)
        {
            var result = _appRepository.GetAppDetails(clientId);
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
