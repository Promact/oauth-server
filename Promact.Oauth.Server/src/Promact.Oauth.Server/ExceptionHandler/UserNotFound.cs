using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.ExceptionHandler
{
    public class UserNotFound : Exception
    {
        /// <summary>
        /// Initializes Exception For User Not Found
        /// </summary>
        public UserNotFound() : base()
        {

        }
    }
}
