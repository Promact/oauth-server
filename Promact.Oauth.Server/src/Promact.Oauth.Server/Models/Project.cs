using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Promact.Oauth.Server.Models
{
    public class Project : OAuthBase
    {
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(25)]
        public string SlackChannelName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string TeamLeaderId { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        
    }
}