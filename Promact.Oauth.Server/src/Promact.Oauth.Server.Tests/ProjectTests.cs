using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Xunit;

namespace Promact.Oauth.Server.Tests
{

    public class ProjectTests:BaseProvider
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectTests():base()
        {
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
        }

        [Fact]
        public void GetAllProjects()
        {
            var a = _projectRepository.AddProject(new ProjectAc
            {
                Name = "slack",
               SlackChannelName=null,
            }, "Ronak");
            
            //string a = "tari bai no piko";
        }
    }
}