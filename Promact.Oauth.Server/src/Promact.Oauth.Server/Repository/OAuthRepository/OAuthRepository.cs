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
        public void Add(OAuth model)
        {
            _oAuthDataRepository.Add(model);
            _oAuthDataRepository.Save();
        }
        public void Update(OAuth model)
        {
            _oAuthDataRepository.Update(model);
            _oAuthDataRepository.Save();
        }
        public OAuth GetDetails(string email)
        {
            var oAuth = _oAuthDataRepository.FirstOrDefault(x => x.userEmail == email);
            return oAuth;
        }
    }
}
