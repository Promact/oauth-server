using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Models;
using System;
using Promact.Oauth.Server.Data_Repository;
using Xunit;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.Models.ApplicationClasses;

namespace Promact.Oauth.Server.Tests
{
    public class ConsumerAppRepositoryTest : BaseProvider
    {
        private readonly IConsumerAppRepository _consumerAppRespository;
        private readonly IDataRepository<ConsumerApps> _consumerAppsContext;

        public ConsumerAppRepositoryTest() : base()
        {
            _consumerAppRespository = serviceProvider.GetService<IConsumerAppRepository>();
            _consumerAppsContext = serviceProvider.GetService<IDataRepository<ConsumerApps>>();

        }


        #region Test Case

        /// <summary>
        /// This test case for add Comnsumer Apps. -An
        /// </summary>
        [Fact]
        public void AddConsumerApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = "Demo Name";
            int id = _consumerAppRespository.AddConsumerApps(consumerApp);
            var consumerApps = _consumerAppsContext.FirstOrDefault(x => x.Id == id);
            Assert.NotNull(consumerApps);
        }

        /// <summary>
        /// This test case used for,when add new consumer app check consumer name is quniqe duplication not allow. -An
        /// </summary>
        [Fact]
        public void ConsumerAppNameUnique()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = "ABCEDF";
            _consumerAppRespository.AddConsumerApps(consumerApp);
            int newId = _consumerAppRespository.AddConsumerApps(consumerApp);
            Assert.Equal(0, newId);
        }
        
        /// <summary>
        /// This test case used for check app details fetch by valid client id.-An
        /// </summary>
        [Fact]
        public void GetAppDetailsByClientId()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = "Demo2";
            int id = _consumerAppRespository.AddConsumerApps(consumerApp);
            var consumerApps = _consumerAppsContext.FirstOrDefault(x => x.Id == id);
            var getApplication = _consumerAppRespository.GetAppDetails(consumerApps.AuthId);
            Assert.NotNull(getApplication);
        }

        /// <summary>
        /// This test case used for check app details not fetch by Invalid client id.
        /// </summary>
        [Fact]
        public void ApplicationDetailsFetchOnlyValidClientId()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = "Demo3";
            _consumerAppRespository.AddConsumerApps(consumerApp);
            var getApplication = _consumerAppRespository.GetAppDetails("ABEDNGdeMR1234568F");
            Assert.Null(getApplication);
        }


        /// <summary>
        /// This test case used for check consumer app details not fetch by primary key id.-An
        /// </summary>
        /// 
        [Fact]
        public void GetConsumerAppsById()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = "Demo4";
            int id = _consumerAppRespository.AddConsumerApps(consumerApp);
            var getApplication = _consumerAppRespository.GetConsumerAppsById(id);
            Assert.NotNull(getApplication);
        }


        /// <summary>
        /// This test case used for check consumer app details not fetch by in valid primary key id. -An
        /// </summary>
        ///  
        [Fact]
        public void ConsumerAppGetByWrongId()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = "Demo5";
            _consumerAppRespository.AddConsumerApps(consumerApp);
            var getApplication = _consumerAppRespository.GetConsumerAppsById(23213);
            Assert.Null(getApplication);
        }
        

        /// <summary>
        /// This test case used for check get list of apps. -An 
        /// </summary>
        [Fact]
        public void GetListOfApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = "Demo6";
            _consumerAppRespository.AddConsumerApps(consumerApp);
            List<ConsumerApps> listOfApps = _consumerAppRespository.GetListOfApps();
            Assert.NotEmpty(listOfApps);
        }

        /// <summary>
        /// This test case used for update consumer app. -An
        /// </summary>
        [Fact]
        public void UpdateConsumerApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerApp();
            consumerApp.Name = "Demo for Update";
            int id = _consumerAppRespository.AddConsumerApps(consumerApp);
            var consumerApps = _consumerAppRespository.GetConsumerAppsById(id);
            consumerApps.Description = "XyzName";
            consumerApps.UpdatedDateTime = DateTime.Now;
            consumerApps.UpdatedBy = "Ankit";
            int newId = _consumerAppRespository.UpdateConsumerApps(consumerApps);
            Assert.NotEqual(0, newId);
        }

        /// <summary>
        /// This test case used for, When update consumer app check consumer name is unique duplication not allow. -An
        /// </summary>
        [Fact]
        public void CheckConsumerAppNameUnique()
        {
            var consumer = GetConsumerApp();
            consumer.Name = "Twitter";
            _consumerAppRespository.AddConsumerApps(consumer);
            var consumerNew = GetConsumerApp();
            consumerNew.Name = "Face Book";
            int idOfConsumer = _consumerAppRespository.AddConsumerApps(consumerNew);
            var oldConsumerApp = _consumerAppRespository.GetConsumerAppsById(idOfConsumer);
            oldConsumerApp.Name = "Twitter";
            int newId = _consumerAppRespository.UpdateConsumerApps(oldConsumerApp);
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
            comnsumerApp.CallbackUrl = "https://promact.slack.com/messages/@roshni/";
            comnsumerApp.CreatedBy = "Roshni";
            comnsumerApp.Description = "This App is Demo App, Please don't used";
            return comnsumerApp;
        }
        #endregion
    }
}
