using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Models.ManageViewModels;
using Promact.Oauth.Server.Repository;
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
        public UserRepositoryTest() : base()
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        #region Test Case

        /// <summary>
        /// This test case gets the list of all users
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetAllUser()
        {
            var id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            IEnumerable<UserAc> users = _userRepository.GetAllUsers();
            Assert.Equal(1, users.Count());
        }

        /// <summary>
        /// This test case gets the user by its id
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserById()
        {
            var id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            UserAc testUser = await _userRepository.GetById(id);
            Assert.Equal(testUser.Email, StringConstant.RawEmailIdForTest);
        }

        /// <summary>
        /// This test case checks if a user exists with the specified Email
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckEmailIsExists()
        {
            var result = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            var exists = await _userRepository.CheckEmailIsExists(StringConstant.RawEmailIdForTest);
            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case checks if a user exists with the specified UserName
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task FindByUserName()
        {
            var id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            var exists = await _userRepository.FindByUserName(StringConstant.RawEmailIdForTest);
            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case is used for adding new user
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task AddUser()
        {
            string id = await _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            var user = await _userManager.FindByIdAsync(id);
            Assert.NotNull(id);
        }

        /// <summary>
        /// This test case is used for updating user details
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UpdateUser()
        {
            var userId = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            var user = await _userManager.FindByIdAsync(userId);
            var newUser = _mapper.Map<ApplicationUser, UserAc>(user);
            newUser.RoleName = StringConstant.Employee;
            newUser.FirstName = StringConstant.FirstName;
            newUser.SlackUserName = StringConstant.FirstName;
            string id = await _userRepository.UpdateUserDetails(newUser, StringConstant.RawFirstNameForTest);
            var editedUser = _userManager.FindByIdAsync(id).Result;
            Assert.Equal(StringConstant.FirstName, editedUser.FirstName);
        }

        ///// <summary>
        ///// This test case is used for changing the password of an user
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task ChangePassword()
        //{
        //    var id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
        //    var user = await _userManager.FindByIdAsync(id);


        //    var password = await _userRepository.ChangePassword(new ChangePasswordViewModel
        //    {
        //        OldPassword = StringConstant.OldPassword,
        //        NewPassword = StringConstant.NewPassword,
        //        ConfirmPassword = StringConstant.NewPassword,
        //        Email = user.Email
        //    });
        //    var passwordMatch = await _userManager.CheckPasswordAsync(user, password);
        //    Assert.Equal(true, passwordMatch);
        //}

        /// <summary>
        /// Test case use for getting user details by its slack name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task UserDetail()
        {
            string id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            var user = _userRepository.UserDetialByUserSlackName(StringConstant.RawFirstNameForTest);
            Assert.Equal(user.Email, _testUser.Email);
        }

        /// <summary>
        /// Test case use for getting TeamLeader's details by users slack name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task TeamLeaderByUserSlackName()
        {
            string id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            var user = await _userRepository.TeamLeaderByUserSlackName(StringConstant.RawFirstNameForTest);
            Assert.Equal(0, user.Count);
        }

        /// <summary>
        /// Test case use to get list of management people
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ManagementDetails()
        {
            string id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            id = await _userRepository.AddUser(userLocal, StringConstant.RawFirstNameForTest);
            var user = await _userRepository.ManagementDetails();
            Assert.Equal(1, user.Count);
        }

        /// <summary>
        /// Test case to get user's number of casual leave
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetUserCasualLeaveBySlackName()
        {
            var id = _userRepository.AddUser(userLocal, StringConstant.RawFirstNameForTest);
            var casualLeave = _userRepository.GetUserAllowedLeaveBySlackName(userLocal.SlackUserName);
            Assert.NotNull(casualLeave);
        }

        /// <summary>
        /// Test case to check GetRoles of User Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetRoles()
        {
            var roles = _userRepository.GetRoles();
            Assert.Equal(2, roles.Count);
        }

        /// <summary>
        /// Test case to check GetUserDetail of User Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task GetUserDetail()
        {
            var id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            var user = await _userRepository.GetUserDetail(StringConstant.RawEmailIdForTest);
            Assert.Equal(id, user.Id);
        }

        /// <summary>
        /// Test case to check FindUserBySlackUserName of user Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckSlackUserNameIsAlreadyExists()
        {
            var id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            var result = _userRepository.FindUserBySlackUserName(StringConstant.RawFirstNameForTest);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test case to check method IsAdmin of user repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task IsAdmin()
        {
            var id = await _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest);
            var result = await _userRepository.IsAdmin(_testUser.UserName);
            Assert.Equal(false, result);
        }

        #endregion

        private UserAc _testUser = new UserAc()
        {
            Email = StringConstant.RawEmailIdForTest,
            FirstName = StringConstant.RawFirstNameForTest,
            LastName = StringConstant.RawLastNameForTest,
            IsActive = true,
            UserName = StringConstant.RawEmailIdForTest,
            SlackUserName = StringConstant.RawFirstNameForTest,
            JoiningDate = DateTime.UtcNow,
            RoleName = StringConstant.Employee
        };

        private UserAc userLocal = new UserAc()
        {
            Email = StringConstant.Email,
            FirstName = StringConstant.RawFirstNameForTest,
            LastName = StringConstant.RawLastNameForTest,
            IsActive = true,
            UserName = StringConstant.Email,
            SlackUserName = StringConstant.RawFirstNameForTest,
            JoiningDate = DateTime.UtcNow,
            RoleName = StringConstant.Admin
        };
    }
}
