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
        public virtual ICollection<UserAc> ApplicatioUsers { get; set; }
        //public virtual ApplicationUser User { get; set; }

        //public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        //public bool Status { get; set; }

    }
}
