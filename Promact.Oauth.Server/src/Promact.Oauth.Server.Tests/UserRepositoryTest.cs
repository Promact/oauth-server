﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
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
        private readonly IStringConstant _stringConstant;
        public UserRepositoryTest() : base()
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _mapper = serviceProvider.GetService<IMapper>();
            _stringConstant = serviceProvider.GetService<IStringConstant>();
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
            var id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
            Assert.NotEqual(string.Empty, id);
        }

        ///// <summary>
        ///// This test case gets the user by its id
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task GetUserById()
        //{
        //    UserAc _testUser = new UserAc()
        //    {
        //        Email = _stringConstant.RawEmailIdForTest,
        //        FirstName = _stringConstant.RawFirstNameForTest,
        //        LastName = _stringConstant.RawLastNameForTest,
        //        IsActive = true,
        //        UserName = _stringConstant.RawEmailIdForTest,
        //        SlackUserName = _stringConstant.RawFirstNameForTest,
        //        JoiningDate = DateTime.UtcNow,
        //        RoleName = _stringConstant.Employee
        //    };
        //    var id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
        //    UserAc testUser = await _userRepository.GetById(id);
        //    Assert.Equal(testUser.Email, _stringConstant.RawEmailIdForTest);
        //}

        /// <summary>
        /// This test case checks if a user exists with the specified Email
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckEmailIsExists()
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
            var result = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
            var exists = await _userRepository.CheckEmailIsExists(_stringConstant.RawEmailIdForTest);
            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case checks if a user exists with the specified UserName
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
            var id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
            var exists = await _userRepository.FindByUserName(_stringConstant.RawEmailIdForTest);
            Assert.Equal(true, exists);
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
            string id = await _userRepository.AddUser(_testUser, _stringConstant.CreatedBy);
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
            var userId = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
            var user = await _userManager.FindByIdAsync(userId);
            var newUser = _mapper.Map<ApplicationUser, UserAc>(user);
            newUser.RoleName = _stringConstant.Employee;
            newUser.FirstName = _stringConstant.FirstName;
            newUser.SlackUserName = _stringConstant.FirstName;
            string id = await _userRepository.UpdateUserDetails(newUser, _stringConstant.RawFirstNameForTest);
            var editedUser = _userManager.FindByIdAsync(id).Result;
            Assert.Equal(_stringConstant.FirstName, editedUser.FirstName);
        }

        ///// <summary>
        ///// This test case is used for changing the password of an user
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task ChangePassword()
        //{
        //    var id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
        //    var user = await _userManager.FindByIdAsync(id);


        //    var password = await _userRepository.ChangePassword(new ChangePasswordViewModel
        //    {
        //        OldPassword = _stringConstant.OldPassword,
        //        NewPassword = _stringConstant.NewPassword,
        //        ConfirmPassword = _stringConstant.NewPassword,
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
            string id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
            var user = _userRepository.UserDetailById(id);
            Assert.Equal(user.Email, _testUser.Email);
        }

        /// <summary>
        /// Test case use for getting TeamLeader's details by users slack name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task TeamLeaderByUserSlackName()
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
            string id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
            var user = await _userRepository.TeamLeaderByUserSlackId(_stringConstant.RawFirstNameForTest);
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
            string id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
            id = await _userRepository.AddUser(userLocal, _stringConstant.RawFirstNameForTest);
            var user = await _userRepository.ManagementDetails();
            Assert.Equal(1, user.Count);
        }

        ///// <summary>
        ///// Test case to get user's number of casual leave
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public void GetUserCasualLeaveBySlackName()
        //{
        //    UserAc userLocal = new UserAc()
        //    {
        //        Email = _stringConstant.Email,
        //        FirstName = _stringConstant.RawFirstNameForTest,
        //        LastName = _stringConstant.RawLastNameForTest,
        //        IsActive = true,
        //        UserName = _stringConstant.Email,
        //        SlackUserId = _stringConstant.RawFirstNameForTest,
        //        SlackUserName = _stringConstant.RawFirstNameForTest,
        //        JoiningDate = DateTime.UtcNow,
        //        RoleName = _stringConstant.Admin
        //    };
        //    var id = _userRepository.AddUser(userLocal, _stringConstant.RawFirstNameForTest);
        //    var casualLeave = _userRepository.GetUserAllowedLeaveBySlackId(userLocal.SlackUserId);
        //    Assert.NotNull(casualLeave);
        //}

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
            var user = await _userRepository.GetUserDetail(_stringConstant.RawEmailIdForTest);
            Assert.Equal(id, user.Id);
        }

        /// <summary>
        /// Test case to check FindUserBySlackUserName of user Repository
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task CheckSlackUserNameIsAlreadyExists()
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
            var id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
            var result = _userRepository.FindUserBySlackUserId(_stringConstant.RawFirstNameForTest);
            Assert.NotNull(result);
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
            var id = await _userRepository.AddUser(_testUser, _stringConstant.RawFirstNameForTest);
            var result = await _userRepository.IsAdmin(_testUser.SlackUserId);
            Assert.Equal(false, result);
        }

        #endregion




    }
}
