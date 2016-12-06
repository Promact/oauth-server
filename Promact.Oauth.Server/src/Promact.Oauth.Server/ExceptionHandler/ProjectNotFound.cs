using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.ExceptionHandler
{
    public class ProjectNotFound: Exception 
    {
         /// <summary>
        /// Initializes Exception For Project Not Found
        /// </summary>
        public ProjectNotFound() : base()
        {

    }
}
}
