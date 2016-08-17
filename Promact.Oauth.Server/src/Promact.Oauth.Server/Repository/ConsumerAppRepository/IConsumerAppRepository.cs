using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
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
        Task<int> AddConsumerApps(ConsumerAppsAc aaps);


        /// <summary>
        /// This method used forget list of apps. -An
        /// </summary>
        /// <returns></returns>
        Task<List<ConsumerApps>> GetListOfApps();

        /// <summary>
        /// This method used for get apps detail by client id. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<ConsumerApps> GetAppDetails(string clientId);

        /// <summary>
        /// This method used fro get apps object by id. -An
        /// </summary>
        /// <param name="id">pass apps object primarykey</param>
        /// <returns></returns>
        Task<ConsumerApps> GetConsumerAppsById(int id);

        /// <summary>
        /// This method used for update consumer apps and return primary key. -An
        /// </summary>
        /// <param name="apps"></param>
        /// <returns></returns>
        Task<int> UpdateConsumerApps(ConsumerApps consumerApps);
    }
}
