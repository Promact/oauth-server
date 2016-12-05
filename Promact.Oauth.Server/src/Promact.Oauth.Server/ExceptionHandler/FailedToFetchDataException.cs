using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.ExceptionHandler
{
    public class FailedToFetchDataException : Exception
    {
        /// <summary>
        /// Initializes Exception For Failed To Fetch The Data
        /// </summary>
        public FailedToFetchDataException() : base()
        {

        }
    }
}
