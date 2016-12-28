using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using System;
using System.Collections.Generic;
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
        public UserRepositoryTest() : base()
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _mapper = serviceProvider.GetService<IMapper>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
            _projectRepository = serviceProvider.GetService<IProjectRepository>();
        }

        #region Test Case

        /// <summary>
        /// This test case gets the list of all users
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetAllUser()
        {
            UserAc _testUser = new UserAc()
            {
                Email = _stringConstant.RawEmailIdForTest,
                FirstName = _stringConstant.RawFirstNameForTest,
                LastName = _stringConstant.RawLastNameForTest,
                IsActive = true,
                UserName = _stringConstant.RawEmailIdForTest,
                SlackUserId = _stringConstant.RawFirstNameForTest,
                JoiningDate = DateTime.UtcNow,
                RoleName = _stringConstant.Employee
            };
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var listOfUsers = await _userRepository.GetAllUsersAsync();
            Assert.NotEqual(0, listOfUsers.Count());
        }

        /// <summary>
        /// This test case used to get user object by id.
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserById()
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
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
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
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            Assert.Throws<AggregateException>(() => _userRepository.GetByIdAsync(_stringConstant.UserIdForTest).Result);
        }

        /// <summary>
        /// This test case used to check exception condition 
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserByIdExcption()
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
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            Assert.Throws<AggregateException>(() => _userRepository.GetByIdAsync(_stringConstant.UserIdForTest).Result);
        }

        /// <summary>
        /// This test case used to check email exists
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task EmailIsExists()
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
            var result = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var exists = await _userRepository.CheckEmailIsExistsAsync(_stringConstant.RawEmailIdForTest);
            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case used to check does not exists
        /// </summary>
        public async Task EmailDoesNotExists()
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
            var result = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var exists = await _userRepository.CheckEmailIsExistsAsync(_stringConstant.EmailForTest);
            Assert.Equal(false, exists);
        }

     

        /// <summary>
        /// This test case used to check does not exists
        /// </summary>
        public async Task EmailDoesNotExists()
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
            var result = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var exists = await _userRepository.CheckEmailIsExistsAsync(_stringConstant.EmailForTest);
            Assert.Equal(false, exists);
        }

        /// <summary>
        /// This test case used to find user by username
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task FindByUserName()
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
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var exists = await _userRepository.FindByUserNameAsync(_stringConstant.RawEmailIdForTest);
            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case used to check exception condition
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task FindByUserNameException()
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
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            Assert.Throws<AggregateException>(() => _userRepository.FindByUserNameAsync(_stringConstant.UserNameForTest).Result);
        }


        /// <summary>
        /// This test case used to check exception condition
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task FindByUserNameException()
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
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            Assert.Throws<AggregateException>(() => _userRepository.FindByUserNameAsync(_stringConstant.UserNameForTest).Result);
        }
        
        /// <summary>
        /// This test case is used for adding new user
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task AddUser()
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
            var userId = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
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
        /// Test case use for getting user details by its slack name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserDetailById()
        {
            UserAc _testUser = new UserAc()
            {
                Email = _stringConstant.RawEmailIdForTest,
                FirstName = _stringConstant.RawFirstNameForTest,
                LastName = _stringConstant.RawLastNameForTest,
                IsActive = true,
                UserName = _stringConstant.RawEmailIdForTest,
                SlackUserName = _stringConstant.RawFirstNameForTest,
                SlackUserId = _stringConstant.RawFirstNameForTest,
                JoiningDate = DateTime.UtcNow,
                RoleName = _stringConstant.Employee
            };
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var user = await _userRepository.UserDetailByIdAsync(id);
            Assert.Equal(user.Email, _testUser.Email);
        }

        /// <summary>
        /// Test case use for getting TeamLeader's details by users slack name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task TeamLeaderByUserSlackId()
        {
            UserAc _testUser = new UserAc()
            {
                Email = _stringConstant.RawEmailIdForTest,
                FirstName = _stringConstant.RawFirstNameForTest,
                LastName = _stringConstant.RawLastNameForTest,
                IsActive = true,
                UserName = _stringConstant.RawEmailIdForTest,
                SlackUserName = _stringConstant.RawFirstNameForTest,
                SlackUserId = _stringConstant.RawFirstNameForTest,
                JoiningDate = DateTime.UtcNow,
                RoleName = _stringConstant.Employee
            };
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var user = await _userRepository.TeamLeaderByUserSlackIdAsync(_stringConstant.RawFirstNameForTest);
            Assert.Equal(0, user.Count);
        }

        /// <summary>
        /// Test case use to get list of management people
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ManagementDetails()
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
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            id = await _userRepository.AddUserAsync(userLocal, _stringConstant.RawFirstNameForTest);
            var user = await _userRepository.ManagementDetailsAsync();
            Assert.Equal(1, user.Count);
        }

        /// <summary>
        /// Test case to check GetRoles of User Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetRoles()
        {
            var roles = await _userRepository.GetRolesAsync();
            Assert.Equal(2, roles.Count);
        }

        /// <summary>
        /// Test case to check GetUserDetail of User Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserDetail()
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
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var user = await _userRepository.GetUserDetailAsync(_stringConstant.RawEmailIdForTest);
            Assert.Equal(id, user.Id);
        }

        /// <summary>
        /// Test case used to find user by username
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task FindUserBySlackUserName()
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
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var result = _userRepository.FindUserBySlackUserNameAsync(_stringConstant.RawFirstNameForTest);
            Assert.NotNull(result);
        }

        /// <summary>
        /// This test case used to check exception condition 
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task FindUserBySlackUserNameException()
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
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            Assert.Throws<AggregateException>(() => _userRepository.FindUserBySlackUserNameAsync(_stringConstant.SlackUserNameForTest).Result);
        }

        /// <summary>
        /// This test case used  to failed resend mail
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task FailedReSendMail()
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
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var result = await _userRepository.ReSendMailAsync(id);
            Assert.Equal(false, result);
        }

        /// <summary>
        /// This test case used for get all employees
        /// </summary>
        /// <returns></returns>
        [Fact, Trait("Category", "Required")]
        public async Task GetAllEmployees()
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
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var listOfEmployees = await _userRepository.GetAllEmployeesAsync();
            Assert.NotEqual(0, listOfEmployees.Count());
        }

        /// <summary>
        /// Test case to check method IsAdmin of user repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task IsAdmin()
        {
            UserAc _testUser = new UserAc()
            {
                Email = _stringConstant.RawEmailIdForTest,
                FirstName = _stringConstant.RawFirstNameForTest,
                LastName = _stringConstant.RawLastNameForTest,
                IsActive = true,
                UserName = _stringConstant.RawEmailIdForTest,
                SlackUserName = _stringConstant.RawFirstNameForTest,
                SlackUserId = _stringConstant.RawFirstNameForTest,
                JoiningDate = DateTime.UtcNow,
                RoleName = _stringConstant.Employee
            };
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var result = await _userRepository.IsAdminAsync(_testUser.SlackUserId);
            Assert.Equal(false, result);
        }

        /// <summary>
        /// Test case to get the user role by username
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserRoleAsync()
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
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.CreatedBy);
            var userRole = await _userRepository.GetUserRoleAsync(_testUser.Id);
            Assert.Equal(1, userRole.Count());
        }

        /// <summary>
        /// Test case to get the user role by username
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
            var userRole = await _userRepository.GetUserRoleAsync(_testUser.Id);
            Assert.Equal(1, userRole.Count());
        }

        [Fact, Trait("Category", "Required")]
        public async Task GetTeamMembersAsync()
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

            string userId = await _userRepository.AddUserAsync(_testUser, _stringConstant.CreatedBy);
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
            var userRole = await _userRepository.GetTeamMembersAsync(_testUser.Id);
            Assert.Equal(1, userRole.Count());
        }

        /// <summary>
        /// Fetches Users of the given Project Name(slack channel name)
        /// </summary>
        [Fact, Trait("Category", "A")]
        public async Task GetProjectUserByGroupNameAsync()
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
            await _projectRepository.AddProjectAsync(projectac, _stringConstant.CreatedBy);
            await _projectRepository.AddUserProjectAsync(projectUser);
            var projectUsers = await _userRepository.GetProjectUserByGroupNameAsync(projectac.SlackChannelName);
            Assert.NotEqual(projectUsers.Count, 2);
        }

        /// <summary>
        /// Test case to check UserDetailById method of User Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async void TestUserDetailById()
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
            var id = await _userRepository.AddUserAsync(_testUser, _stringConstant.RawFirstNameForTest);
            var user = await _userRepository.UserDetailByIdAsync(id);
            Assert.Equal(user.FirstName, _stringConstant.RawFirstNameForTest);
        }

        /// <summary>
        /// Test case to check GetProjectUsersByTeamLeaderId method of user repository 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async void TestGetProjectUsersByTeamLeaderId()
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
            string id = await _userRepository.AddUserAsync(_testUser, _stringConstant.CreatedBy);
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
            Assert.NotNull(projectUsers.Count);
        }

        #endregion


    }
}
