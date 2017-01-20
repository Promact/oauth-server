using IdentityServer4.Models;
using System.Collections.Generic;

namespace Promact.Oauth.Server.Configuration.DefaultIdentityResource
{
    public class DefaultIdentityResources : IDefaultIdentityResources
    {
        #region Public Method
        /// <summary>
        /// Method to get list of Identity Resource with defined value
        /// </summary>
        /// <returns>List of IdentityResource</returns>
        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                // Defined openid and profile as Identity Resource
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
        #endregion
    }
}
