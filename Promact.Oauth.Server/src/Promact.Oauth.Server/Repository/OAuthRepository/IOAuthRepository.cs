using Promact.Oauth.Server.Models;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.OAuthRepository
{
    public interface IOAuthRepository
    {
        bool GetDetailsClientByAccessToken(string accessToken);
        Task<string> UserAlreadyLogin(string userName, string clientId, string callBackUrl);
        Task<string> UserNotAlreadyLogin(OAuthLogin model);
    }
}
