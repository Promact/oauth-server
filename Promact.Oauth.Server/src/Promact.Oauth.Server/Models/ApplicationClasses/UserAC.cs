using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class UserAc
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public bool IsActive { get; set; }
        //public string Status { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public string UserName { get; set; }
    }
}
