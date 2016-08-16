using AutoMapper;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {

        protected override void Configure()
        {
            CreateMap<ConsumerAppsAc, ConsumerApps>();
        }
    }
}
