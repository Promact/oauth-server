using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.ExceptionHandler;
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
        #region Private Variables
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringConstant _stringConstant;
        private readonly IProjectRepository _projectRepository;
        private readonly Mock<IEmailSender> _mockEmailService;
        #endregion

        #region Constructor
        public UserRepositoryTest() : base()
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _mapper = serviceProvider.GetService<IMapper>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
            _mockEmailService = serviceProvider.GetService<Mock<IEmailSender>>();
        }
        #endregion

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
                JoiningDate = DateTime.UtcNow.AddYears(-1),
                RoleName = _stringConstant.Employee
            };
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.CreatedBy);
            var user = await _userManager.FindByIdAsync(id);
            Assert.NotNull(user);
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
                JoiningDate = DateTime.UtcNow.AddYears(+1),
                RoleName = _stringConstant.Employee
            };
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.CreatedBy);
            var user = await _userManager.FindByIdAsync(id);
            Assert.NotNull(user);
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
            string id = await _userRepository.UpdateUserDetailsAsync(newUser, _stringConstant.RawFirstNameForTest);
            var editedUser = _userManager.FindByIdAsync(id).Result;
            Assert.Equal(_stringConstant.FirstName, editedUser.FirstName);
        }

        /// <summary>
        /// This test case is used for deleteing user details
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task DeleteUser()
        {
            string userId = await CreateMockAndUserAsync();
            await _userRepository.DeleteUserAsync(userId);
            var user = await _userManager.FindByIdAsync(userId);
            Assert.Null(user);
        }

        /// <summary>
        /// This test case is used for deleteing user details
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task DeleteUserNegative()
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
            await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);
            await _userRepository.DeleteUserAsync(userId);
            var user = await _userManager.FindByIdAsync(userId);
            Assert.NotNull(user);
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
        /// Test case use for getting teamLeader's details by users slack id
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ListOfTeamLeaderByUserIdAsync()
        {
            var userId = await CreateMockAndUserAsync();
            var user = await _userRepository.ListOfTeamLeaderByUserIdAsync(userId);
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
                JoiningDate = DateTime.UtcNow,
                RoleName = _stringConstant.Admin
            };
            string id = await _userRepository.AddUserAsync(userLocal, _stringConstant.RawFirstNameForTest);
            var user = await _userRepository.ListOfManagementDetailsAsync();
            Assert.Equal(1, user.Count);
        }

        /// <summary>
        /// Test case use to get roles
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetRoles()
        {
            var roles = await _userRepository.GetRolesAsync();
            Assert.Equal(2, roles.Count);
        }

        /// <summary>
        /// This test case is used to resend mail
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
        public async Task IsAdminAsync()
        {
            var userId = await CreateMockAndUserAsync();
            var result = await _userRepository.IsAdminAsync(userId);
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
                JoiningDate = DateTime.UtcNow,
                RoleName = _stringConstant.Admin
            };
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.CreatedBy);
            var userRole = await _userRepository.GetUserRoleAsync(id);
            Assert.Equal(1, userRole.Count());
        }

        /// <summary>
        ///This test case is used to get the team leader infromation.
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
            await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);
            var userRole = await _userRepository.GetUserRoleAsync(id);
            Assert.Equal(1, userRole.Count());
        }

        /// <summary>
        /// This test case is used to get team members with role using team leader id.   
        /// </summary>
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

        /// <summary>
        /// Test case to check UserBasicDetialByUserIdAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserBasicDetialByUserIdAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var user = await _userRepository.UserBasicDetialByUserIdAsync(userId);
            Assert.Equal(user.Email, _stringConstant.UserName);
        }

        /// <summary>
        /// Test case to throw Exception UserBasicDetialByUserIdAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserBasicDetialByUserIdAsyncForExceptionAsync()
        {
            var result = await Assert.ThrowsAsync<SlackUserNotFound>(() =>
            _userRepository.UserBasicDetialByUserIdAsync(_stringConstant.UserId));
            Assert.Equal(result.Message, _stringConstant.ExceptionMessageSlackUserNotFound);
        }

        /// <summary>
        /// Test case to check ListOfTeamLeaderByUserIdAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckListOfTeamLeaderByUserIdAsync()
        {
            string userId = await CreateMockAndUserAsync();
            var project = ProjectDetails();
            project.TeamLeaderId = userId;
            var projectId = await _projectRepository.AddProjectAsync(project, _stringConstant.Name);
            ProjectUser projectUser = new ProjectUser()
            {
                UserId = userId,
                ProjectId = projectId,
                CreatedBy = _stringConstant.UserId,
                CreatedDateTime = DateTime.UtcNow,
            };
            await _projectRepository.AddUserProjectAsync(projectUser);
            var user = await _userRepository.ListOfTeamLeaderByUserIdAsync(userId);
            Assert.Equal(user.Count, 1);
        }

        /// <summary>
        /// Test case to throw Exception ListOfTeamLeaderByUserIdAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ListOfTeamLeaderByUserIdAsyncForExceptionAsync()
        {
            var result = await Assert.ThrowsAsync<SlackUserNotFound>(() =>
            _userRepository.ListOfTeamLeaderByUserIdAsync(_stringConstant.UserId));
            Assert.Equal(result.Message, _stringConstant.ExceptionMessageSlackUserNotFound);
        }

        /// <summary>
        /// Test case to check ListOfManagementDetailsAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ListOfManagementDetailsAsync()
        {
            var userId = await _userRepository.AddUserAsync(UserDetails(), _stringConstant.UserId);
            var user = await _userRepository.ListOfManagementDetailsAsync();
            Assert.Equal(user.Count, 1);
        }

        /// <summary>
        /// Test case to throw Exception ListOfManagementDetailsAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ListOfManagementDetailsForExceptionAsync()
        {
            var result = await Assert.ThrowsAsync<FailedToFetchDataException>(() =>
            _userRepository.ListOfManagementDetailsAsync());
            Assert.Equal(result.Message, _stringConstant.ExceptionMessageFailedToFetchDataException);
        }

        /// <summary>
        /// Test case to check GetUserAllowedLeaveByUserIdAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserAllowedLeaveByUserIdAsync()
        {
            var userId = await CreateMockAndUserAsync();
            var leaveAllowed = await _userRepository.GetUserAllowedLeaveByUserIdAsync(userId);
            Assert.NotNull(leaveAllowed.CasualLeave);
        }

        /// <summary>
        /// Test case to throw Exception GetUserAllowedLeaveByUserIdAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserAllowedLeaveByUserIdAsyncForExceptionAsync()
        {
            var result = await Assert.ThrowsAsync<SlackUserNotFound>(() =>
            _userRepository.GetUserAllowedLeaveByUserIdAsync(_stringConstant.UserId));
            Assert.Equal(result.Message, _stringConstant.ExceptionMessageSlackUserNotFound);
        }

        /// <summary>
        /// Test case to check IsAdminAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckIsAdminAsync()
        {
            var userId = await _userRepository.AddUserAsync(UserDetails(), _stringConstant.UserId);
            var result = await _userRepository.IsAdminAsync(userId);
            Assert.Equal(result, true);
        }

        /// <summary>
        /// Test case to throw Exception IsAdminAsync method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task IsAdminForExceptionAsync()
        {
            var result = await Assert.ThrowsAsync<SlackUserNotFound>(() =>
            _userRepository.IsAdminAsync(_stringConstant.SlackUserId));
            Assert.Equal(result.Message, _stringConstant.ExceptionMessageSlackUserNotFound);
        }

        /// <summary>
        /// This test case is used for deleteing user details
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task DeleteUser()
        {
            string userId = await CreateMockAndUserAsync();
            await _userRepository.DeleteUserAsync(userId);
            var user = await _userManager.FindByIdAsync(userId);
            Assert.Null(user);
        }

        /// <summary>
        /// This test case is used for deleteing user details
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task DeleteUserNegative()
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
            await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);
        }
        #endregion

            #region Initialization
        private UserAc UserDetails()
        {
            return new UserAc()
            {
                Email = _stringConstant.UserName,
                FirstName = _stringConstant.FirstName,
                LastName = _stringConstant.LastName,
                IsActive = true,
                SlackUserId = _stringConstant.SlackUserId,
                UserName = _stringConstant.UserName,
                SlackUserName = _stringConstant.SlackUserName,
                JoiningDate = DateTime.UtcNow,
                RoleName = _stringConstant.Admin
            };
        }

        private ProjectAc ProjectDetails()
        {
            return new ProjectAc()
            {
                ApplicationUsers = new System.Collections.Generic.List<UserAc>()
                {
                    UserDetails()
                },
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                SlackChannelName = _stringConstant.SlackChannelName,
                TeamLeader = UserDetails(),
                CreatedBy = _stringConstant.UserId,
                Name = _stringConstant.ProjectName,
                TeamLeaderId = _stringConstant.UserId,
                UpdatedBy = _stringConstant.UserId,
                UpdatedDate = DateTime.UtcNow
            };
        }
        #endregion
    }
}
