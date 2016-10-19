using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Data_Repository;
using Xunit;
using System;
using Promact.Oauth.Server.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data;

namespace Promact.Oauth.Server.Tests
{
    public class ProjectTests : BaseProvider
    {
        private readonly IProjectRepository _projectRepository;

        private readonly IDataRepository<Project> _dataRepository;
        private readonly IDataRepository<ProjectUser> _dataRepositoryProjectUser;
        private readonly IStringConstant _stringConstant;

        public ProjectTests() : base()
        {
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
            _dataRepository = serviceProvider.GetService<IDataRepository<Project>>();
            _dataRepositoryProjectUser = serviceProvider.GetService<IDataRepository<ProjectUser>>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();

        }
       
        /// <summary>
        /// This test case to add a new project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void AddProject()
        {
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            Task<int> id = _projectRepository.AddProject(projectac, _stringConstant.CreatedBy);
            var project = _dataRepository.FirstOrDefault(x => x.Id == id.Result);
            Assert.NotNull(project);
        }

        /// <summary>
        /// This test case for the add user and project in userproject table
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void AddUserProject()
        {

            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = _stringConstant.Name },
                UserId = _stringConstant.UserId,
                User = new ApplicationUser { FirstName = _stringConstant.FirstName }
            };
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
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = _stringConstant.Name },
                UserId = _stringConstant.UserId,
                User = new ApplicationUser { FirstName = _stringConstant.FirstName }
            };
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            Task<int> id = _projectRepository.AddProject(projectac, _stringConstant.CreatedBy);
            _projectRepository.AddUserProject(projectUser);
            Task<ProjectAc> project = _projectRepository.GetById(id.Result);
            Assert.NotNull(project);
        }

        /// <summary>
        /// This test case edit project 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void EditProject()
        {
            UserAc user = new UserAc()
            {

                FirstName = _stringConstant.FirstName,
                LastName = _stringConstant.LastName,
                Email = _stringConstant.Email,
                RoleName = _stringConstant.Employee
            };
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = _stringConstant.Name },
                UserId = _stringConstant.UserId,
                User = new ApplicationUser { FirstName = _stringConstant.FirstName }
            };
            UserAc userSecound = new UserAc()
            { Id = _stringConstant.UserIdSecond, FirstName = _stringConstant.FirstNameSecond };
            UserAc userThird = new UserAc()
            { Id = _stringConstant.UserIdThird, FirstName = _stringConstant.FirstNameThird };
            List<UserAc> userlist = new List<UserAc>();
            userlist.Add(user);
            userlist.Add(userSecound);
            userlist.Add(userThird);
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            Task<int> id = _projectRepository.AddProject(projectac, _stringConstant.FirstName);
            _projectRepository.AddUserProject(projectUser);
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = id.Result,
                Name = _stringConstant.EditName,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = _stringConstant.IsActive,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = _stringConstant.TeamLeaderId,
                CreatedBy = _stringConstant.CreatedBy,
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            _projectRepository.EditProject(projectacSecound, _stringConstant.CreatedBy);
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
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            _projectRepository.AddProject(projectac, _stringConstant.CreatedBy);
            var project = _projectRepository.checkDuplicate(projectac);
            Assert.Null(project.Name);
        }

        /// <summary>
        /// This test case for the check duplicate project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void checkDuplicatePositive()
        {
            UserAc userSecound = new UserAc()
            { Id = _stringConstant.UserIdSecond, FirstName = _stringConstant.FirstNameSecond };
            UserAc userThird = new UserAc()
            { Id = _stringConstant.UserIdThird, FirstName = _stringConstant.FirstNameThird };
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            _projectRepository.AddProject(projectac, _stringConstant.CreatedBy);
            List<UserAc> userlist = new List<UserAc>();
            userlist.Add(userSecound);
            userlist.Add(userThird);
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = 2,
                Name = _stringConstant.ProjectName,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = true,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = _stringConstant.TeamLeaderId,
                CreatedBy = _stringConstant.CreatedBy,
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            _projectRepository.AddProject(projectacSecound, _stringConstant.CreatedBy);
            var project = _projectRepository.checkDuplicate(projectacSecound);
            Assert.Null(project.Name);
        }

        /// <summary>
        /// This test case for the get all projects
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetAllProject()
        {
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = _stringConstant.Name },
                UserId = _stringConstant.UserId,
                User = new ApplicationUser { FirstName = _stringConstant.FirstName }
            };
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            _projectRepository.AddProject(projectac, _stringConstant.CreatedBy);
            _projectRepository.AddUserProject(projectUser);
            Task<IEnumerable<ProjectAc>> projects = _projectRepository.GetAllProjects();
            Assert.NotNull(projects);
        }

        /// <summary>
        /// Fetches Users of the given Project Name(slack channel name)
        /// </summary>
        [Fact, Trait("Category", "A")]
        public void GetProjectUserByGroupName()
        {
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = _stringConstant.Name },
                UserId = _stringConstant.UserId,
                User = new ApplicationUser { FirstName = _stringConstant.FirstName }
            };
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            _projectRepository.AddProject(projectac, _stringConstant.CreatedBy);
            _projectRepository.AddUserProject(projectUser);
            var projectUsers = _projectRepository.GetProjectUserByGroupName(projectac.SlackChannelName);
            Assert.NotEqual(projectUsers.Count, 2);
        }

       
        /// <summary>
        /// Fetch the project of the given slack channel name 
        /// </summary>
        [Fact, Trait("Category", "A")]
        public void GetProjectByGroupName()
        {
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            _projectRepository.AddProject(projectac, _stringConstant.CreatedBy);
            var project = _projectRepository.GetProjectByGroupName(projectac.SlackChannelName);
            Assert.Equal(projectac.TeamLeaderId, project.TeamLeaderId);
        }     
           
    }
}