using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.StringLliterals
{
    public class Projects
    {
        public string title { get; set; }
        public string DateFormate { get; set; }
        public string TeamLeaderNotAssign { get; set; }
    }

    public class User
    {
        public string title { get; set; }
        public string UserDetialTemplateFolderPath { get; set; }
        public string UserPassword { get; set; }
    }
    public class ConsumerApp
    {
        public string ATOZaTOz0TO9 { get; set; }
        public string ATOZ0TO9 { get; set; }
    }
    public class CommanStringConstant
    {
        public string title { get; set; }
        public string RoleAdmin { get; set; }
        public string RoleTeamLeader { get; set; }
        public string RoleEmployee { get; set; }

    }

    public class StringLiterals
    {
        public Projects Projects { get; set; }
        public User User { get; set; }
        public ConsumerApp ConsumerApp { get; set; }
        public CommanStringConstant CommanStringConstant { get; set; }
    }
}
