using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models
{
    public class OAuth
    {
        public int Id { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string userEmail { get; set; }
    }
}
