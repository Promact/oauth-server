using AutoMapper;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;

namespace Promact.Oauth.Server.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        #pragma warning disable CS0672 // Member overrides obsolete member
        protected override void Configure()
        #pragma warning restore CS0672 // Member overrides obsolete member
        {
            CreateMap<ConsumerAppsAc, ConsumerApps>();
            CreateMap<ProjectAc, Project>();
            CreateMap<Project, ProjectAc>();
            CreateMap<UserAc, ApplicationUser>().ReverseMap();
        }
    }
}
