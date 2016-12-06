using Promact.Oauth.Server.Repository.OAuthRepository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using System.Threading.Tasks;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;

namespace Promact.Oauth.Server.Tests
{
    public class OAuthRepositoryTest : BaseProvider
    {
        private readonly IOAuthRepository _oAuthRepository;
        private readonly IDataRepository<OAuth> _oAuthDataRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConsumerAppRepository _appRepository;
        private readonly IStringConstant _stringConstant;
        public OAuthRepositoryTest() : base()
        {
            _oAuthRepository = serviceProvider.GetService<IOAuthRepository>();
            _oAuthDataRepository = serviceProvider.GetService<IDataRepository<OAuth>>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _appRepository = serviceProvider.GetService<IConsumerAppRepository>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
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

        ///// <summary>
        ///// Test case to check UserAlreadyLogin of Oauth Repository
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task UserAlreadyLogin()
        //{
        //    var userId = await _userRepository.AddUser(_testUser, _stringConstant.FirstNameSecond);
        //    var consumerId = await _appRepository.AddConsumerApps(app);
        //    var returnUrl = string.Format("{0}?accessToken={1}&email={2}", _stringConstant.CallBackUrl, oAuth.AccessToken, oAuth.userEmail);
        //    var redirectUrl = await _oAuthRepository.UserAlreadyLogin(_stringConstant.UserName, _stringConstant.ClientIdForTest, _stringConstant.CallBackUrl);
        //    Assert.Equal(redirectUrl, returnUrl);
        //}

        ///// <summary>
        ///// Test case to check UserNotAlreadyLogin for true value of Oauth Repository
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task UserNotAlreadyLogin()
        //{
        //    var userId = await _userRepository.AddUser(_testUser, _stringConstant.FirstNameSecond);
        //    var consumerId = await _appRepository.AddConsumerApps(app);
        //    var returnUrl = string.Format("{0}?accessToken={1}&email={2}&slackUserName={3}", _stringConstant.CallBackUrl, oAuth.AccessToken, oAuth.userEmail, _stringConstant.SlackUserName);
        //    var response = await _oAuthRepository.UserNotAlreadyLogin(login);
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

       

        //private UserAc _testUser = new UserAc()
        //{
        //    Email = _stringConstant.UserName,
        //    FirstName = _stringConstant.FirstName,
        //    LastName = _stringConstant.LastName,
        //    IsActive = true,
        //    UserName = _stringConstant.UserName,
        //    SlackUserName = _stringConstant.FirstName,
        //    JoiningDate = DateTime.UtcNow,
        //    RoleName = _stringConstant.Employee
        //};

        //private ConsumerAppsAc app = new ConsumerAppsAc()
        //{
        //    AuthId = _stringConstant.ClientIdForTest,
        //    AuthSecret = _stringConstant.AccessToken,
        //    CallbackUrl = _stringConstant.CallBackUrl,
        //    CreatedBy = _stringConstant.FirstName,
        //    Description = _stringConstant.ConsumerDescription,
        //    Name = _stringConstant.ConsumerAppNameDemo1
        //};

       
    }
}
