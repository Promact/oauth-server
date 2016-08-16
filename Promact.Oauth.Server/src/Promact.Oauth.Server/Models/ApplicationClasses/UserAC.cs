using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class UserAc
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [Required]
        [EmailAddress]
        [JsonProperty("Email")]
        public string Email { get; set; }

        [StringLength(255)]
        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }
    }
}
