using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Data_Repository;
using Xunit;
using System;
using Promact.Oauth.Server.Models;
using System.Collections.Generic;
using Promact.Oauth.Server.Repository;
using System.Globalization;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Tests
{

    public class ProjectTests:BaseProvider
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDataRepository<Project> _dataRepository;
        private readonly IDataRepository<ProjectUser> _dataRepositoryProjectUser;
        private readonly IDataRepository<ApplicationUser> _dataRepositoryUser;
        private readonly IUserRepository _userRepository;
        

        public ProjectTests():base()
        {
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
            _dataRepository = serviceProvider.GetService<IDataRepository<Project>>();
            _dataRepositoryProjectUser = serviceProvider.GetService<IDataRepository<ProjectUser>>();
            _dataRepositoryUser = serviceProvider.GetService<IDataRepository<ApplicationUser>>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
        }

        ProjectAc projectac = new ProjectAc()
        {
            Name = "slack",
            SlackChannelName = "SlackChannelName",
            IsActive = true,
            TeamLeader = new UserAc { FirstName = "Roshni" },
            TeamLeaderId = "1",
            CreatedBy = "Roshni"

        };

        ProjectUser projectUser = new ProjectUser()
        {
            ProjectId = 1,
            Project = new Project { Name = "Slack" },
            UserId = "1",
            User = new ApplicationUser { FirstName = "Roshni" }
        };
        /// <summary>
        /// This test case to add a new project
        /// </summary>

        [Fact, Trait("Category", "Required")]
        public void AddProject()
        {
            Task<int> id = _projectRepository.AddProject(projectac, "Ronak");
            var project = _dataRepository.FirstOrDefault(x => x.Id== id.Result);
            Assert.NotNull(project);
        }
        /// <summary>
        /// This test case for the add user and project in userproject table
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void AddUserProject()
        {
           
            _projectRepository.AddUserProject(projectUser);
            var ProjectUser = _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(ProjectUser);
        }
        /// <summary>
        /// This test case for gets project By Id
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetById()
        {
            UserAc user = new UserAc()
            {
                
                FirstName = "Admin1",
                LastName="test1",
                Email= "test131@yahoo.com"
            };
            var TId=_userRepository.AddUser(user, "Ronak");
            projectac.TeamLeaderId = TId;
            Task<int> id =  _projectRepository.AddProject(projectac, "Ronak");
            _projectRepository.AddUserProject(projectUser);
            Task<ProjectAc> Pc = _projectRepository.GetById(id.Result);
            Assert.NotNull(Pc);
        }

        /// <summary>
        /// This test case edit project 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void EditProject()
        {
            UserAc user = new UserAc()
            {Id = "1",FirstName = "Roshni"};
            UserAc userSecound = new UserAc()
            {Id = "2",FirstName = "Ronit"};
            UserAc userThird = new UserAc()
            {Id = "3",FirstName = "Raj"};
            _userRepository.AddUser(user,"Ronak");
            _userRepository.AddUser(userSecound, "Ronak");
            _userRepository.AddUser(userThird, "Ronak");
            Task<int> id = _projectRepository.AddProject(projectac, "Ronak");
            _projectRepository.AddUserProject(projectUser);
            List<UserAc> userlist = new List<UserAc>();
            userlist.Add(new UserAc { Id = "2", FirstName = "Ronit" });
            userlist.Add(new UserAc { Id = "3", FirstName = "Raj" });
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = id.Result,
                Name = "slackEdit",
                SlackChannelName = "SlackChannelNameEdit",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Roshni" },
                TeamLeaderId = "1",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            _projectRepository.EditProject(projectacSecound, "Ronak");
            var project = _dataRepository.Fetch(x => x.Id == 1);
            _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(project);
        }
        /// <summary>
        /// This test case for the check duplicate project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void checkDuplicateNegative()
        {
            _projectRepository.AddProject(projectac, "Ronak");
           var project =_projectRepository.checkDuplicate(projectac);
            Assert.Null(project.Name);
        }
        /// <summary>
        /// This test case for the check duplicate project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void checkDuplicatePositive()
        {
            
            _projectRepository.AddProject(projectac, "Ronak");
            List<UserAc> userlist = new List<UserAc>();
            userlist.Add(new UserAc { Id = "2", FirstName = "Ronit" });
            userlist.Add(new UserAc { Id = "3", FirstName = "Raj" });
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = 2,
                Name = "slackEdit",
                SlackChannelName = "SlackChannelNameEdit",
                IsActive = true,
                TeamLeader = new UserAc { FirstName = "Roshni" },
                TeamLeaderId = "1",
                CreatedBy = "Roshni",
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            _projectRepository.AddProject(projectacSecound, "Ronak");
            var project = _projectRepository.checkDuplicate(projectacSecound);
            Assert.Null(project.Name);
        }
        /// <summary>
        /// This test case for the get all projects
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetAllProject()
        {
            UserAc user = new UserAc()
            {

                FirstName = "Admin",
                LastName = "test",
                Email = "test13@yahoo.com"
            };
            var TId = _userRepository.AddUser(user, "Ronak");
            _projectRepository.AddProject(projectac, "Ronak");
            _projectRepository.AddUserProject(projectUser);
            Task<IEnumerable<ProjectAc>> p = _projectRepository.GetAllProjects();
            Assert.NotNull(p);
        }



    }
}