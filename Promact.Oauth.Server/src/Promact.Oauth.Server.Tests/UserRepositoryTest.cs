using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Promact.Oauth.Server.Tests
{
    public class UserRepositoryTest : BaseProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringConstant _stringConstant;
        private readonly IProjectRepository _projectRepository;
        private readonly Mock<IEmailSender> _mockEmailService;

        public UserRepositoryTest() : base()
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _mapper = serviceProvider.GetService<IMapper>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
            _mockEmailService = serviceProvider.GetService<Mock<IEmailSender>>();
        }

        #region Test Case

        /// <summary>
        /// This test case is used to gets the list of all users
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetAllUser()
        {
            await CreateMockAndUserAsync();
            var listOfUsers = await _userRepository.GetAllUsersAsync();
            Assert.NotEqual(0, listOfUsers.Count());
        }

        /// <summary>
        /// This test case is used to get user object by id.
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserById()
        {
            string id = await CreateMockAndUserAsync();
            var user = await _userRepository.GetByIdAsync(id);
            Assert.NotNull(user);
        }

        /// <summary>
        /// This test case used to check exception condition 
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserByIdExcption()
        {
            await CreateMockAndUserAsync();
            Assert.Throws<AggregateException>(() => _userRepository.GetByIdAsync(_stringConstant.UserIdForTest).Result);
        }

        /// <summary>
        /// This test case is used to check email exists
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task EmailIsExists()
        {
            await CreateMockAndUserAsync();
            var exists = await _userRepository.CheckEmailIsExistsAsync(_stringConstant.UserName);
            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case is used to find user by username
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task FindByUserName()
        {
            await CreateMockAndUserAsync();
            var exists = await _userRepository.FindByUserNameAsync(_stringConstant.UserName);
            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case is used to check exception condition
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task FindByUserNameException()
        {
            await CreateMockAndUserAsync();
            Assert.Throws<AggregateException>(() => _userRepository.FindByUserNameAsync(_stringConstant.UserNameForTest).Result);
        }

        /// <summary>
        /// This test case is used to add new user
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task AddUser()
        {
            string id = await CreateMockAndUserAsync();
            var user = await _userManager.FindByIdAsync(id);
            Assert.NotNull(user);
        }

        /// <summary>
        /// This test case is used to calculate allowed leaves for past years
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CalculateAllowedLeavesForPastyears()
        {


            UserAc _testUser = new UserAc()
            {
                Email = _stringConstant.RawEmailIdForTest,
                FirstName = _stringConstant.RawFirstNameForTest,
                LastName = _stringConstant.RawLastNameForTest,
                IsActive = true,
                UserName = _stringConstant.RawEmailIdForTest,
                SlackUserName = _stringConstant.RawFirstNameForTest,
                JoiningDate = DateTime.UtcNow.AddYears(-1),
                RoleName = _stringConstant.Employee
            };
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.CreatedBy);
            var user = await _userManager.FindByIdAsync(id);
            Assert.NotNull(id);
        }

        /// <summary>
        /// This test case is used to Calculate Allowed Leaves for future year
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CalculateAllowedLeavesForFutureyear()
        {
            UserAc _testUser = new UserAc()
            {
                Email = _stringConstant.RawEmailIdForTest,
                FirstName = _stringConstant.RawFirstNameForTest,
                LastName = _stringConstant.RawLastNameForTest,
                IsActive = true,
                UserName = _stringConstant.RawEmailIdForTest,
                SlackUserName = _stringConstant.RawFirstNameForTest,
                JoiningDate = DateTime.UtcNow.AddYears(+1),
                RoleName = _stringConstant.Employee
            };
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.CreatedBy);
            var user = await _userManager.FindByIdAsync(id);
            Assert.NotNull(id);
        }

        /// <summary>
        /// This test case is used for updating user details
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UpdateUser()
        {
            string userId = await CreateMockAndUserAsync();
            var user = await _userManager.FindByIdAsync(userId);
            var newUser = _mapper.Map<ApplicationUser, UserAc>(user);
            newUser.RoleName = _stringConstant.Employee;
            newUser.FirstName = _stringConstant.FirstName;
            newUser.SlackUserName = _stringConstant.FirstName;
            string id = await _userRepository.UpdateUserDetailsAsync(newUser, _stringConstant.RawFirstNameForTest);
            var editedUser = _userManager.FindByIdAsync(id).Result;
            Assert.Equal(_stringConstant.FirstName, editedUser.FirstName);
        }
      

        /// <summary>
        /// Test case is used to get user details by id
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserDetailById()
        {
            string id = await CreateMockAndUserAsync();
            var user = await _userRepository.UserDetailByIdAsync(id);
            Assert.Equal(user.Email, _stringConstant.UserName);
        }

        /// <summary>
        /// Test case use for getting TeamLeader's details by users slack name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task TeamLeaderByUserSlackId()
        {
            string id = await CreateMockAndUserAsync();
            var user = await _userRepository.TeamLeaderByUserSlackIdAsync(_stringConstant.SlackUserId);
            Assert.Equal(0, user.Count);
        }

        /// <summary>
        /// Test case use to get list of management people
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ManagementDetails()
        {
            await CreateMockAndUserAsync();
            UserAc userLocal = new UserAc()
            {
                Email = _stringConstant.Email,
                FirstName = _stringConstant.RawFirstNameForTest,
                LastName = _stringConstant.RawLastNameForTest,
                IsActive = true,
                UserName = _stringConstant.Email,
                SlackUserName = _stringConstant.RawFirstNameForTest,
                JoiningDate = DateTime.UtcNow,
                RoleName = _stringConstant.Admin
            };
            string id = await _userRepository.AddUserAsync(userLocal, _stringConstant.RawFirstNameForTest);
            var user = await _userRepository.ManagementDetailsAsync();
            Assert.Equal(1, user.Count);
        }

        /// <summary>
        /// Test case to check get roles of user repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetRoles()
        {
            var roles = await _userRepository.GetRolesAsync();
            Assert.Equal(2, roles.Count);
        }


        /// <summary>
        /// Test case used to find user by username
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task FindUserBySlackUserName()
        {
            await CreateMockAndUserAsync();
            var result = _userRepository.FindUserBySlackUserNameAsync(_stringConstant.RawFirstNameForTest);
            Assert.NotNull(result);
        }

        /// <summary>
        /// This test case is used to check exception condition 
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task FindUserBySlackUserNameException()
        {
            await CreateMockAndUserAsync();
            Assert.Throws<AggregateException>(() => _userRepository.FindUserBySlackUserNameAsync(_stringConstant.SlackUserNameForTest).Result);
        }

        /// <summary>
        /// This test case is used to check exception condition
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task ReSendMail()
        {
            string id = await CreateMockAndUserAsync();
            await _userRepository.ReSendMailAsync(id);
            _mockEmailService.VerifyAll();
        }

        /// <summary>
        /// This test case is used to get all employees
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task GetAllEmployees()
        {
            await CreateMockAndUserAsync();
            var listOfEmployees = await _userRepository.GetAllEmployeesAsync();
            Assert.NotEqual(0, listOfEmployees.Count());
        }

        /// <summary>
        ///This test case is used to test method IsAdmin of user repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task IsAdmin()
        {
            await CreateMockAndUserAsync();
            var result = await _userRepository.IsAdminAsync(_stringConstant.SlackUserId);
            Assert.Equal(false, result);
        }

        /// <summary>
        ///This test case is used to get the user role by user id
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserRoleAsync()
        {
            string id = await CreateMockAndUserAsync();
            var userRole = await _userRepository.GetUserRoleAsync(id);
            Assert.Equal(1, userRole.Count());
        }

        /// <summary>
        ///This test case is used to get the list of active user with role using admin user id.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserRoleAdmin()
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
                RoleName = _stringConstant.Admin
            };
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.CreatedBy);
            var userRole = await _userRepository.GetUserRoleAsync(id);
            Assert.Equal(1, userRole.Count());
        }

        /// <summary>
        ///This test case is used to get the list of active user with role using admin user id.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserRoleTeamLeader()
        {
            string id = await CreateMockAndUserAsync();
            ProjectAc projectac = new ProjectAc()
            {
                Name = _stringConstant.Name,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = _stringConstant.IsActive,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = id,
                CreatedBy = _stringConstant.CreatedBy

            };
            var projectId = await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);
            var userRole = await _userRepository.GetUserRoleAsync(id);
            Assert.Equal(1, userRole.Count());
        }

        /// <summary>
        /// This test case is used to get team members with role using team leader id.   
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task GetTeamMembersAsync()
        {
            string userId = await CreateMockAndUserAsync();
            ProjectAc projectac = new ProjectAc()
            {
                Name = _stringConstant.Name,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = _stringConstant.IsActive,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = userId,
                CreatedBy = _stringConstant.CreatedBy

            };
            var projectId = await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = projectId,
                UserId = userId,
                CreatedBy = userId,
                CreatedDateTime = DateTime.UtcNow,
            };
            await _projectRepository.AddUserProjectAsync(projectUser);
            var userRole = await _userRepository.GetTeamMembersAsync(userId);
            Assert.Equal(2, userRole.Count());
        }


        /// <summary>
        /// Fetches Users of the given project name(slack channel name)
        /// </summary>
        [Fact, Trait("Category", "A")]
        public async Task GetProjectUserBySlackChannelNameAsync()
        {
            string userId = await CreateMockAndUserAsync();
            ProjectAc projectac = new ProjectAc();
            projectac.Name = _stringConstant.Name;
            projectac.SlackChannelName = _stringConstant.SlackChannelName;
            projectac.IsActive = _stringConstant.IsActive;
            projectac.TeamLeader = new UserAc { FirstName = _stringConstant.FirstName };
            projectac.TeamLeaderId = _stringConstant.TeamLeaderId;
            projectac.CreatedBy = _stringConstant.CreatedBy;
            int projectId = await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);
            ProjectUser projectUser = new ProjectUser()
            {
                ProjectId = projectId,
                UserId = userId,
                CreatedBy = userId,
                CreatedDateTime = DateTime.UtcNow,
            };
            await _projectRepository.AddUserProjectAsync(projectUser);
            var projectUsers = await _userRepository.GetProjectUserBySlackChannelNameAsync(projectac.SlackChannelName);
            Assert.Equal(projectUsers.Count, 1);
        }


        /// <summary>
        /// Test case to check GetProjectUsersByTeamLeaderId method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async void TestGetProjectUsersByTeamLeaderId()
        {
            string id = await CreateMockAndUserAsync();
            ProjectAc project = new ProjectAc()
            {
                Name = _stringConstant.Name,
                SlackChannelName = _stringConstant.SlackChannelName,
                IsActive = _stringConstant.IsActive,
                TeamLeader = new UserAc { FirstName = _stringConstant.FirstName },
                TeamLeaderId = id,
                CreatedBy = _stringConstant.CreatedBy,
            };

            await _projectRepository.AddProjectAsync(project, _stringConstant.CreatedBy);
            var projectUsers = await _userRepository.GetProjectUsersByTeamLeaderIdAsync(id);
            Assert.NotNull(projectUsers);
        }

        #endregion
    }
}
