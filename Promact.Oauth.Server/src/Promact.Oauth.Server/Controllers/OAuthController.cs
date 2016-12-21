using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Repository.OAuthRepository;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    public class OAuthController : BaseController
    {
        private readonly IConsumerAppRepository _appRepository;
        private readonly IOAuthRepository _oAuthRepository;
        private readonly IOptions<AppSettingUtil> _appSettingUtil;
        private readonly IStringConstant _stringConstant;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OAuthController(IConsumerAppRepository appRepository,
            IOAuthRepository oAuthRepository, IOptions<AppSettingUtil> appSettingUtil,
            IStringConstant stringConstant, SignInManager<ApplicationUser> signInManager)
        {
            _appRepository = appRepository;
            _oAuthRepository = oAuthRepository;
            _appSettingUtil = appSettingUtil;
            _stringConstant = stringConstant;
            _signInManager = signInManager;
        }

        /**
        * @api {post} OAuth/Login 
        * @apiVersion 1.0.0
        * @apiName Login
        * @apiGroup Login
        * @apiParam {OAuthLogin} Name  login
        * @apiParamExample {json} Request-Example:
        *        {
        *             "Email":"siddhartha@promactinfo.com",
        *             "Password":"***********",
        *             "RedirectUrl":"http://localhost:1234",
        *             "ClientId":"d1tge1ygr1t321yhfty",
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "Description":"Redirect to Authorize user to external server with proper message"
        * }
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 408 HttpRequestException 
        * {
        *     "Description":"Redirect to external login page"
        * }
        */
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(OAuthLogin login)
        {
            try
            {
                var redirectUrl = string.Format(_stringConstant.OAuthExternalLoginUrl, _appSettingUtil.Value.PromactOAuthUrl, login.ClientId);
                if (ModelState.IsValid)
                {
                    redirectUrl = await _oAuthRepository.UserNotAlreadyLoginAsync(login);
                }
                return Redirect(redirectUrl);
            }
            catch (HttpRequestException ex)
            {
                ex.ToExceptionless().Submit();
                // If catch error, redirect to promact OAuth message page and will display error.
                return Redirect("~/Home/Error");
            }
        }


        /**
        * @api {get} OAuth/ExternalLogin
        * @apiVersion 1.0.0
        * @apiName ExternalLogin
        * @apiGroup ExternalLogin
        * @apiParam {string} Name  clientId    
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "Description":"Redirect to Promact OAuth server external login page. If user already login, then Redirect to Authorize user to external server."
        * }
        * HTTP/1.1 408 HttpRequestException
        * {
        *     "Description":"Redirect to Promact slack server with appropriate message of error
        * }
        */

        public async Task<IActionResult> ExternalLogin(string clientId)
        {
            try
            {
                ConsumerApps result = await _appRepository.GetAppDetailsAsync(clientId);
                if (result != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        // If already login it will return a redirect url and will be redirect back to another server with access token 
                        string returnUrl = await _oAuthRepository.UserAlreadyLoginAsync(User.Identity.Name, clientId, result.CallbackUrl);
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
                var errorMessage = string.Format(_stringConstant.PromactAppNotFoundClientId, clientId);
                return Redirect(_appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                                 + _stringConstant.Message + errorMessage);
            }
            catch (HttpRequestException ex)
            {
                ex.ToExceptionless().Submit();
                // If catch error, redirect to promact OAuth message page and will display error.
                return Redirect("~/Home/Error");
            }
        }
    }
}
