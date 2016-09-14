using Promact.Oauth.Server.Repository.OAuthRepository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using System.Threading.Tasks;
using Promact.Oauth.Server.Repository;
using System;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;

namespace Promact.Oauth.Server.Tests
{
    public class OAuthRepositoryTest : BaseProvider
    {
        private readonly IOAuthRepository _oAuthRepository;
        private readonly IDataRepository<OAuth> _oAuthDataRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConsumerAppRepository _appRepository;
        public OAuthRepositoryTest() : base()
        {
            _oAuthRepository = serviceProvider.GetService<IOAuthRepository>();
            _oAuthDataRepository = serviceProvider.GetService<IDataRepository<OAuth>>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _appRepository = serviceProvider.GetService<IConsumerAppRepository>();
        }

        /// <summary>
        /// Test case to check GetDetailsClientByAccessToken for true value of Oauth Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetDetailsClientByAccessToken()
        {
            _oAuthDataRepository.Add(oAuth);
            _oAuthDataRepository.Save();
            var result = _oAuthRepository.GetDetailsClientByAccessToken(StringConstant.AccessToken);
            Assert.Equal(result, true);
        }

        /// <summary>
        /// Test case to check GetDetailsClientByAccessToken for false value of Oauth Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetDetailsClientByAccessTokenForWrongValue()
        {
            _oAuthDataRepository.Add(oAuth);
            _oAuthDataRepository.Save();
            var result = _oAuthRepository.GetDetailsClientByAccessToken(StringConstant.ClientIdForTest);
            Assert.Equal(result, false);
        }

        ///// <summary>
        ///// Test case to check UserAlreadyLogin of Oauth Repository
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task UserAlreadyLogin()
        //{
        //    var userId = await _userRepository.AddUser(_testUser, StringConstant.FirstNameSecond);
        //    var consumerId = await _appRepository.AddConsumerApps(app);
        //    var returnUrl = string.Format("{0}?accessToken={1}&email={2}", StringConstant.CallBackUrl, oAuth.AccessToken, oAuth.userEmail);
        //    var redirectUrl = await _oAuthRepository.UserAlreadyLogin(StringConstant.UserName, StringConstant.ClientIdForTest, StringConstant.CallBackUrl);
        //    Assert.Equal(redirectUrl, returnUrl);
        //}

        ///// <summary>
        ///// Test case to check UserNotAlreadyLogin for true value of Oauth Repository
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task UserNotAlreadyLogin()
        //{
        //    var userId = await _userRepository.AddUser(_testUser, StringConstant.FirstNameSecond);
        //    var consumerId = await _appRepository.AddConsumerApps(app);
        //    var returnUrl = string.Format("{0}?accessToken={1}&email={2}&slackUserName={3}", StringConstant.CallBackUrl, oAuth.AccessToken, oAuth.userEmail, StringConstant.SlackUserName);
        //    var response = await _oAuthRepository.UserNotAlreadyLogin(login);
        //    Assert.Equal(response, returnUrl);
        //}

        /// <summary>
        /// Test case to check UserNotAlreadyLogin for false value of Oauth Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserNotAlreadyLoginForWrongValue()
        {
            var response = await _oAuthRepository.UserNotAlreadyLogin(login);
            Assert.Equal(response, StringConstant.EmptyString);
        }

        private OAuth oAuth = new OAuth()
        {
            AccessToken = StringConstant.AccessToken,
            ClientId = StringConstant.ClientIdForTest,
            RefreshToken = StringConstant.ClientIdForTest,
            userEmail = StringConstant.UserName
        };

        private UserAc _testUser = new UserAc()
        {
            Email = StringConstant.UserName,
            FirstName = StringConstant.FirstName,
            LastName = StringConstant.LastName,
            IsActive = true,
            UserName = StringConstant.UserName,
            SlackUserName = StringConstant.FirstName,
            JoiningDate = DateTime.UtcNow,
            RoleName = StringConstant.Employee
        };

        private ConsumerAppsAc app = new ConsumerAppsAc()
        {
            AuthId = StringConstant.ClientIdForTest,
            AuthSecret = StringConstant.AccessToken,
            CallbackUrl = StringConstant.CallBackUrl,
            CreatedBy = StringConstant.FirstName,
            Description = StringConstant.ConsumerDescription,
            Name = StringConstant.ConsumerAppNameDemo1
        };

        private OAuthLogin login = new OAuthLogin()
        {
            ClientId = StringConstant.ClientIdForTest,
            Email = StringConstant.UserName,
            Password = StringConstant.PasswordForTest,
            RedirectUrl = StringConstant.CallBackUrl
        };
    }
}
