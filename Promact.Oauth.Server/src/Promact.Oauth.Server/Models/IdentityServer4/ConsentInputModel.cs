using System.Collections.Generic;

namespace Promact.Oauth.Server.Models.IdentityServer4
{
    public class ConsentInputModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public bool RememberConsent { get; set; }
        public string ReturnUrl { get; set; }
    }
}
