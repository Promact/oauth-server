using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.HttpClientRepository
{
    public interface IHttpClientRepository
    {
        Task<HttpResponseMessage> GetAsync(string baseUrl, string contentUrl);
    }
}
