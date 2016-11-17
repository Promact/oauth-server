using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Repository.HttpClientRepository;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.OAuthRepository
{
    public class OAuthRepository : IOAuthRepository
    {
        private readonly IDataRepository<OAuth> _oAuthDataRepository;
        private readonly IHttpClientRepository _httpClientRepository;
        private readonly IConsumerAppRepository _appRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringConstant _stringConstant;

        public OAuthRepository(IDataRepository<OAuth> oAuthDataRepository, IHttpClientRepository httpClientRepository, IConsumerAppRepository appRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStringConstant stringConstant)
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
        private async Task<OAuth> GetDetails(string email, string clientId)
        {
            OAuth oAuth = await _oAuthDataRepository.FirstOrDefaultAsync(x => x.userEmail == email && x.ClientId == clientId);
            return oAuth;
        }


        /// <summary>
        /// Checks whether with this app email is register or not if  not new OAuth will be created.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private async Task<OAuth> OAuthClientChecking(string email, string clientId)
        {
            //checking whether with this app email is register or not if  not new OAuth will be created.
            OAuth oAuth = await GetDetails(email, clientId);
            ConsumerApps consumerApp = await _appRepository.GetAppDetails(clientId);
            if (oAuth == null && consumerApp != null)
            {
                OAuth newOAuth = new OAuth();
                newOAuth.RefreshToken = Guid.NewGuid().ToString();
                newOAuth.userEmail = email;
                newOAuth.AccessToken = Guid.NewGuid().ToString();
                newOAuth.ClientId = clientId;
                Add(newOAuth);
                return newOAuth;
            }
            return oAuth;
        }


        /// <summary>
        /// Fetches the app details from the client.
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<OAuthApplication> GetAppDetailsFromClient(string redirectUrl, string refreshToken, string slackUserName)
        {
            // Assigning Base Address with redirectUrl
            string requestUrl = string.Format("?refreshToken={0}&slackUserName={1}", refreshToken, slackUserName);
            HttpResponseMessage response = await _httpClientRepository.GetAsync(redirectUrl, requestUrl);
            string responseResult = response.Content.ReadAsStringAsync().Result;
            // Transforming Json String to object type OAuthApplication
            return JsonConvert.DeserializeObject<OAuthApplication>(responseResult);
        }


        /// <summary>
        /// Fetches the details of Client using access token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns>true if value is not null otherwise false</returns>
        public async Task<bool> GetDetailsClientByAccessToken(string accessToken)
        {
            OAuth value = await _oAuthDataRepository.FirstOrDefaultAsync(x => x.AccessToken == accessToken);
            if (value != null)
                return true;
            else
                return false;
        }


        /// <summary>
        /// Returns appropriate Url for the user to be redirected to
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="clientId"></param>
        /// <param name="callBackUrl"></param>
        /// <returns>returUrl</returns>
        public async Task<string> UserAlreadyLogin(string userName, string clientId, string callBackUrl)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            OAuth oAuth = await OAuthClientChecking(user.Email, clientId);
            OAuthApplication clientResponse = await GetAppDetailsFromClient(callBackUrl, oAuth.RefreshToken);
            return string.Format("{0}?accessToken={1}&email={2}&slackUserName={3}&userId={4}", clientResponse.ReturnUrl, oAuth.AccessToken, oAuth.userEmail, user.SlackUserName,user.Id);
        }


        /// <summary>
        /// Signs in the user and redirects to the appropraite url.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>if not successfully signed-in then return empty string if successfully signed-in and credentials match then return redirect url</returns>
        public async Task<string> UserNotAlreadyLogin(OAuthLogin model)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                ApplicationUser appUser = await _userManager.FindByEmailAsync(model.Email);
                OAuth oAuth = await OAuthClientChecking(model.Email, model.ClientId);
                OAuthApplication clientResponse = await GetAppDetailsFromClient(model.RedirectUrl, oAuth.RefreshToken);
                // Checking whether request client is equal to response client
                if (model.ClientId == clientResponse.ClientId)
                {
                    //Getting app details from clientId or AuthId
                    ConsumerApps consumerApp = await _appRepository.GetAppDetails(clientResponse.ClientId);
                    // Refresh token and app's secret is checking if match then accesstoken will be send
                    if (consumerApp.AuthSecret == clientResponse.ClientSecret && clientResponse.RefreshToken == oAuth.RefreshToken)
                    {
                        ApplicationUser user = await _userManager.FindByEmailAsync(oAuth.userEmail);
                        return string.Format("{0}?accessToken={1}&email={2}&slackUserName={3}&userId={4}", clientResponse.ReturnUrl, oAuth.AccessToken, oAuth.userEmail, user.SlackUserName,user.Id);
                    }
                }
            }
            return _stringConstant.EmptyString;
        }


    }
}
