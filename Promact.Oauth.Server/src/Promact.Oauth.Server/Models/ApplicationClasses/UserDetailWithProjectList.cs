using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class UserDetailWithProjectList
    {
        public UserAc UserAc { get; set; }

        public List<ProjectAc> ListOfProject { get; set; }
    }
}
