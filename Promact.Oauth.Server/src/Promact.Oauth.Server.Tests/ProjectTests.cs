using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Data_Repository;
using Xunit;
using System;
using Promact.Oauth.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Data;

namespace Promact.Oauth.Server.Tests
{
    public class ProjectTests : BaseProvider
    {
        private readonly IProjectRepository _projectRepository;

        private readonly IDataRepository<Project, PromactOauthDbContext> _dataRepository;
        private readonly IDataRepository<ProjectUser, PromactOauthDbContext> _dataRepositoryProjectUser;
        private readonly IStringConstant _stringConstant;
        private readonly IUserRepository _userRepository;

        public ProjectTests() : base()
        {
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
            _dataRepository = serviceProvider.GetService<IDataRepository<Project, PromactOauthDbContext>>();
            _dataRepositoryProjectUser = serviceProvider.GetService<IDataRepository<ProjectUser, PromactOauthDbContext>>();
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
        public async Task AddUserProject()
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
        /// This test case for gets project By Id without team leader
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetProjectByProjectId()
        {
            string userId = await MockOfUserAc();
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = userId;
            var id=await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);
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
                CreatedDate = DateTime.UtcNow,
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
                CreatedDate = DateTime.Now,
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
            var project = await _projectRepository.CheckDuplicateProjectAsync(projectAc);
            Assert.Null(project.Name);
        }

        /// <summary>
        /// This test case for the check duplicate project
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckDuplicatePositive()
        {
            List<UserAc> userlist = GetUserListMockData();
            await GetProjectMockData();
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = 4,
                Name = _stringConstant.ProjectName,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = true,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = _stringConstant.TeamLeaderId,
                CreatedBy = _stringConstant.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ApplicationUsers = userlist
            };
            var project = await _projectRepository.CheckDuplicateProjectAsync(projectacSecound);
            Assert.Null(project.SlackChannelName);
        }


        /// <summary>
        /// This test case for the check duplicate where project name and slack channel name both are already in database.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckDuplicateProject()
        {
            List<UserAc> userlist = GetUserListMockData();
            await GetProjectMockData();
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = 5,
                Name = _stringConstant.Name,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = true,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = _stringConstant.TeamLeaderId,
                CreatedBy = _stringConstant.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ApplicationUsers = userlist
            };
            var project = await _projectRepository.CheckDuplicateProjectAsync(projectacSecound);
            Assert.Null(project.SlackChannelName);
        }

        /// <summary>
        /// This test case for the check duplicate where project name and slack channel name both not in database.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task TestCheckDuplicateNegative()
        {
            List<UserAc> userlist = GetUserListMockData();
            await GetProjectMockData();
            ProjectAc projectacSecound = new ProjectAc()
            {
                Id = 5,
                Name =_stringConstant.LastName,
                SlackChannelName = _stringConstant.LastName,
                IsActive = true,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = _stringConstant.TeamLeaderId,
                CreatedBy = _stringConstant.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ApplicationUsers = userlist
            };
            var project = await _projectRepository.CheckDuplicateProjectAsync(projectacSecound);
            Assert.NotNull(project.SlackChannelName);
        }


        /// <summary>
        /// This test case for the get all projects
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetAllProject()
        {
            var id = await MockOfUserAc();
            ProjectAc projectAc = MockOfProjectAc();
            projectAc.TeamLeaderId = id;
            projectAc.CreatedBy = id;
            var projectId = await _projectRepository.AddProjectAsync(projectAc, id);
            IEnumerable<ProjectAc> projects = await _projectRepository.GetAllProjectsAsync();
            Assert.NotNull(projects);
        }

        /// <summary>
        /// This test case for the get all projects without Team leader
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetAllProjects()
        {
            var id = await MockOfUserAc();
            ProjectAc projectAc = MockOfProjectAc();
            projectAc.TeamLeaderId = "";
            projectAc.CreatedBy = id;
            var projectId = await _projectRepository.AddProjectAsync(projectAc, id);
            IEnumerable<ProjectAc> projects = await _projectRepository.GetAllProjectsAsync();
            Assert.NotNull(projects);
        }

        /// <summary>
        /// Fetch the project of the given slack channel name 
        /// </summary>
        [Fact, Trait("Category", "A")]
        public async Task GetProjectByGroupName()
        {
            var userId = await MockOfUserAc();
            ProjectAc projectAc = MockOfProjectAc();
            projectAc.TeamLeaderId = userId;
            await _projectRepository.AddProjectAsync(projectAc, _stringConstant.CreatedBy);
            var project = await _projectRepository.GetProjectBySlackChannelNameAsync(projectAc.SlackChannelName);
            Assert.Equal(projectAc.Name, project.Name);
        }

        /// <summary>
        /// Fetch the project of the given slack channel name  with out teamleader
        /// </summary>
        [Fact, Trait("Category", "A")]
        public async Task GetProjectByGroupNameWithoutTeamLeader()
        {
            var userId = await MockOfUserAc();
            ProjectAc projectAc = MockOfProjectAc();
            projectAc.TeamLeaderId = "";
            await _projectRepository.AddProjectAsync(projectAc, _stringConstant.CreatedBy);
            var project = await _projectRepository.GetProjectBySlackChannelNameAsync(projectAc.SlackChannelName);
            Assert.Null(project.Name);
        }

        /// <summary>
        /// Fetch the project of the given Team leader
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "A")]
        public async Task GetAllProjectForUserAsync()
        {
            var userId = await MockOfUserAc();
            ProjectAc projectAc = MockOfProjectAc();
            projectAc.TeamLeaderId = userId;
            await _projectRepository.AddProjectAsync(projectAc, _stringConstant.CreatedBy);
            var project = await _projectRepository.GetAllProjectForUserAsync(userId);
            Assert.NotNull(projectAc);
        }

        /// <summary>
        /// Fetch the project of the given user
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "A")]
        public async Task GetAllProjectForUserAsyncForUser()
        {
            await GetProjectUserMockData();
            var project = await _projectRepository.GetAllProjectForUserAsync(_stringConstant.UserId);
            Assert.NotNull(project);
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
            var projectUsers = await _projectRepository.GetProjectsWithUsersAsync();
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

        #region private methods
        /// <summary>
        /// mock data of project
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetProjectMockData()
        {
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            return await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);

        }

        /// <summary>
        /// mock data of projectuser.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// mock of users data.
        /// </summary>
        /// <returns></returns>
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
        /// mock of project ac
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Creates mock user
        /// </summary>
        /// <returns>id of user created</returns>
        private async Task<string> MockOfUserAc()
        {
            UserAc user = new UserAc()
            {
                Email = _stringConstant.RawEmailIdForTest,
                JoiningDate = DateTime.UtcNow,
                IsActive = true,
                RoleName = _stringConstant.Employee
            };
            return await _userRepository.AddUserAsync(user, _stringConstant.RawFirstNameForTest);
        }


        #endregion
    }
}