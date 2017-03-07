using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Promact.Oauth.Server.Models
{
    public class Project : OAuthBase
    {
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        
        public string TeamLeaderId { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        
    }
}