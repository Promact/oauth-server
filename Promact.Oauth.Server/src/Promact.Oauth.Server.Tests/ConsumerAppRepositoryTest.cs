using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Data_Repository;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.ExceptionHandler;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;


namespace Promact.Oauth.Server.Tests
{
    public class ConsumerAppRepositoryTest : BaseProvider
    {
        #region Private Variables
        private readonly IConsumerAppRepository _consumerAppRespository;
        private readonly IDataRepository<IdentityServer4.EntityFramework.Entities.Client, ConfigurationDbContext> _clientContext;
        private readonly IStringConstant _stringConstant;
        #endregion

        #region Construtor
        public ConsumerAppRepositoryTest() : base()
        {
            _consumerAppRespository = serviceProvider.GetService<IConsumerAppRepository>();
            _clientContext = serviceProvider.GetService<IDataRepository<IdentityServer4.EntityFramework.Entities.Client, ConfigurationDbContext>>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
        }
        #endregion

        #region Test Case

        /// <summary>
        /// This test case for add Consumer Apps
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task AddConsumerAppsAsync()
        {
            ConsumerApps consumerApp = GetConsumerApp();
            var app = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            var clientApp = await _clientContext.FirstOrDefaultAsync(x => x.ClientId == app.ClientId);
            Assert.Equal(clientApp.AllowAccessTokensViaBrowser, true);
        }

        /// <summary>
        /// This test case used for check client Id is unique or not.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task AddConsumerAppsExceptionAsync()
        {
            ConsumerApps consumerApp = GetConsumerApp();
            var app = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            consumerApp.Id = 0;
            var result = await Assert.ThrowsAsync<ConsumerAppNameIsAlreadyExists>(() => 
            _consumerAppRespository.AddConsumerAppsAsync(consumerApp));
            Assert.Equal(_stringConstant.ExceptionMessageConsumerAppNameIsAlreadyExists, result.Message);
        }

        /// <summary>
        /// This test case used for check app details fetch by valid client clientId.-An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetAppDetailsByClientIdAsync()
        {
            ConsumerApps consumerApp = GetConsumerApp();
            var app = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            var consumerApps = await _consumerAppRespository.GetAppDetailsByClientIdAsync(_stringConstant.RandomClientId);
            Assert.Equal(consumerApps.Name,_stringConstant.Name);
        }

        /// <summary>
        /// This test case used for check GetAppDetailsByClientIdAsync with invalid clientId
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetAppDetailsByClientIdForExceptionAsync()
        {
            ConsumerApps consumerApp = GetConsumerApp();
            var app = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            var result = await Assert.ThrowsAsync<ConsumerAppNotFound>(() => 
            _consumerAppRespository.GetAppDetailsByClientIdAsync(_stringConstant.RandomClientSecret));
            Assert.Equal(result.Message, _stringConstant.ExceptionMessageConsumerAppNotFound);
        }

        /// <summary>
        /// This test case used for getting list of app
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetListOfConsumerAppsAsync()
        {
            ConsumerApps consumerApp = GetConsumerApp();
            var app = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            var result = await _consumerAppRespository.GetListOfConsumerAppsAsync();
            Assert.Equal(result.Count, 1);
        }

        /// <summary>
        /// This test case used for generating random number in Upper case
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetRandomNumberForTrue()
        {
            var result = _consumerAppRespository.GetRandomNumber(true);
            Assert.NotNull(result);
        }

        /// <summary>
        /// This test case used for generating random number in mix case
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetRandomNumberForFalse()
        {
            var result = _consumerAppRespository.GetRandomNumber(false);
            Assert.NotNull(result);
        }

        /// <summary>
        /// This test case used updating consumerApp
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UpdateConsumerAllDetailsAsync()
        {
            ConsumerApps consumerApp = GetConsumerApp();
            var app = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            var previousAppDetails = await _consumerAppRespository.GetAppDetailsByClientIdAsync(app.ClientId);
            consumerApp.AuthSecret = _stringConstant.AccessToken;
            consumerApp.CallbackUrl = _stringConstant.CallbackUrl;
            consumerApp.LogoutUrl = _stringConstant.CallbackUrl;
            consumerApp.Scopes = new List<AllowedScope>() { AllowedScope.email, AllowedScope.openid, AllowedScope.profile, AllowedScope.project_read, AllowedScope.user_read };
            consumerApp.AuthId = _stringConstant.SlackUserId;
            consumerApp.Name = _stringConstant.ProjectName;
            consumerApp.Id = previousAppDetails.Id;
            await _consumerAppRespository.UpdateConsumerAppsAsync(consumerApp);
            var updateApp = await _consumerAppRespository.GetAppDetailsByClientIdAsync(_stringConstant.SlackUserId);
            #region AuthId Assert
            Assert.Equal(previousAppDetails.AuthId, _stringConstant.RandomClientId);
            Assert.Equal(updateApp.AuthId, _stringConstant.SlackUserId);
            Assert.NotEqual(updateApp.AuthId, previousAppDetails.AuthId);
            #endregion
            #region AuthSecret Assert
            var encodedSecret = _stringConstant.RandomClientSecret.Sha256();
            Assert.Equal(previousAppDetails.AuthSecret, encodedSecret);
            encodedSecret = _stringConstant.AccessToken.Sha256();
            Assert.Equal(updateApp.AuthSecret, encodedSecret);
            Assert.NotEqual(updateApp.AuthSecret, previousAppDetails.AuthSecret);
            #endregion
            #region CallbackUrl Assert
            Assert.Equal(previousAppDetails.CallbackUrl, _stringConstant.PromactErpUrlForTest);
            Assert.Equal(updateApp.CallbackUrl, _stringConstant.CallbackUrl);
            Assert.NotEqual(updateApp.CallbackUrl, previousAppDetails.CallbackUrl);
            #endregion
            #region LogoutUrl Assert
            Assert.Equal(previousAppDetails.LogoutUrl, _stringConstant.PromactErpUrlForTest);
            Assert.Equal(updateApp.LogoutUrl, _stringConstant.CallbackUrl);
            Assert.NotEqual(updateApp.LogoutUrl, previousAppDetails.LogoutUrl);
            #endregion
            #region Scope Assert
            Assert.Equal(previousAppDetails.Scopes.Count, 4);
            Assert.Equal(updateApp.Scopes.Count, 5);
            Assert.NotEqual(updateApp.Scopes.Count, previousAppDetails.Scopes.Count);
            #endregion
            #region Name Assert
            Assert.Equal(previousAppDetails.Name, _stringConstant.Name);
            Assert.Equal(updateApp.Name, _stringConstant.ProjectName);
            Assert.NotEqual(updateApp.Name, previousAppDetails.Name);
            #endregion
        }
        #endregion

        #region "Private Method(s)"

        /// <summary>
        /// This method used for get valid object with data.
        /// </summary>
        /// <returns></returns>
        private ConsumerApps GetConsumerApp()
        {
            ConsumerApps comnsumerApp = new ConsumerApps();
            comnsumerApp.CallbackUrl = _stringConstant.PromactErpUrlForTest;
            comnsumerApp.AuthId = _stringConstant.RandomClientId;
            comnsumerApp.AuthSecret = _stringConstant.RandomClientSecret;
            comnsumerApp.LogoutUrl = _stringConstant.PromactErpUrlForTest;
            comnsumerApp.Name = _stringConstant.Name;
            comnsumerApp.Scopes = new List<AllowedScope>()
            {
                AllowedScope.email,
                AllowedScope.openid,
                AllowedScope.profile,
                AllowedScope.slack_user_id,
            };
            return comnsumerApp;
        }
        #endregion
    }
}
