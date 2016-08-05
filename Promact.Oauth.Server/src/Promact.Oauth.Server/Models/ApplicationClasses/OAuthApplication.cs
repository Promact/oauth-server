using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class OAuthApplication
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
        public string ReturnUrl { get; set; }

    }
}
