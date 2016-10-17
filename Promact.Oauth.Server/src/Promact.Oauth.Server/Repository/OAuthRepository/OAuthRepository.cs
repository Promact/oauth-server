using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Repository.HttpClientRepository;
using System;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.OAuthRepository
{
    public class OAuthRepository:IOAuthRepository
    {
        private readonly IDataRepository<OAuth> _oAuthDataRepository;
        private readonly IHttpClientRepository _httpClientRepository;
        private readonly IConsumerAppRepository _appRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly StringConstant _stringConstant;

        public OAuthRepository(IDataRepository<OAuth> oAuthDataRepository, IHttpClientRepository httpClientRepository,IConsumerAppRepository appRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, StringConstant stringConstant)
        {
            _oAuthDataRepository = oAuthDataRepository;
            _httpClientRepository = httpClientRepository;
            _appRepository = appRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _stringConstant = stringConstant;
        }

        /// <summary>
        /// Method to add OAuth table
        /// </summary>
        /// <param name="model"></param>
        private void Add(OAuth model)
        {
            _oAuthDataRepository.Add(model);
            _oAuthDataRepository.Save();
        }

        /// <summary>
        /// To get details of a OAuth Access for an email and corresponding to app
        /// </summary>
        /// <param name="email"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private OAuth GetDetails(string email,string clientId)
        {
            var oAuth = _oAuthDataRepository.FirstOrDefault(x => x.userEmail == email && x.ClientId == clientId);
            return oAuth;
        }

        private OAuth OAuthClientChecking(string email,string clientId)
        {
            //checking whether with this app email is register or not if  not new OAuth will be created.
            var oAuth = GetDetails(email, clientId);
            var app = _appRepository.GetAppDetails(clientId);
            if (oAuth == null && app!=null)
            {
                oAuth = new OAuth();
                oAuth.RefreshToken = Guid.NewGuid().ToString();
                oAuth.userEmail = email;
                oAuth.AccessToken = Guid.NewGuid().ToString();
                oAuth.ClientId = clientId;
                Add(oAuth);
            }
            return oAuth;
        }

        private async Task<OAuthApplication> GetAppDetailsFromClient(string redirectUrl, string refreshToken)
        {
            // Assigning Base Address with redirectUrl
            var requestUrl = string.Format("?refreshToken={0}", refreshToken);
            var response = await _httpClientRepository.GetAsync(redirectUrl, requestUrl);
            var responseResult = response.Content.ReadAsStringAsync().Result;
            // Transforming Json String to object type OAuthApplication
            var content = JsonConvert.DeserializeObject<OAuthApplication>(responseResult);
            return content;
        }

        public bool GetDetailsClientByAccessToken(string accessToken)
        {
            var value = _oAuthDataRepository.FirstOrDefault(x => x.AccessToken == accessToken);
            if (value != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> UserAlreadyLogin(string userName, string clientId, string callBackUrl)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var oAuth = OAuthClientChecking(user.Email, clientId);
            var clientResponse = await GetAppDetailsFromClient(callBackUrl, oAuth.RefreshToken);
            var returnUrl = string.Format("{0}?accessToken={1}&email={2}&slackUserName={3}", clientResponse.ReturnUrl, oAuth.AccessToken, oAuth.userEmail, user.SlackUserName);
            return returnUrl;
        }

        public async Task<string> UserNotAlreadyLogin(OAuthLogin model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var oAuth = OAuthClientChecking(model.Email, model.ClientId);
                var clientResponse = await GetAppDetailsFromClient(model.RedirectUrl, oAuth.RefreshToken);
                // Checking whether request client is equal to response client
                if (model.ClientId == clientResponse.ClientId)
                {
                    //Getting app details from clientId or AuthId
                    var app = await _appRepository.GetAppDetails(clientResponse.ClientId);
                    // Refresh token and app's secret is checking if match then accesstoken will be send
                    if (app.AuthSecret == clientResponse.ClientSecret && clientResponse.RefreshToken == oAuth.RefreshToken)
                    {
                        var user = await _userManager.FindByEmailAsync(oAuth.userEmail);
                        var returnUrl = string.Format("{0}?accessToken={1}&email={2}&slackUserName={3}", clientResponse.ReturnUrl, oAuth.AccessToken, oAuth.userEmail, user.SlackUserName);
                        return returnUrl;
                    }
                }
            }
            return _stringConstant.EmptyString;
        }
    }
}
