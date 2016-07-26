using Promact.Oauth.Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models
{
    public class Apps : OAuthBase
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Url]
        public string CallbackUrl { get; set; }

        [Required]
        public int AuthId { get; set; }

        [Required]
        public string AuthSecret { get; set; }

    }
}