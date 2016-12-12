using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.HttpClientRepository
{
    public interface IHttpClientRepository
    {
        Task<string> GetAsync(string baseUrl, string contentUrl);
    }
}
