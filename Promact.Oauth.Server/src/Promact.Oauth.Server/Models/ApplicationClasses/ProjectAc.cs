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
        public UserAc TeamLeader { get; set; }
        public List<UserAc> ApplicatioUsers { get; set; }
        //public bool Status { get; set; }

    }
}
