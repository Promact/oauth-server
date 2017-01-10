using Promact.Oauth.Server.Repository.OAuthRepository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using System.Threading.Tasks;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using Moq;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Promact.Oauth.Server.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Promact.Oauth.Server.Tests
{
    public class OAuthRepositoryTest : BaseProvider
    {
        private readonly IOAuthRepository _oAuthRepository;
        private readonly IDataRepository<OAuth> _oAuthDataRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConsumerAppRepository _appRepository;
        private readonly IStringConstant _stringConstant;
        private readonly Mock<IHttpClientService> _mockHttpClient;
        //private UserAc _testUser = new UserAc();
        private ConsumerAppsAc app = new ConsumerAppsAc();
        private OAuth oAuth = new OAuth();
        private OAuthLogin oAuthLogin = new OAuthLogin();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapperContext;
        private readonly IOptions<AppSettingUtil> _appSettingUtil;

        public OAuthRepositoryTest() : base()
        {
            _oAuthRepository = serviceProvider.GetService<IOAuthRepository>();
            _oAuthDataRepository = serviceProvider.GetService<IDataRepository<OAuth>>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _appRepository = serviceProvider.GetService<IConsumerAppRepository>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
            _mockHttpClient = serviceProvider.GetService<Mock<IHttpClientService>>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _mapperContext = serviceProvider.GetService<IMapper>();
            _appSettingUtil = serviceProvider.GetService<IOptions<AppSettingUtil>>();
            Initialize();

        }

        /// <summary>
        /// Test case to check GetDetailsClientByAccessToken for true value of Oauth Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetDetailsClientByAccessTokenAsync()
        {
            OAuth oAuth = new OAuth()
            {
                AccessToken = _stringConstant.AccessToken,
                ClientId = _stringConstant.ClientIdForTest,
                RefreshToken = _stringConstant.ClientIdForTest,
                userEmail = _stringConstant.UserName
            };
            _oAuthDataRepository.Add(oAuth);
            _oAuthDataRepository.Save();
            var result = await _oAuthRepository.GetDetailsClientByAccessTokenAsync(_stringConstant.AccessToken);
            Assert.Equal(result, true);
        }

        /// <summary>
        /// Test case to check GetDetailsClientByAccessToken for false value of Oauth Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetDetailsClientByAccessTokenForWrongValueAsync()
        {
            OAuth oAuth = new OAuth()
            {
                AccessToken = _stringConstant.AccessToken,
                ClientId = _stringConstant.ClientIdForTest,
                RefreshToken = _stringConstant.ClientIdForTest,
                userEmail = _stringConstant.UserName
            };
            _oAuthDataRepository.Add(oAuth);
            _oAuthDataRepository.Save();
            var result = await _oAuthRepository.GetDetailsClientByAccessTokenAsync(_stringConstant.ClientIdForTest);
            Assert.Equal(result, false);
        }

        /// <summary>
        /// Test case to check UserAlreadyLogin of Oauth Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserAlreadyLoginAsync()
        {

            string userId = await CreateMockAndUserAsync();
            var consumerId = await _appRepository.AddConsumerAppsAsync(app);
            var appDetails = await _appRepository.GetConsumerAppByIdAsync(consumerId);
            _oAuthDataRepository.AddAsync(oAuth);
            await _oAuthDataRepository.SaveChangesAsync();
            var requestUrl = MockingGetAppDetailsFromClientAsync(_stringConstant.GetAppDetailsFromClientAsyncResponse);
            var returnUrl = string.Format("{0}?accessToken={1}&email={2}&slackUserId={3}&userId={4}", _stringConstant.ReturnUrl, 
                _stringConstant.AccessToken, _stringConstant.UserName, _stringConstant.SlackUserId, userId);
            var redirectUrl = await _oAuthRepository.UserAlreadyLoginAsync(_stringConstant.UserName, _stringConstant.ClientIdForTest, _stringConstant.CallBackUrl);
            Assert.Equal(redirectUrl, returnUrl);
            _mockHttpClient.Verify(x => x.GetAsync(_stringConstant.CallBackUrl, requestUrl), Times.Once);
        }

        /// <summary>
        /// Test case to check UserAlreadyLogin of Oauth Repository for In-Correct Slack Name Error
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserAlreadyLoginInCorrectSlackNameErrorAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var consumerId = await _appRepository.AddConsumerAppsAsync(app);
            var appDetails = await _appRepository.GetConsumerAppByIdAsync(consumerId);
            _oAuthDataRepository.AddAsync(oAuth);
            await _oAuthDataRepository.SaveChangesAsync();
            var requestUrl = MockingGetAppDetailsFromClientAsync(_stringConstant.GetAppDetailsFromClientAsyncResponseInCorrectSlackName);
            var returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                             + _stringConstant.Message + _stringConstant.InCorrectSlackName;
            var redirectUrl = await _oAuthRepository.UserAlreadyLoginAsync(_stringConstant.UserName, _stringConstant.ClientIdForTest, _stringConstant.CallBackUrl);
            Assert.Equal(redirectUrl, returnUrl);
            _mockHttpClient.Verify(x => x.GetAsync(_stringConstant.CallBackUrl, requestUrl), Times.Once);
        }

        /// <summary>
        /// Test case to check UserAlreadyLogin of Oauth Repository for Promact App Not Set
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserAlreadyLoginForWrongValueAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var consumerId = await _appRepository.AddConsumerAppsAsync(app);
            _oAuthDataRepository.AddAsync(oAuth);
            await _oAuthDataRepository.SaveChangesAsync();
            var requestUrl = MockingGetAppDetailsFromClientAsync(_stringConstant.EmptyString);
            var returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                                 + _stringConstant.Message + _stringConstant.PromactAppNotSet;
            var redirectUrl = await _oAuthRepository.UserAlreadyLoginAsync(_stringConstant.UserName, _stringConstant.ClientIdForTest, _stringConstant.CallBackUrl);
            Assert.Equal(redirectUrl, returnUrl);
            _mockHttpClient.Verify(x => x.GetAsync(_stringConstant.CallBackUrl, requestUrl), Times.Once);
        }

        /// <summary>
        /// Test case to check UserAlreadyLogin of Oauth Repository for Promact App Not Found ClientId
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserAlreadyLoginForWrongValuePromactAppNotFoundClientIdAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var errorMessage = string.Format(_stringConstant.PromactAppNotFoundClientId, _stringConstant.ClientIdForTest);
            var returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                             + _stringConstant.Message + errorMessage;
            var redirectUrl = await _oAuthRepository.UserAlreadyLoginAsync(_stringConstant.UserName, _stringConstant.ClientIdForTest, _stringConstant.CallBackUrl);
            Assert.Equal(redirectUrl, returnUrl);
        }

        /// <summary>
        /// Test case to check UserNotAlreadyLogin for true value for Promact App Not Found ClientSecret
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserNotAlreadyLoginPromactAppNotFoundClientSecretAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var user = await _userManager.FindByIdAsync(userId);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, code, _stringConstant.PasswordForTest);
            var consumerId = await _appRepository.AddConsumerAppsAsync(app);
            var appDetails = await _appRepository.GetConsumerAppByIdAsync(consumerId);
            var returnGetAppDetailsFromClientAsync = _stringConstant.GetAppDetailsFromClientAsyncResponse;
            oAuthLogin.ClientId = appDetails.AuthId;
            var oAuth = JsonConvert.DeserializeObject<OAuthApplication>(_stringConstant.GetAppDetailsFromClientAsyncResponse);
            oAuth.ClientId = appDetails.AuthId;
            oAuth.ClientSecret = appDetails.AuthSecret;
            var responseExpected = JsonConvert.SerializeObject(oAuth);
            var requestUrl = MockingGetAppDetailsFromClientAsync(responseExpected);
            var errorMessage = string.Format(_stringConstant.PromactAppNotFoundClientSecret, appDetails.AuthSecret);
            var returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl + _stringConstant.Message + errorMessage;
            var response = await _oAuthRepository.UserNotAlreadyLoginAsync(oAuthLogin);
            Assert.Equal(response, returnUrl);
            _mockHttpClient.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Test case to check UserNotAlreadyLogin for true value of Oauth Repository for Promact App Not Found ClientId
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserNotAlreadyLoginPromactAppNotFoundClientIdAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var user = await _userManager.FindByIdAsync(userId);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, code, _stringConstant.PasswordForTest);
            var consumerId = await _appRepository.AddConsumerAppsAsync(app);
            var appDetails = await _appRepository.GetConsumerAppByIdAsync(consumerId);
            var returnGetAppDetailsFromClientAsync = _stringConstant.GetAppDetailsFromClientAsyncResponse;
            oAuthLogin.ClientId = appDetails.AuthId;
            var oAuth = JsonConvert.DeserializeObject<OAuthApplication>(_stringConstant.GetAppDetailsFromClientAsyncResponse);
            var requestUrl = MockingGetAppDetailsFromClientAsync(_stringConstant.GetAppDetailsFromClientAsyncResponse);
            var errorMessage = string.Format(_stringConstant.PromactAppNotFoundClientId, oAuth.ClientId);
            var returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl + _stringConstant.Message + errorMessage;
            var response = await _oAuthRepository.UserNotAlreadyLoginAsync(oAuthLogin);
            Assert.Equal(response, returnUrl);
            _mockHttpClient.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Test case to check UserNotAlreadyLogin for true value of Oauth Repository for In-Correct Slack Name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserNotAlreadyLoginInCorrectSlackNameAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var user = await _userManager.FindByIdAsync(userId);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, code, _stringConstant.PasswordForTest);
            var consumerId = await _appRepository.AddConsumerAppsAsync(app);
            var appDetails = await _appRepository.GetConsumerAppByIdAsync(consumerId);
            oAuthLogin.ClientId = appDetails.AuthId;
            var requestUrl = MockingGetAppDetailsFromClientAsync(_stringConstant.GetAppDetailsFromClientAsyncResponseInCorrectSlackName);
            var returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                             + _stringConstant.Message + _stringConstant.InCorrectSlackName;
            var response = await _oAuthRepository.UserNotAlreadyLoginAsync(oAuthLogin);
            Assert.Equal(response, returnUrl);
            _mockHttpClient.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Test case to check UserNotAlreadyLogin for true value of Oauth Repository for Promact App Not Set
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserNotAlreadyLoginPromactAppNotSetAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var user = await _userManager.FindByIdAsync(userId);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, code, _stringConstant.PasswordForTest);
            var consumerId = await _appRepository.AddConsumerAppsAsync(app);
            var appDetails = await _appRepository.GetConsumerAppByIdAsync(consumerId);
            oAuthLogin.ClientId = appDetails.AuthId;
            var requestUrl = MockingGetAppDetailsFromClientAsync(_stringConstant.EmptyString);
            var returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                                 + _stringConstant.Message + _stringConstant.PromactAppNotSet;
            var response = await _oAuthRepository.UserNotAlreadyLoginAsync(oAuthLogin);
            Assert.Equal(response, returnUrl);
            _mockHttpClient.Verify(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Test case to check UserNotAlreadyLogin for true value of Oauth Repository for Promact App Not Found ClientId OAuth Empty
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserNotAlreadyLoginPromactAppNotFoundClientIdOAuthEmptyAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var errorMessage = string.Format(_stringConstant.PromactAppNotFoundClientId, _stringConstant.ClientIdForTest);
            var returnUrl = _appSettingUtil.Value.PromactErpUrl + _stringConstant.ErpAuthorizeUrl
                             + _stringConstant.Message + errorMessage;
            var redirectUrl = await _oAuthRepository.UserNotAlreadyLoginAsync(oAuthLogin);
            Assert.Equal(redirectUrl, returnUrl); ;
        }

        private void Initialize()
        {
            app.AuthId = _stringConstant.ClientIdForTest;
            app.AuthSecret = _stringConstant.AccessToken;
            app.CallbackUrl = _stringConstant.CallBackUrl;
            app.CreatedBy = _stringConstant.FirstName;
            app.Description = _stringConstant.ConsumerDescription;
            app.Name = _stringConstant.ConsumerAppNameDemo1;
            oAuth.AccessToken = _stringConstant.AccessToken;
            oAuth.ClientId = _stringConstant.ClientIdForTest;
            oAuth.RefreshToken = _stringConstant.ClientIdForTest;
            oAuth.userEmail = _stringConstant.UserName;

            oAuthLogin.ClientId = _stringConstant.ClientIdForTest;
            oAuthLogin.Email = _stringConstant.UserName;
            oAuthLogin.Password = _stringConstant.PasswordForTest;
            oAuthLogin.RedirectUrl = _stringConstant.CallBackUrl;
        }

        private string MockingGetAppDetailsFromClientAsync(string returnValue)
        {
            var response = Task.FromResult(returnValue);
            var requestUrl = string.Format("?refreshToken={0}&slackUserName={1}", oAuth.RefreshToken, _stringConstant.SlackUserName);
            _mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(response);
            return requestUrl;
        }
    }
}
