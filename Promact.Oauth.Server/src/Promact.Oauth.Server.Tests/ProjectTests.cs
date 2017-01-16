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
using Promact.Oauth.Server.Repository;

namespace Promact.Oauth.Server.Tests
{
    public class ProjectTests : BaseProvider
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IDataRepository<Project> _dataRepository;
        private readonly IDataRepository<ProjectUser> _dataRepositoryProjectUser;
        private readonly IUserRepository _userRepository;
        private readonly IStringConstant _stringConstant;
        private readonly IUserRepository _userRepository;

        public ProjectTests() : base()
        {
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
            _dataRepository = serviceProvider.GetService<IDataRepository<Project>>();
            _dataRepositoryProjectUser = serviceProvider.GetService<IDataRepository<ProjectUser>>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
        }
      
        #region Test Case

        /// <summary>
        /// This test case to add a new project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task AddProjectAsync()
        {
            var id = await GetProjectMockData();
            var project = _dataRepository.FirstOrDefault(x => x.Id == id);
            Assert.NotNull(project);
        }

        /// <summary>
        /// This test case for the add user and project in userproject table
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void AddUserProject()
        {
            await GetProjectUserMockData();
            var ProjectUser = _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(ProjectUser);
        }

        /// <summary>
        /// This test case for gets project By Id
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetProjectById()
        {
            
            var id = await GetProjectMockData();
            await GetProjectUserMockData();
            ProjectAc project = await _projectRepository.GetProjectByIdAsync(id);
            Assert.NotNull(project);
        }

        /// <summary>
        /// This test case used to check exception condition 
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task GetProjectByIdExcption()
        {
            var id = await GetProjectMockData();
            await GetProjectUserMockData();
            Assert.Throws<AggregateException>(() => _projectRepository.GetProjectByIdAsync(2).Result);
        }

        /// <summary>
        /// This test case edit project 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task EditProject()
        {
            List<UserAc> userlist= GetUserListMockData();
            var id = await GetProjectMockData();
            await GetProjectUserMockData();
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = id,
                Name = _stringConstant.EditName,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = _stringConstant.IsActive,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = _stringConstant.TeamLeaderId,
                CreatedBy = _stringConstant.CreatedBy,
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            await _projectRepository.EditProjectAsync(id, projectacSecound, _stringConstant.CreatedBy);
            var project = _dataRepository.Fetch(x => x.Id == 1);
            _dataRepositoryProjectUser.Fetch(x => x.ProjectId == 1);
            Assert.NotNull(project);
        }

        /// <summary>
        /// TThis test case used to check exception condition 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task EditProjectExcption()
        {
            List<UserAc> userlist = GetUserListMockData();
            var id = await GetProjectMockData();
            await GetProjectUserMockData();
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = id,
                Name = _stringConstant.EditName,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = _stringConstant.IsActive,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = _stringConstant.TeamLeaderId,
                CreatedBy = _stringConstant.CreatedBy,
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };

            Assert.Throws<AggregateException>(() => _projectRepository.EditProjectAsync(2, projectacSecound, _stringConstant.CreatedBy).Result);
        }

        /// <summary>
        /// This test case for the check duplicate project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckDuplicateNegative()
        {
            ProjectAc projectAc = MockOfProjectAc();
            await _projectRepository.AddProjectAsync(projectAc, _stringConstant.CreatedBy);
            var project =await _projectRepository.CheckDuplicateProjectAsync(projectAc);
            Assert.Null(project.Name);
        }

        /// <summary>
        /// This test case for the check duplicate project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckDuplicatePositive()
        {
            List<UserAc> userlist = GetUserListMockData();
            var id = await GetProjectMockData();
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = 4,
                Name = _stringConstant.ProjectName,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = true,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = _stringConstant.TeamLeaderId,
                CreatedBy = _stringConstant.CreatedBy,
                CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                ApplicationUsers = userlist
            };
            await _projectRepository.AddProjectAsync(projectacSecound, _stringConstant.CreatedBy);
            var project =await _projectRepository.CheckDuplicateProjectAsync(projectacSecound);
            Assert.Null(project.SlackChannelName);
        }

        /// <summary>
        /// This test case for the get all projects
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetAllProject()
        {
            UserAc _testUser = new UserAc()
            {
                Email = _stringConstant.RawEmailIdForTest,
                FirstName = _stringConstant.RawFirstNameForTest,
                LastName = _stringConstant.RawLastNameForTest,
                IsActive = true,
                UserName = _stringConstant.RawEmailIdForTest,
                SlackUserName = _stringConstant.RawFirstNameForTest,
                JoiningDate = DateTime.UtcNow,
                RoleName = _stringConstant.Employee
            };
            var id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
           
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = id;
            projectac.CreatedBy = id;
            await _projectRepository.AddProjectAsync(projectac, id);
            await GetProjectUserMockData();
            IEnumerable<ProjectAc> projects =await _projectRepository.GetAllProjectsAsync();
            Assert.NotNull(projects);
        }

        /// <summary>
        /// Fetch the project of the given slack channel name 
        /// </summary>
        [Fact, Trait("Category", "A")]
        public async Task GetProjectByGroupName()
        {
            ProjectAc projectAc = MockOfProjectAc();
            await _projectRepository.AddProjectAsync(projectAc, _stringConstant.CreatedBy);
            var project =await _projectRepository.GetProjectByGroupNameAsync(projectAc.SlackChannelName);
            Assert.Equal(projectAc.Name, project.Name);
        }

        /// <summary>
        /// This test case used to check exception condition 
        /// </summary>
        [Fact, Trait("Category", "A")]
        public async Task GetProjectByGroupNameException()
        {
            ProjectAc projectAc = MockOfProjectAc();
            await _projectRepository.AddProjectAsync(projectAc, _stringConstant.CreatedBy);
            Assert.Throws<AggregateException>(() => _projectRepository.GetProjectByGroupNameAsync("test").Result);
        }

        private ProjectAc MockOfProjectAc()
        {
            ProjectAc projectAc = new ProjectAc();
            projectAc.Name = _stringConstant.Name;
            projectAc.SlackChannelName = _stringConstant.SlackChannelName;
            projectAc.IsActive = _stringConstant.IsActive;
            projectAc.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectAc.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectAc.CreatedBy = _stringConstant.CreatedBy;
            return projectAc;
        }

        private async Task<int> GetProjectMockData()
        {
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            return await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);

        }

        private async Task GetProjectUserMockData()
        {
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = 1,
                Project = new Project { Name = _stringConstant.Name },
                UserId = _stringConstant.UserId,
                User = new ApplicationUser { FirstName = _stringConstant.FirstName }
            };
            await _projectRepository.AddUserProjectAsync(projectUser);
        }

        private List<UserAc> GetUserListMockData()
        {
            List<UserAc> userlist = new List<UserAc>();
            UserAc user = new UserAc()
            { FirstName = _stringConstant.FirstName };
            UserAc userSecound = new UserAc()
            { Id = _stringConstant.UserIdSecond, FirstName = _stringConstant.FirstNameSecond };
            UserAc userThird = new UserAc()
            { Id = _stringConstant.UserIdThird, FirstName = _stringConstant.FirstNameThird };
            userlist.Add(user);
            userlist.Add(userSecound);
            userlist.Add(userThird);
            return userlist;

        }

        /// <summary>
        /// Test case to check GetProjectsWithUsers 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async void TestGetProjectsWithUsers()
        {
            string id = await CreateMockAndUserAsync();
            ProjectAc project = new ProjectAc()
            {
                Name = _stringConstant.Name,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = _stringConstant.IsActive,
                TeamLeaderId = id,
                CreatedBy = _stringConstant.CreatedBy,
            };
            await _projectRepository.AddProjectAsync(project, _stringConstant.CreatedBy);
            var projectUsers =await  _projectRepository.GetProjectsWithUsersAsync();
            Assert.NotNull(projectUsers);
        }

        /// <summary>
        /// Test case to check GetProjectDetails
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async void TestGetProjectDetails()
        {
            string id = await CreateMockAndUserAsync();
            ProjectAc project = new ProjectAc()
            {
                Name = _stringConstant.Name,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = _stringConstant.IsActive,
                TeamLeaderId = id,
                CreatedBy = _stringConstant.CreatedBy,
            };
            var projectId = await _projectRepository.AddProjectAsync(project, _stringConstant.CreatedBy);
            var projectDetails = await _projectRepository.GetProjectDetailsAsync(projectId);
            Assert.Equal(projectDetails.Name, _stringConstant.Name);
        }

        #endregion
    }
}