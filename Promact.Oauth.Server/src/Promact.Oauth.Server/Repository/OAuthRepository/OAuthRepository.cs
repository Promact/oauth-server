using Newtonsoft.Json;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Promact.Oauth.Server.Repository.HttpClientRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.OAuthRepository
{
    public class OAuthRepository:IOAuthRepository
    {
        private readonly IDataRepository<OAuth> _oAuthDataRepository;
        private readonly IHttpClientRepository _httpClientRepository;
        private readonly IConsumerAppRepository _appRepository;

        public OAuthRepository(IDataRepository<OAuth> oAuthDataRepository, IHttpClientRepository httpClientRepository,IConsumerAppRepository appRepository)
        {
            _oAuthDataRepository = oAuthDataRepository;
            _httpClientRepository = httpClientRepository;
            _appRepository = appRepository;
        }

        /// <summary>
        /// Method to add OAuth table
        /// </summary>
        /// <param name="model"></param>
        private void Add(OAuth model)
        {
            _oAuthDataRepository.Add(model);
            _oAuthDataRepository.Save();
        }

        /// <summary>
        /// To get details of a OAuth Access for an email and corresponding to app
        /// </summary>
        /// <param name="email"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private OAuth GetDetails(string email,string clientId)
        {
            var oAuth = _oAuthDataRepository.FirstOrDefault(x => x.userEmail == email && x.ClientId == clientId);
            return oAuth;
        }

        public OAuth OAuthClientChecking(string email,string clientId)
        {
            //checking whether with this app email is register or not if  not new OAuth will be created.
            var oAuth = GetDetails(email, clientId);
            var app = _appRepository.GetAppDetails(clientId);
            if (oAuth == null && app!=null)
            {
                oAuth = new OAuth();
                oAuth.RefreshToken = Guid.NewGuid().ToString();
                oAuth.userEmail = email;
                oAuth.AccessToken = Guid.NewGuid().ToString();
                oAuth.ClientId = clientId;
                Add(oAuth);
            }
            return oAuth;
        }

        public async Task<OAuthApplication> GetAppDetailsFromClient(string redirectUrl, string refreshToken)
        {
            // Assigning Base Address with redirectUrl
            var requestUrl = string.Format("?refreshToken={0}", refreshToken);
            var response = await _httpClientRepository.GetAsync(redirectUrl, requestUrl);
            var responseResult = response.Content.ReadAsStringAsync().Result;
            // Transforming Json String to object type OAuthApplication
            var content = JsonConvert.DeserializeObject<OAuthApplication>(responseResult);
            return content;
        }
    }
}
