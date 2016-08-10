using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.OAuthRepository
{
    public class OAuthRepository:IOAuthRepository
    {
        private readonly IDataRepository<OAuth> _oAuthDataRepository;
        public OAuthRepository(IDataRepository<OAuth> oAuthDataRepository)
        {
            _oAuthDataRepository = oAuthDataRepository;
        }

        /// <summary>
        /// Method to add OAuth table
        /// </summary>
        /// <param name="model"></param>
        public void Add(OAuth model)
        {
            _oAuthDataRepository.Add(model);
            _oAuthDataRepository.Save();
        }

        /// <summary>
        /// Method to update OAuth table
        /// </summary>
        /// <param name="model"></param>
        public void Update(OAuth model)
        {
            _oAuthDataRepository.Update(model);
            _oAuthDataRepository.Save();
        }

        /// <summary>
        /// To get details of a OAuth Access for an email and corresponding to app
        /// </summary>
        /// <param name="email"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public OAuth GetDetails(string email,string clientId)
        {
            var oAuth = _oAuthDataRepository.FirstOrDefault(x => x.userEmail == email && x.ClientId == clientId);
            return oAuth;
        }
    }
}
