using System;

namespace Promact.Oauth.Server.ExceptionHandler
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
