using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.ExceptionHandler
{
    public class ConsumerAppNameIsAlreadyExists : Exception
    {  
        /// <summary>
        /// Initializes Exception For Consumer App Name is already exists in database.
        /// 
        /// </summary>
        public ConsumerAppNameIsAlreadyExists() : base()
        {

        }
    }
}
