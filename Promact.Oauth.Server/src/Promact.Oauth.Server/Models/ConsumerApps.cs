using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Oauth.Server.Models
{
    public class ConsumerApps 
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Url]
        public string CallbackUrl { get; set; }

        [Required]
        [StringLength(15)]
        public string AuthId { get; set; }

        [Required]
        [StringLength(30)]
        public string AuthSecret { get; set; }

        public List<AllowedScope> Scopes { get; set; }

        [Required]
        [StringLength(255)]
        [Url]
        public string LogoutUrl { get; set; }
    }
}