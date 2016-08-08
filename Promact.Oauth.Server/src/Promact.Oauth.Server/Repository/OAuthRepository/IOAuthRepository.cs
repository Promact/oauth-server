using Promact.Oauth.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.OAuthRepository
{
    public interface IOAuthRepository
    {
        void Add(OAuth model);
        void Update(OAuth model);
        OAuth GetDetails(string email, string clientId);
    }
}
