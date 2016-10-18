using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Exception_Handler
{
    public class SlackUserNotFound : Exception 
    {
        /// <summary>
        /// Initializes Exception For Slack User Not Found
        /// </summary>
        public SlackUserNotFound() : base()
        {

        }
    }
}
