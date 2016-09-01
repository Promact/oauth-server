using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models
{
    public class OAuth
    {
        /// <summary>
        /// Primary Key Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Refresh Token used to send to get client's app secret
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// AccessToken send after successfully Authorise
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Email Address of User
        /// </summary>
        public string userEmail { get; set; }

        /// <summary>
        /// Client Id of app
        /// </summary>
        public string ClientId { get; set; }
    }
}
