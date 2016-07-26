using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}