using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.OAuthRepository
{
    public interface IOAuthRepository
    {
        OAuth OAuthClientChecking(string email, string clientId);
        OAuthApplication GetAppDetailsFromClient(string redirectUrl, string refreshToken);
    }
}
