using IdentityServer4.EntityFramework.Entities;
using Promact.Oauth.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public interface IConsumerAppRepository 
    {
        /// <summary>
        /// This method used for added consumer app and return primary key. -An
        /// </summary>
        /// <param name="consumerApp">App details as object</param>
        /// <returns>App details after saving changes as object</returns>
        Task<IdentityServer4.Models.Client> AddConsumerAppsAsync(ConsumerApps aaps);


        /// <summary>
        /// This method used for get list of apps. -An
        /// </summary>
        /// <returns>list of App</returns>
        Task<List<ConsumerApps>> GetListOfConsumerAppsAsync();

        /// <summary>
        /// This method used for get apps detail by client id. 
        /// </summary>
        /// <param name="clientId">App's clientId</param>
        /// <returns>App details</returns>
        Task<ConsumerApps> GetAppDetailsByClientIdAsync(string clientId);

        /// <summary>
        /// This method used for update consumer app and return primary key. -An
        /// </summary>
        /// <param name="consumerApps">App details as object</param>
        /// <returns>updated app details</returns>
        Task<Client> UpdateConsumerAppsAsync(ConsumerApps consumerApp);

        /// <summary>
        /// This method used for get random number For AuthId and Auth Secreate. -An
        /// </summary>
        /// <param name="isAuthId">isAuthId = true (get random number for auth id.) and 
        /// isAuthId = false (get random number for auth secreate)</param>
        /// <returns>Random generated code</returns>
        string GetRandomNumber(bool isAuthId);
    }
}
