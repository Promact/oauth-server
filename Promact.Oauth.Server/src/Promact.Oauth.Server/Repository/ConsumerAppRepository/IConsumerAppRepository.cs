using IdentityServer4.EntityFramework.Entities;
using Promact.Oauth.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public interface IConsumerAppRepository 
    {
        /// <summary>
        /// This method used for added Consumer apps and return primary key. -An
        /// </summary>
        /// <param name="aaps"></param>
        /// <returns></returns>
        Task<IdentityServer4.Models.Client> AddConsumerAppsAsync(ConsumerApps aaps);


        /// <summary>
        /// This method used for get list of consumer apps. -An
        /// </summary>
        /// <returns></returns>
        Task<List<ConsumerApps>> GetListOfConsumerAppsAsync();

        /// <summary>
        /// This method used for get apps detail by client id. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<ConsumerApps> GetAppDetailsByClientIdAsync(string clientId);

        /// <summary>
        /// This method used for update consumer app and return primary key. -An
        /// </summary>
        /// <param name="apps"></param>
        /// <returns></returns>
        Task<Client> UpdateConsumerAppsAsync(ConsumerApps consumerApp);
    }
}
