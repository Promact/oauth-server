using IdentityServer4.Models;
using System.Collections.Generic;

namespace Promact.Oauth.Server.Configuration.DefaultIdentityResource
{
    public interface IDefaultIdentityResources
    {
        IEnumerable<IdentityResource> GetIdentityResources();
    }
}
