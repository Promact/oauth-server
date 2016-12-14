using Promact.Oauth.Server.Constants;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{
    public class HttpClientService : IHttpClientService
    {
        private HttpClient _client;
        private readonly IStringConstant _stringConstant;
        public HttpClientService(IStringConstant stringConstant)
        {
            _stringConstant = stringConstant;
        }

        /// <summary>
        /// Method to use System.Net.Http.HttpClient's GetAsync method
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="contentUrl"></param>
        /// <returns>response</returns>
        public async Task<string> GetAsync(string baseUrl, string contentUrl)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            var response = await _client.GetAsync(contentUrl);
            _client.Dispose();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                return responseContent;
            }
            else
                return _stringConstant.EmptyString;
        }
    }
}
