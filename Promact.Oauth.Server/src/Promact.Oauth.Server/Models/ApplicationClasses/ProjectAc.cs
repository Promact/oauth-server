using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class ProjectAc
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(25)]
        [JsonProperty("SlackChannelName")]
        public string SlackChannelName { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("TeamLeaderId")]
        public string TeamLeaderId { get; set; }

        [JsonProperty("CreatedBy")]
        public string CreatedBy { get; set;}

        [JsonProperty("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("UpdatedBy")]
        public string UpdatedBy { get; set;}

        [JsonProperty("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty("TeamLeader")]
        public UserAc TeamLeader { get; set; }

        [JsonProperty("ApplicationUsers")]
        public List<UserAc> ApplicationUsers { get; set; }
       

    }
}
