using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public class ConsumerAppRepository : IConsumerAppReposiotry
    {
        #region "Private Variable(s)"

        private readonly IDataRepository<Apps> _appsDataRepository;

        #endregion


        #region "Constructor"
        public ConsumerAppRepository(IDataRepository<Apps> appsDataRepository)
        {
            _appsDataRepository = appsDataRepository;
        }

        #endregion

        #region "Public Method(s)"




        #endregion


    }
}
