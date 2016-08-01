using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models.ApplicationClass
{
    public class UserModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public bool Status { get; set; }

        public string Email { get; set; }
        
        public string Designation { get; set; }
    }
}
