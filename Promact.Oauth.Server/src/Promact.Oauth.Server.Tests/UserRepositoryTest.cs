﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Data_Repository;
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
        private readonly IDataRepository<ApplicationUser> _dataRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private UserAc _testUser;
        private readonly PromactOauthDbContext context;

        public UserRepositoryTest() : base()
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _dataRepository = serviceProvider.GetService<IDataRepository<ApplicationUser>>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            context = serviceProvider.GetService<PromactOauthDbContext>();
            
        }

        public void GenerateTestUser()
        {
            _testUser = new UserAc()
            {
                Email = "testUser@promactinfo.com",
                FirstName = "First",
                LastName = "Last name",
                IsActive = true,
                Password = "User@123",
                UserName = "testUser@pronactinfo.com",
                SlackUserName = "test",
                JoiningDate = DateTime.Now,
            };
        }

        #region Test Case

        /// <summary>
        /// This test case gets the list of all users
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetAllUser()
        {
            GenerateTestUser();
            AddRole();
            _userRepository.AddUser(_testUser, "Rajdeep");
            Task<IEnumerable<UserAc>> users = _userRepository.GetAllUsers();
            Assert.NotNull(users);
        }

        /// <summary>
        /// This test case gets the user by its id
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetUserById()
        {
            GenerateTestUser();
            UserAc user = new UserAc()
            {
                Email = "testUser2@promactinfo.com",
                FirstName = "First name 2",
                LastName = "Last name 2",
                IsActive = true,
                Password = "User@123",
                UserName = "testUser2@promactinfo.com",
                SlackUserName="test"
            };
            AddRole();
            var id = _userRepository.AddUser(user, "Rajdeep");
            Task<UserAc> testUser = _userRepository.GetById(id);

            Assert.NotNull(testUser);
        }


        /// <summary>
        /// This test case checks if a user exists with the specified Email
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void FindByEmail()
        {
            GenerateTestUser();
            AddRole();
            _userRepository.AddUser(_testUser, "Rajdeep");
            var exists = _userRepository.FindByEmail("testUser@promactinfo.com");

            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case checks if a user exists with the specified UserName
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void FindByUserName()
        {
            GenerateTestUser();
            AddRole();
            _userRepository.AddUser(_testUser, "Rajdeep");
            var exists = _userRepository.FindByUserName("testUser@promactinfo.com");

            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case is used for adding new user
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void AddUser()
        {
            GenerateTestUser();
            AddRole();
            string id = _userRepository.AddUser(_testUser, "Rajdeep");
            var user = _dataRepository.FirstOrDefault(u => u.Id == id);
            Assert.NotNull(user);
        }

        /// <summary>
        /// This test case is used for updating user details
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void UpdateUser()
        {
            GenerateTestUser();
            AddRole();
            _userRepository.AddUser(_testUser, "Rajdeep");
            var user = _dataRepository.FirstOrDefault(u => u.Email == "testUser@promactinfo.com");

            string id = _userRepository.UpdateUserDetails(new UserAc
            {
                Id = user.Id,
                FirstName = "Updated User",
                SlackUserName= "Updated test"
            }, "Rajdeep");

            var editedUser = _dataRepository.FirstOrDefault(u => u.Id == id);
            Assert.Equal("Updated User", editedUser.FirstName);
        }

        /// <summary>
        /// This test case is used for changing the password of an user
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void ChangePassword()
        {
            GenerateTestUser();
            AddRole();
            _userRepository.AddUser(_testUser, "Rajdeep");
            var user = _dataRepository.FirstOrDefault(u => u.Email == "testUser@promactinfo.com");

            var password = _userRepository.ChangePassword(new ChangePasswordViewModel
            {
                OldPassword = "User@123",
                NewPassword = "User@1",
                ConfirmPassword = "User@1",
                Email = user.Email
            });

            var passwordMatch = _userManager.CheckPasswordAsync(user, password).Result;

            Assert.Equal(true, passwordMatch);
        }

        /// <summary>
        /// Test case use for getting user details by its first name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void UserDetail()
        {
            AddRole();
            string id = _userRepository.AddUser(userLocal, "siddhartha");
            var user = _userRepository.UserDetialByUserSlackName("myslackname");
            Assert.Equal(user.Email, userLocal.Email);
        }

        /// <summary>
        /// Test case use for getting team leader's details by users first name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task TeamLeaderByUserId()
        {
            GenerateTestUser();
            AddRole();
            string id = _userRepository.AddUser(_testUser, "Siddhartha");
            var user = await _userRepository.TeamLeaderByUserSlackName("test");
            Assert.Equal(0, user.Count);
        }

        /// <summary>
        /// Test case use for getting management's details by users first name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ManagementDetails()
        {
            GenerateTestUser();
            AddRole();
            string id = _userRepository.AddUser(_testUser, "Siddhartha");
            var user = await _userRepository.ManagementDetails();
            Assert.Equal(0, user.Count);
        }
        #endregion

        private void AddRole()
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!roleManager.Roles.Any())
            {
                List<IdentityRole> roles = new List<IdentityRole>();
                roles.Add(new IdentityRole { Name = "Employee", NormalizedName = "EMPLOYEE" });
                roles.Add(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });

                foreach (var role in roles)
                {
                    var roleExit = roleManager.RoleExistsAsync(role.Name).Result;
                    if (!roleExit)
                    {
                        context.Roles.Add(role);
                        context.SaveChanges();
                    }
                }
            }
        }
        private UserAc userLocal = new UserAc()
        {
            Email = "testing@promactinfo.com",
            UserName = "testing@promactinfo.com",
            FirstName = "Myfirsttest",
            LastName = "testing",
            JoiningDate = DateTime.UtcNow,
            SlackUserName = "myslackname"
        };
    }
}
