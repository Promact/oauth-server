using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Repository.OAuthRepository;
using System;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    public class OAuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConsumerAppRepository _appRepository;
        private readonly IOAuthRepository _oAuthRepository;

        public OAuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConsumerAppRepository appRepository, IOAuthRepository oAuthRepository)
        {
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
            try
            {
                if (ModelState.IsValid)
                {
                    var redirectUrl = await _oAuthRepository.UserNotAlreadyLogin(model);
                    if (redirectUrl != StringConstant.EmptyString)
                    {
                        return Redirect(redirectUrl);
                    }
                    return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, StringConstant.InvalidLogin);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw;
            }
        }

        /// <summary>
        /// Call will hit here at everytime for external Login with clientId and it will check for 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<IActionResult> ExternalLogin(string clientId)
        {
            try
            {
                var result = await _appRepository.GetAppDetails(clientId);
                if (result != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        // If already login it will return a redirect url and will be redirect back to another server with access token 
                        var returnUrl = await _oAuthRepository.UserAlreadyLogin(User.Identity.Name, clientId, result.CallbackUrl);
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // RedirectUrl is assign to ViewBag and on client side it will bind with OAuthLogin data
                        ViewBag.returnUrl = result.CallbackUrl;
                        ViewBag.clientId = result.AuthId;
                        return View();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw;
            }
        }
    }
}
