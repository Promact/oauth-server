using System;
using System.Collections.Generic;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class ProjectAc
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string SlackChannelName { get; set; }
        public bool IsActive { get; set; }
        public string TeamLeaderId { get; set; }
        public string CreatedBy { get; set;}
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set;}
        public string UpdatedDate { get; set; }
        public UserAc TeamLeader { get; set; }
        public List<UserAc> ApplicationUsers { get; set; }
        //public bool Status { get; set; }

    }
}
