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
        public void AddedConsumerApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerAppObject();
            consumerApp.Name = "Demo Name";
            int id = _consumerAppRespository.AddedConsumerApps(consumerApp);
            var consumerApps = _consumerAppsContext.FirstOrDefault(x => x.Id == id);
            Assert.NotNull(consumerApps);
        }

        /// <summary>
        /// This test case used for,when add new consumer app check consumer name is quniqe duplication not allow. -An
        /// </summary>
        [Fact]
        public void ConsumerAppNameUnique()
        {
            ConsumerAppsAc consumerApp = GetConsumerAppObject();
            consumerApp.Name = "ABCEDF";
            _consumerAppRespository.AddedConsumerApps(consumerApp);
            int newId = _consumerAppRespository.AddedConsumerApps(consumerApp);
            Assert.Equal(0, newId);
        }
        
        /// <summary>
        /// This test case used for check app details fetch by valid client id.-An
        /// </summary>
        [Fact]
        public void GetAppDetailsByClientId()
        {
            ConsumerAppsAc consumerApp = GetConsumerAppObject();
            consumerApp.Name = "Demo2";
            int id = _consumerAppRespository.AddedConsumerApps(consumerApp);
            var consumerApps = _consumerAppsContext.FirstOrDefault(x => x.Id == id);
            var getApplication = _consumerAppRespository.GetAppDetails(consumerApps.AuthId);
            Assert.NotNull(getApplication);
        }

        /// <summary>
        /// This test case used for check app details not fetch by Invalid client id.
        /// </summary>
        [Fact]
        public void ApplicationDetailsFetchOnlyValidclientId()
        {
            ConsumerAppsAc consumerApp = GetConsumerAppObject();
            consumerApp.Name = "Demo3";
            _consumerAppRespository.AddedConsumerApps(consumerApp);
            var getApplication = _consumerAppRespository.GetAppDetails("ABEDNGdeMR1234568F");
            Assert.Null(getApplication);
        }


        /// <summary>
        /// This test case used for check consumer app details not fetch by primary key id.-An
        /// </summary>
        /// 
        [Fact]
        public void GetAppsObjectById()
        {
            ConsumerAppsAc consumerApp = GetConsumerAppObject();
            consumerApp.Name = "Demo4";
            int id = _consumerAppRespository.AddedConsumerApps(consumerApp);
            var getApplication = _consumerAppRespository.GetAppsObjectById(id);
            Assert.NotNull(getApplication);
        }


        /// <summary>
        /// This test case used for check consumer app details not fetch by in valid primary key id. -An
        /// </summary>
        ///  
        [Fact]
        public void ConsumerAppObjectNotGetByWrongId()
        {
            ConsumerAppsAc consumerApp = GetConsumerAppObject();
            consumerApp.Name = "Demo5";
            _consumerAppRespository.AddedConsumerApps(consumerApp);
            var getApplication = _consumerAppRespository.GetAppsObjectById(23213);
            Assert.Null(getApplication);
        }
        

        /// <summary>
        /// This test case used for check get list of apps. -An 
        /// </summary>
        [Fact]
        public void GetListOfApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerAppObject();
            consumerApp.Name = "Demo6";
            _consumerAppRespository.AddedConsumerApps(consumerApp);
            List<ConsumerApps> listOfApps = _consumerAppRespository.GetListOfApps();
            Assert.NotEmpty(listOfApps);
        }

        /// <summary>
        /// This test case used for update consumer app. -An
        /// </summary>
        [Fact]
        public void updateConsumerApps()
        {
            ConsumerAppsAc consumerApp = GetConsumerAppObject();
            consumerApp.Name = "Demo for Update";
            int id = _consumerAppRespository.AddedConsumerApps(consumerApp);
            var consumerApps = _consumerAppRespository.GetAppsObjectById(id);
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
            var consumer = GetConsumerAppObject();
            consumer.Name = "Twitter";
            _consumerAppRespository.AddedConsumerApps(consumer);
            var consumerNew = GetConsumerAppObject();
            consumerNew.Name = "Face Book";
            int idOfConsumer = _consumerAppRespository.AddedConsumerApps(consumerNew);
            var oldConsumerApp = _consumerAppRespository.GetAppsObjectById(idOfConsumer);
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
        private ConsumerAppsAc GetConsumerAppObject()
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
