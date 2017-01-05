using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Models;
using System;
using Promact.Oauth.Server.Data_Repository;
using Xunit;
using System.Collections.Generic;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System.Threading.Tasks;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.StringLliterals;
using Microsoft.Extensions.Options;

namespace Promact.Oauth.Server.Tests
{
    public class ConsumerAppRepositoryTest : BaseProvider
    {
        private readonly IConsumerAppRepository _consumerAppRespository;
        private readonly IDataRepository<ConsumerApps> _consumerAppsContext;
        private readonly IStringConstant _stringConstant;
        private readonly IOptionsMonitor<StringLiterals> _stringLiterals;
        public ConsumerAppRepositoryTest() : base()
        {
            _consumerAppRespository = serviceProvider.GetService<IConsumerAppRepository>();
            _consumerAppsContext = serviceProvider.GetService<IDataRepository<ConsumerApps>>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
            _stringLiterals = serviceProvider.GetService<IOptionsMonitor<StringLiterals>>();
        }

        #region Test Case

        /// <summary>
        /// This test case for add Consumer Apps. -An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task AddConsumerApps()
        {
            
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = _stringConstant.ConsumerAppNameDemo;
            int id = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            var consumerApps = await _consumerAppsContext.FirstOrDefaultAsync(x => x.Id == id);
            Assert.NotNull(consumerApps);
        }

        /// <summary>
        /// This test case used for check consumer name is unique or not. -An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ConsumerAppNameUnique()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = _stringConstant.ConsumerAppNameDemo1;
            int id = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            Assert.Throws<AggregateException>(() => _consumerAppRespository.AddConsumerAppsAsync(consumerApp).Result);
        }

        /// <summary>
        /// This test case used for check app details fetch by valid client id.-An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetAppDetailsByClientId()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = _stringConstant.ConsumerAppNameDemo2;
            int id = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            var consumerApps = await _consumerAppsContext.FirstOrDefaultAsync(x => x.Id == id);
            var getApplication = await _consumerAppRespository.GetAppDetailsAsync(consumerApps.AuthId);
            Assert.NotNull(getApplication);
        }

        /// <summary>
        /// This test case used for check app details not fetch by invalid client id.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ApplicationDetailsFetchOnlyValidClientId()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = _stringConstant.ConsumerAppNameDemo3;
            int id = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            ConsumerApps getApplication = await _consumerAppRespository.GetAppDetailsAsync("ABEDNGdeMR1234568F");
            Assert.Null(getApplication);
        }


        /// <summary>
        /// This test case used for check consumer app details not fetch by primary key id.-An
        /// </summary>
        /// 
        [Fact, Trait("Category", "Required")]
        public async Task GetConsumerAppsById()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = _stringConstant.ConsumerAppNameDemo4;
            int id = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            Task<ConsumerApps> getApplication = _consumerAppRespository.GetConsumerAppByIdAsync(id);
            Assert.NotNull(getApplication.Result);
        }


        /// <summary>
        /// This test case used for check consumer app details not fetch by invalid primary key id. -An
        /// </summary>
        ///  
        [Fact, Trait("Category", "Required")]
        public async Task ConsumerAppGetByWrongId()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = _stringConstant.ConsumerAppNameDemo5;
            await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            Assert.Throws<AggregateException>(() => _consumerAppRespository.GetConsumerAppByIdAsync(23213).Result);
        }


        /// <summary>
        /// This test case used for check get list of apps. -An 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetListOfApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = _stringConstant.ConsumerAppNameDemo6;
            int id = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            List<ConsumerApps> listOfApps = await _consumerAppRespository.GetListOfConsumerAppsAsync();
            Assert.NotEmpty(listOfApps);
        }

        /// <summary>
        /// This test case used for update consumer app. -An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UpdateConsumerApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = _stringConstant.ConsumerAppNameDemo7;
            int id = await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            ConsumerApps consumerApps = await _consumerAppRespository.GetConsumerAppByIdAsync(id);
            consumerApps.Description = _stringConstant.ConsumerDescription;
            consumerApps.UpdatedDateTime = DateTime.Now;
            consumerApps.UpdatedBy = _stringConstant.UpdateBy;
            int newId = await _consumerAppRespository.UpdateConsumerAppsAsync(consumerApps);
            Assert.NotEqual(0, newId);
        }

        /// <summary>
        /// This test case used for check consumer name is unique or not when update consumer app. -An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckConsumerAppNameUnique()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = _stringConstant.TwitterName;
            await _consumerAppRespository.AddConsumerAppsAsync(consumerApp);
            ConsumerAppsAc newConsumerApp = GetConsumerApp();
            newConsumerApp.Name = _stringConstant.FaceBookName;
            int id = await _consumerAppRespository.AddConsumerAppsAsync(newConsumerApp);
            ConsumerApps oldConsumerApp = await _consumerAppRespository.GetConsumerAppByIdAsync(id);
            oldConsumerApp.Name = _stringConstant.TwitterName;
            Assert.Throws<AggregateException>(() => _consumerAppRespository.UpdateConsumerAppsAsync(oldConsumerApp).Result);
        }


        #endregion

        #region "Private Method(s)"

        /// <summary>
        /// This method used for get valid object with data. -An
        /// </summary>
        /// <returns></returns>
        private ConsumerAppsAc GetConsumerApp()
        {
            ConsumerAppsAc comnsumerApp = new ConsumerAppsAc();
            comnsumerApp.CallbackUrl = _stringConstant.CallbackUrl;
            comnsumerApp.CreatedBy = _stringConstant.CreatedBy;
            comnsumerApp.Description = _stringConstant.ConsumerDescription;
            return comnsumerApp;
        }
        #endregion
    }
}
