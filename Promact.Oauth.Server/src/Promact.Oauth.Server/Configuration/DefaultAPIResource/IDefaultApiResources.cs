using IdentityServer4.Models;
using System.Collections.Generic;

namespace Promact.Oauth.Server.Configuration.DefaultAPIResource
{
    public interface IDefaultApiResources
    {
        IEnumerable<ApiResource> GetDefaultApiResource();
    }
}
