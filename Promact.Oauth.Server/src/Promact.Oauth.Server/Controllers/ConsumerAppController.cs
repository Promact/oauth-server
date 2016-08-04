using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    public class ConsumerAppController : Controller
    {
        private readonly IConsumerAppReposiotry _iConsumerAppRepository;

        public ConsumerAppController(IConsumerAppReposiotry iConsumerAppRepository)
        {
            _iConsumerAppRepository = iConsumerAppRepository;
        }

        // POST api/values
        [HttpPost]
        [Route("addConsumer")]
        public Apps AddConsumerApp(ConsumerAppsAc consumerAppsAc)
        {
            try
            {

                return null;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}