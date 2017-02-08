using IdentityServer4;
using IdentityServer4.Models;
using Promact.Oauth.Server.Constants;
using System.Collections.Generic;

namespace Promact.Oauth.Server.Configuration.DefaultAPIResource
{
    public class DefaultApiResources : IDefaultApiResources
    {
        #region Private Variable
        private readonly IStringConstant _stringConstant;
        #endregion

        #region Constructor
        public DefaultApiResources(IStringConstant stringConstant)
        {
            _stringConstant = stringConstant;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Method used to return a API resource with defined value
        /// </summary>
        /// <returns>List of ApiResource</returns>
        public IEnumerable<ApiResource> GetDefaultApiResource()
        {
            return new List<ApiResource>()
            {
                new ApiResource()
                {
                    // API name of API Resource
                    Name = _stringConstant.APIResourceName,
                    // Display name of API Resource
                    DisplayName = _stringConstant.APIResourceDisplayName,
                    // Adding Secret of API Resource
                    ApiSecrets = new List<Secret>()
                    {
                        new Secret(_stringConstant.APIResourceApiSecrets.Sha256())
                    },
                    // Adding Scopes of API Resource
                    Scopes = new List<Scope>()
                    {
                        new Scope(IdentityServerConstants.StandardScopes.Email),
                        new Scope(IdentityServerConstants.StandardScopes.OpenId),
                        new Scope(IdentityServerConstants.StandardScopes.Profile),
                        new Scope(_stringConstant.APIResourceUserReadScope),
                        new Scope(_stringConstant.APIResourceProjectReadScope)
                    }
                }
            };
        }
        #endregion
    }
}
