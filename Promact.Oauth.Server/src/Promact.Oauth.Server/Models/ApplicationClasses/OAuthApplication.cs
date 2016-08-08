using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class OAuthApplication
    {
        /// <summary>
        /// Client Id of App
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client Secret of App
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Refresh Token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Redirect Url of App
        /// </summary>
        public string ReturnUrl { get; set; }

    }
}
