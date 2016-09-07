using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models
{
    public class OAuthLogin
    {
        /// <summary>
        /// Login Email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Login Password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Hidden parameter used to send returnUrl from one Controller to another. Basically used after Login
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Hidden parameter used to send ClientId from one Controller to another. Basically used after Login
        /// </summary>
        public string ClientId { get; set; }
    }
}
