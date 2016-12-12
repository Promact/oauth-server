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
using Promact.Oauth.Server.Repository.HttpClientRepository;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace Promact.Oauth.Server.Tests
{
    public class OAuthRepositoryTest : BaseProvider
    {
        private readonly IOAuthRepository _oAuthRepository;
        private readonly IDataRepository<OAuth> _oAuthDataRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConsumerAppRepository _appRepository;
        private readonly IStringConstant _stringConstant;
        private readonly Mock<IHttpClientRepository> _mockHttpClient;
        private UserAc _testUser = new UserAc();
        private ConsumerAppsAc app = new ConsumerAppsAc();
        private OAuth oAuth = new OAuth();
        private OAuthLogin oAuthLogin = new OAuthLogin();
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapperContext;

        public OAuthRepositoryTest() : base()
        {
            _oAuthRepository = serviceProvider.GetService<IOAuthRepository>();
            _oAuthDataRepository = serviceProvider.GetService<IDataRepository<OAuth>>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _appRepository = serviceProvider.GetService<IConsumerAppRepository>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
            _mockHttpClient = serviceProvider.GetService<Mock<IHttpClientRepository>>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _mapperContext = serviceProvider.GetService<IMapper>();
            Initialize();

        }

        /// <summary>
        /// Test case to check GetDetailsClientByAccessToken for true value of Oauth Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetDetailsClientByAccessToken()
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
        public async Task GetDetailsClientByAccessTokenForWrongValue()
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
        public async Task UserAlreadyLogin()
        {
            var userId = await _userRepository.AddUser(_testUser, _stringConstant.FirstNameSecond);
            var consumerId = await _appRepository.AddConsumerAppsAsync(app);
            var appDetails = await _appRepository.GetConsumerAppByIdAsync(consumerId);
            _oAuthDataRepository.AddAsync(oAuth);
            await _oAuthDataRepository.SaveChangesAsync();
            var requestUrl = MockingGetAppDetailsFromClientAsync(_stringConstant.GetAppDetailsFromClientAsyncResponse);
            var returnUrl = string.Format("{0}?accessToken={1}&email={2}&slackUserId={3}&userId={4}", _stringConstant.ReturnUrl, _stringConstant.AccessToken, _stringConstant.UserName, _stringConstant.UserSlackId, _testUser.Id);
            var redirectUrl = await _oAuthRepository.UserAlreadyLoginAsync(_stringConstant.UserName, _stringConstant.ClientIdForTest, _stringConstant.CallBackUrl);
            Assert.Equal(redirectUrl, returnUrl);
            _mockHttpClient.Verify(x => x.GetAsync(_stringConstant.CallBackUrl, requestUrl), Times.Once);
        }

        /// <summary>
        /// Test case to check UserAlreadyLogin of Oauth Repository for wrong value
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserAlreadyLoginForWrongValue()
        {
            var userId = await _userRepository.AddUser(_testUser, _stringConstant.FirstNameSecond);
            var consumerId = await _appRepository.AddConsumerAppsAsync(app);
            _oAuthDataRepository.AddAsync(oAuth);
            await _oAuthDataRepository.SaveChangesAsync();
            var requestUrl = MockingGetAppDetailsFromClientAsync(_stringConstant.EmptyString);
            var redirectUrl = await _oAuthRepository.UserAlreadyLoginAsync(_stringConstant.UserName, _stringConstant.ClientIdForTest, _stringConstant.CallBackUrl);
            Assert.Equal(redirectUrl, _stringConstant.EmptyString);
            _mockHttpClient.Verify(x => x.GetAsync(_stringConstant.CallBackUrl, requestUrl), Times.Once);
        }

        /// <summary>
        /// Test case to check UserNotAlreadyLogin for true value of Oauth Repository
        /// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task UserNotAlreadyLogin()
        //{
        //    var returnUrl = MockingGetAppDetailsFromClientAsync(_stringConstant.GetAppDetailsFromClientAsyncResponse);
        //    var userId = await _userRepository.AddUser(_testUser, _stringConstant.FirstNameSecond);
        //    var user = await _userManager.FindByIdAsync(userId);
        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    await _userManager.ResetPasswordAsync(user, code, _stringConstant.PasswordForTest);
        //    var consumerId = await _appRepository.AddConsumerAppsAsync(app);
        //    var appDetails = await _appRepository.GetConsumerAppByIdAsync(consumerId);
        //    var returnGetAppDetailsFromClientAsync = string.Format(_stringConstant.GetAppDetailsFromClientAsyncResponse, appDetails.AuthId, appDetails.AuthSecret);
        //    oAuthLogin.ClientId = appDetails.AuthId;
        //    var response = await _oAuthRepository.UserNotAlreadyLoginAsync(oAuthLogin);
        //    Assert.Equal(response, returnUrl);
        //}

        /// <summary>
        /// Test case to check UserNotAlreadyLogin for false value of Oauth Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserNotAlreadyLoginForWrongValue()
        {
            OAuthLogin login = new OAuthLogin()
            {
                ClientId = _stringConstant.ClientIdForTest,
                Email = _stringConstant.UserName,
                Password = _stringConstant.PasswordForTest,
                RedirectUrl = _stringConstant.CallBackUrl
            };
            var response = await _oAuthRepository.UserNotAlreadyLoginAsync(login);
            Assert.Equal(response, _stringConstant.EmptyString);
        }

        private void Initialize()
        {
            _testUser.Id = _stringConstant.UserId;
            _testUser.Email = _stringConstant.UserName;
            _testUser.FirstName = _stringConstant.FirstName;
            _testUser.LastName = _stringConstant.LastName;
            _testUser.IsActive = true;
            _testUser.UserName = _stringConstant.UserName;
            _testUser.SlackUserName = _stringConstant.SlackUserName;
            _testUser.JoiningDate = DateTime.UtcNow;
            _testUser.RoleName = _stringConstant.Employee;

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
