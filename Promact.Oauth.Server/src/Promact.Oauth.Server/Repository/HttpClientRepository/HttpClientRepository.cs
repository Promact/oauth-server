using Exceptionless.Json;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.HttpClientRepository
{
    public class HttpClientRepository: IHttpClientRepository
    {
        private HttpClient _client;
        public HttpClientRepository()
        {
            
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
            var responseContent = response.Content.ReadAsStringAsync().Result;
            return responseContent;
        }
    }
}
