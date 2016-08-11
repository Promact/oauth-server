using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class ProjectAc
    {
        
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(25)]
        public string SlackChannelName { get; set; }

        public bool IsActive { get; set; }
        public string TeamLeaderId { get; set; }
        public string CreatedBy { get; set;}
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set;}
        public string UpdatedDate { get; set; }
        public UserAc TeamLeader { get; set; }
        public List<UserAc> ApplicationUsers { get; set; }
       

    }
}
