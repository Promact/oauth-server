using Promact.Oauth.Server.Models;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.OAuthRepository
{
    public interface IOAuthRepository
    {
        /// <summary>
        /// Fetches the details of Client using access token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns>true if value is not null otherwise false</returns>
        bool GetDetailsClientByAccessToken(string accessToken);


        /// <summary>
        /// Returns appropriate Url for the user to be redirected to
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="clientId"></param>
        /// <param name="callBackUrl"></param>
        /// <returns>returUrl</returns>
        Task<string> UserAlreadyLogin(string userName, string clientId, string callBackUrl);


        /// <summary>
        /// Signs in the user and redirects to the appropraite url.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>if not successfully signed-in then return empty string if successfully signed-in and credentials match then return redirect url</returns>
        Task<string> UserNotAlreadyLogin(OAuthLogin model);
    }
}
