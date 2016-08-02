using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class UserAC
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Last { get; set; }
        public bool Status { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NormalizedUserName { get; set; }
    }
}
