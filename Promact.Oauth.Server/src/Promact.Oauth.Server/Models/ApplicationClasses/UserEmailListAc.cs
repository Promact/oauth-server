using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class UserEmailListAc
    {
        public List<string> TeamLeader { get; set; }

        public List<string> TamMemeber { get; set; }

        public List<string> Management { get; set; }
    }
}
