using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;

namespace Promact.Oauth.Server.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<ConsumerAppsAc, ConsumerApps>();
            CreateMap<ProjectAc, Project>().ReverseMap();
            CreateMap<IdentityRole, RolesAc>();
            CreateMap<UserAc, ApplicationUser>().ReverseMap();
        }
    }
}
