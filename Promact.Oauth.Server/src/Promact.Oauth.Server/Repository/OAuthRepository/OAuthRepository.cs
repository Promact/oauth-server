using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.OAuthRepository
{
    public class OAuthRepository : IOAuthRepository
    {
        private readonly IDataRepository<OAuth> _oAuthDataRepository;
        private readonly IHttpClientService _httpClientRepository;
        private readonly IConsumerAppRepository _appRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringConstant _stringConstant;
        private readonly IOptions<AppSettingUtil> _appSettingUtil;

        public OAuthRepository(IDataRepository<OAuth> oAuthDataRepository,
            IHttpClientService httpClientRepository, IConsumerAppRepository appRepository,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IOptions<AppSettingUtil> appSettingUtil, IStringConstant stringConstant)
        {
            _oAuthDataRepository = oAuthDataRepository;
            _httpClientRepository = httpClientRepository;
            _appRepository = appRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _stringConstant = stringConstant;
            _appSettingUtil = appSettingUtil;
        }

        /// <summary>
        /// Method to add OAuth table
        /// </summary>
        /// <param name="model">OAuth object</param>
        private async Task AddAsync(OAuth model)
        {
            _oAuthDataRepository.Add(model);
            await _oAuthDataRepository.SaveChangesAsync();
        }


        /// <summary>
        /// To get details of a OAuth Access for an email and corresponding to app
        /// </summary>
        /// <param name="email">User's email Id</param>
        /// <param name="clientId">Promact App's Client Id</param>
        /// <returns>object of OAuth</returns>
        private async Task<OAuth> GetDetailsAsync(string email, string clientId)
        {
            OAuth oAuth = await _oAuthDataRepository.FirstOrDefaultAsync(x => x.userEmail == email && x.ClientId == clientId);
            return oAuth;
        }


        /// <summary>
        /// Checks whether with this app ClientId & email, any oAuth details already register or not.
        /// If not new OAuth will be created.
        /// </summary>
        /// <param name="email">User's email Id</param>
        /// <param name="clientId">Promact App's Client Id</param>
        /// <returns>object of OAuth</returns>
        private async Task<OAuth> OAuthClientCheckingAsync(string email, string clientId)
        {
            //checking whether with this app email is register or not if  not new OAuth will be created.
            OAuth oAuth = await GetDetailsAsync(email, clientId);
            ConsumerApps consumerApp = await _appRepository.GetAppDetailsAsync(clientId);
            if (oAuth == null && consumerApp != null)
            {
                OAuth newOAuth = new OAuth();
                newOAuth.RefreshToken = Guid.NewGuid().ToString();
                newOAuth.userEmail = email;
                newOAuth.AccessToken = Guid.NewGuid().ToString();
                newOAuth.ClientId = clientId;
                await AddAsync(newOAuth);
                return newOAuth;
            }
            return oAuth;
        }


        /// <summary>
        /// Fetches the app details from the client server.
        /// </summary>
        /// <param name="redirectUrl">Redirect url of OAuth</param>
        /// <param name="refreshToken">Promact App's refresh token</param>
        /// <param name="slackUserName">User's slack name</param>
        /// <returns>object of OAuthApplication</returns>
        private async Task<OAuthApplication> GetAppDetailsFromClientAsync(string redirectUrl, string refreshToken, string slackUserName)
        {
            // Assigning Base Address with redirectUrl
            var requestUrl = string.Format(_stringConstant.GetAppDetailsFromClientAsyncUrl, refreshToken, slackUserName);
            var response = await _httpClientRepository.GetAsync(redirectUrl, requestUrl);
            // Transforming Json String to object type OAuthApplication
            return JsonConvert.DeserializeObject<OAuthApplication>(response);
        }


        /// <summary>
        /// Fetches the details of Client using access token
        /// </summary>
        /// <param name="accessToken">AccessToken of User</param>
        /// <returns>true if value is not null otherwise false</returns>
        public async Task<bool> GetDetailsClientByAccessTokenAsync(string accessToken)
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
        /// <param name="userName">User's username</param>
        /// <param name="clientId">Promact app's Client Id</param>
        /// <param name="callBackUrl">Callback url of oAuth</param>
        /// <returns>returUrl</returns>
        public async Task<string> UserAlreadyLoginAsync(string userName, string clientId, string callBackUrl)
        {
            string returnUrl = _stringConstant.EmptyString;
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            OAuth oAuth = await OAuthClientCheckingAsync(user.Email, clientId);
            if (oAuth != null)
            {
                OAuthApplication clientResponse = await GetAppDetailsFromClientAsync(callBackUrl, oAuth.RefreshToken,
                    user.SlackUserName);
                if (clientResponse != null)
                {
                    if (!String.IsNullOrEmpty(clientResponse.UserId))
                    {
                        user.SlackUserId = clientResponse.UserId;
                        await _userManager.UpdateAsync(user);
                        returnUrl = string.Format(_stringConstant.OAuthAfterLoginResponseUrl, clientResponse.ReturnUrl,
                            oAuth.AccessToken, oAuth.userEmail, user.SlackUserId, user.Id);
                    }
                    else
                        returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                             + _stringConstant.Message + _stringConstant.InCorrectSlackName;
                }
                else
                    returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                                 + _stringConstant.Message + _stringConstant.PromactAppNotSet;
            }
            else
            {
                var errorMessage = string.Format(_stringConstant.PromactAppNotFoundClientId, clientId);
                returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                                 + _stringConstant.Message + errorMessage;
            }
            return returnUrl;
        }



        /// <summary>
        /// Signs in the user and redirects to the appropraite url.
        /// </summary>
        /// <param name="model">OAuth login object</param>
        /// <returns>returnUrl</returns>
        public async Task<string> UserNotAlreadyLoginAsync(OAuthLogin model)
        {
            string returnUrl = _stringConstant.EmptyString;
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                ApplicationUser appUser = await _userManager.FindByEmailAsync(model.Email);
                OAuth oAuth = await OAuthClientCheckingAsync(model.Email, model.ClientId);
                if (oAuth != null)
                {
                    OAuthApplication clientResponse = await GetAppDetailsFromClientAsync(model.RedirectUrl, oAuth.RefreshToken, appUser.SlackUserName);
                    if (clientResponse != null)
                    {
                        if (!String.IsNullOrEmpty(clientResponse.UserId))
                        {
                            appUser.SlackUserId = clientResponse.UserId;
                            await _userManager.UpdateAsync(appUser);
                            // Checking whether request client is equal to response client
                            if (model.ClientId == clientResponse.ClientId)
                            {
                                //Getting app details from clientId or AuthId
                                ConsumerApps consumerApp = await _appRepository.GetAppDetailsAsync(clientResponse.ClientId);
                                // Refresh token and app's secret is checking if match then accesstoken will be send
                                if (consumerApp.AuthSecret == clientResponse.ClientSecret && clientResponse.RefreshToken == oAuth.RefreshToken)
                                {
                                    ApplicationUser user = await _userManager.FindByEmailAsync(oAuth.userEmail);
                                    returnUrl = string.Format(_stringConstant.OAuthAfterLoginResponse, clientResponse.ReturnUrl, oAuth.AccessToken, oAuth.userEmail, user.SlackUserId, user.Id);
                                }
                                else
                                {
                                    var errorMessage = string.Format(_stringConstant.PromactAppNotFoundClientSecret, clientResponse.ClientSecret);
                                    returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl + _stringConstant.Message + errorMessage;
                                }
                            }
                            else
                            {
                                var errorMessage = string.Format(_stringConstant.PromactAppNotFoundClientId, clientResponse.ClientId);
                                returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl + _stringConstant.Message + errorMessage;
                            }
                        }
                        else
                            returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl + _stringConstant.Message
                                + _stringConstant.InCorrectSlackName;
                    }
                    else
                        returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                                     + _stringConstant.Message + _stringConstant.PromactAppNotSet;
                }
                else
                {
                    var errorMessage = string.Format(_stringConstant.PromactAppNotFoundClientId, model.ClientId);
                    returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                                     + _stringConstant.Message + errorMessage;
                }
            }
            else
            {
                returnUrl = string.Format(_stringConstant.OAuthExternalLoginUrl, _appSettingUtil.Value.PromactOAuthUrl, model.ClientId);
            }
            return returnUrl;
        }
    }
}
