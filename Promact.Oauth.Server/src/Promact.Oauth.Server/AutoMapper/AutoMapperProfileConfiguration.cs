using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;

namespace Promact.Oauth.Server.AutoMapper
{
    // public class AutoMapperProfileConfiguration : Profile
    public class OrganizationProfile : Profile
    {
#pragma warning disable CS0672 // Member overrides obsolete member
        public OrganizationProfile()
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            CreateMap<ConsumerAppsAc, ConsumerApps>();
            CreateMap<ProjectAc, Project>().ReverseMap();
            CreateMap<IdentityRole, RolesAc>();
            CreateMap<UserAc, ApplicationUser>().ReverseMap();
        }
    }
}
