using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public interface IConsumerAppRepository 
    {
        /// <summary>
        /// This method used for added consumer app and return consumerApps Id. -An
        /// </summary>
        /// <param name="consumerApps">consumerApp object</param>
        /// <returns>consumerApp Id</returns>
        Task<int> AddConsumerAppsAsync(ConsumerAppsAc aaps);


        /// <summary>
        /// This method used for get list of apps. -An
        /// </summary>
        /// <returns>list of ConsumerApps</returns>
        Task<List<ConsumerApps>> GetListOfConsumerAppsAsync();

        /// <summary>
        /// This method used for get apps detail by client id. 
        /// </summary>
        /// <param name="clientId">passed client Id</param>
        /// <returns>Consumer App object</returns>
        Task<ConsumerApps> GetAppDetailsAsync(string clientId);

        /// <summary>
        /// This method used for get consumer app object by id. -An
        /// </summary>
        /// <param name="id">ConsumerApp Id</param>
        /// <returns>ConsumerApp Object</returns>
        Task<ConsumerApps> GetConsumerAppByIdAsync(int id);

        /// <summary>
        /// This method used for update consumer app and return consumerApp Id. -An
        /// </summary>
        /// <param name="consumerApps">passed consumerApp Object</param>
        /// <returns>consumerApp Id</returns>
        Task<int> UpdateConsumerAppsAsync(ConsumerApps consumerApps);
    }
}
