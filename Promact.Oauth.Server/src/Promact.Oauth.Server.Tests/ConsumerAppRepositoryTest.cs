using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Models;
using System;
using Promact.Oauth.Server.Data_Repository;
using Xunit;
using System.Collections.Generic;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System.Threading.Tasks;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data;

namespace Promact.Oauth.Server.Tests
{
    public class ConsumerAppRepositoryTest : BaseProvider
    {
        private readonly IConsumerAppRepository _consumerAppRespository;
        private readonly IDataRepository<ConsumerApps> _consumerAppsContext;
        private readonly PromactOauthDbContext _db;

        public ConsumerAppRepositoryTest() : base()
        {
            _consumerAppRespository = serviceProvider.GetService<IConsumerAppRepository>();
            _consumerAppsContext = serviceProvider.GetService<IDataRepository<ConsumerApps>>();
            _db = serviceProvider.GetService<PromactOauthDbContext>();
        }

        #region Test Case

        /// <summary>
        /// This test case for add Consumer Apps. -An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void AddConsumerApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = StringConstant.ConsumerAppNameDemo;
            int id = _consumerAppRespository.AddConsumerApps(consumerApp).Result;
            var consumerApps = _consumerAppsContext.FirstOrDefault(x => x.Id == id);
            Assert.NotNull(consumerApps);
        }

        /// <summary>
        /// This test case used for check consumer name is unique or not. -An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void ConsumerAppNameUnique()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = StringConstant.ConsumerAppNameDemo1;
            _consumerAppRespository.AddConsumerApps(consumerApp);
            int newId = _consumerAppRespository.AddConsumerApps(consumerApp).Result;
            Assert.Equal(0, newId);
        }

        /// <summary>
        /// This test case used for check app details fetch by valid client id.-An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetAppDetailsByClientId()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = StringConstant.ConsumerAppNameDemo2;
            int id = _consumerAppRespository.AddConsumerApps(consumerApp).Result;
            var consumerApps = _consumerAppsContext.FirstOrDefault(x => x.Id == id);
            var getApplication = _consumerAppRespository.GetAppDetails(consumerApps.AuthId);
            Assert.NotNull(getApplication);
        }

        /// <summary>
        /// This test case used for check app details not fetch by invalid client id.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void ApplicationDetailsFetchOnlyValidClientId()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = StringConstant.ConsumerAppNameDemo3;
            _consumerAppRespository.AddConsumerApps(consumerApp);
            ConsumerApps getApplication = _consumerAppRespository.GetAppDetails("ABEDNGdeMR1234568F").Result;
            Assert.Null(getApplication);
        }


        /// <summary>
        /// This test case used for check consumer app details not fetch by primary key id.-An
        /// </summary>
        /// 
        [Fact, Trait("Category", "Required")]
        public void GetConsumerAppsById()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = StringConstant.ConsumerAppNameDemo4;
            int id = _consumerAppRespository.AddConsumerApps(consumerApp).Result;
            Task<ConsumerApps> getApplication = _consumerAppRespository.GetConsumerAppsById(id);
            Assert.NotNull(getApplication.Result);
        }


        /// <summary>
        /// This test case used for check consumer app details not fetch by invalid primary key id. -An
        /// </summary>
        ///  
        [Fact, Trait("Category", "Required")]
        public void ConsumerAppGetByWrongId()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = StringConstant.ConsumerAppNameDemo5;
            _consumerAppRespository.AddConsumerApps(consumerApp);
            ConsumerApps getApplication = _consumerAppRespository.GetConsumerAppsById(23213).Result;
            Assert.Null(getApplication);
        }


        /// <summary>
        /// This test case used for check get list of apps. -An 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetListOfApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = StringConstant.ConsumerAppNameDemo6;
            _consumerAppRespository.AddConsumerApps(consumerApp);
            List<ConsumerApps> listOfApps = _consumerAppRespository.GetListOfApps().Result;
            Assert.NotEmpty(listOfApps);
        }

        /// <summary>
        /// This test case used for update consumer app. -An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void UpdateConsumerApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = StringConstant.ConsumerAppNameDemo7;
            int id = _consumerAppRespository.AddConsumerApps(consumerApp).Result;
            ConsumerApps consumerApps = _consumerAppRespository.GetConsumerAppsById(id).Result;
            consumerApps.Description = StringConstant.ConsumerDescription;
            consumerApps.UpdatedDateTime = DateTime.Now;
            consumerApps.UpdatedBy = StringConstant.UpdateBy;
            int newId = _consumerAppRespository.UpdateConsumerApps(consumerApps).Result;
            Assert.NotEqual(0, newId);
        }

        /// <summary>
        /// This test case used for check consumer name is unique or not when update consumer app. -An
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void CheckConsumerAppNameUnique()
        {
            var consumer = GetConsumerApp();
            consumer.Name = StringConstant.TwitterName;
            _consumerAppRespository.AddConsumerApps(consumer);
            var consumerNew = GetConsumerApp();
            consumerNew.Name = StringConstant.FaceBookName;
            int idOfConsumer = _consumerAppRespository.AddConsumerApps(consumerNew).Result;
            ConsumerApps oldConsumerApp = _consumerAppRespository.GetConsumerAppsById(idOfConsumer).Result;
            oldConsumerApp.Name = StringConstant.TwitterName;
            int newId = _consumerAppRespository.UpdateConsumerApps(oldConsumerApp).Result;
            Assert.Equal(0, newId);
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
            comnsumerApp.CallbackUrl = StringConstant.CallbackUrl;
            comnsumerApp.CreatedBy = StringConstant.CreatedBy;
            comnsumerApp.Description = StringConstant.ConsumerDescription;
            return comnsumerApp;
        }
        #endregion
        ~ConsumerAppRepositoryTest()
        {
            _db.Dispose();
        }
    }
}
