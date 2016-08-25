using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Promact.Oauth.Server.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public string LastName { get; set; }

        [Required]
        public bool IsActive { get; set; }


        public double NumberOfCasualLeave { get; set; }

        public double NumberOfSickLeave { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime JoiningDate {get;set;}

        [Required]
        [StringLength(255)]
        public string SlackUserName { get; set; }

        [NotMapped]
        public virtual ICollection<Project> Projects { get; set; }


        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDateTime { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDateTime { get; set; }
    }
}