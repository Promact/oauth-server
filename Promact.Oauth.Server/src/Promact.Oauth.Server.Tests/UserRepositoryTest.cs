﻿//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Promact.Oauth.Server.Constants;
//using Promact.Oauth.Server.Data_Repository;
//using Promact.Oauth.Server.Models;
//using Promact.Oauth.Server.Models.ApplicationClasses;
//using Promact.Oauth.Server.Models.ManageViewModels;
//using Promact.Oauth.Server.Repository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace Promact.Oauth.Server.Tests
//{
//    public class UserRepositoryTest : BaseProvider
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;

//        public UserRepositoryTest() : base()
//        {
//            _userRepository = serviceProvider.GetService<IUserRepository>();
//            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
//            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//        }

//        #region Test Case

//        ///// <summary>
//        ///// This test case gets the list of all users
//        ///// </summary>
//        //[Fact, Trait("Category", "Required")]
//        //public void GetAllUser()
//        //{
//        //    AddRole().Wait();
//        //    var id = _userRepository.AddUser(_testUser, "Rajdeep").Result;
//        //    IEnumerable<UserAc> users = _userRepository.GetAllUsers().Result;
//        //    Assert.NotNull(users);
//        //}

//        ///// <summary>
//        ///// This test case gets the user by its id
//        ///// </summary>
//        //[Fact, Trait("Category", "Required")]
//        //public void GetUserById()
//        //{

//        //    UserAc user = new UserAc()
//        //    {
//        //        Email = "testUser2@promactinfo.com",
//        //        FirstName = "First name 2",
//        //        LastName = "Last name 2",
//        //        IsActive = true,
//        //        Password = "User@123",
//        //        UserName = "testUser2@promactinfo.com",
//        //        SlackUserName = "test",
//        //        RoleName = StringConstant.Employee
//        //    };
//        //    AddRole().Wait();
//        //    var id = _userRepository.AddUser(user, "Rajdeep").Result;
//        //    UserAc testUser = _userRepository.GetById(id).Result;

//        //    Assert.NotNull(testUser);
//        //}

//        ///// <summary>
//        ///// This test case checks if a user exists with the specified Email
//        ///// </summary>
//        //[Fact, Trait("Category", "Required")]
//        //public void FindByEmail()
//        //{
//        //    AddRole().Wait();
//        //    var result = _userRepository.AddUser(_testUser, "Rajdeep").Result;
//        //    var exists = _userRepository.FindByEmail("testUser@promactinfo.com");
//        //    Assert.Equal(true, exists);
//        //}

//        ///// <summary>
//        ///// This test case checks if a user exists with the specified UserName
//        ///// </summary>
//        //[Fact, Trait("Category", "Required")]
//        //public void FindByUserName()
//        //{
//        //    AddRole().Wait();
//        //    var id = _userRepository.AddUser(_testUser, "Rajdeep").Result;
//        //    var exists = _userRepository.FindByUserName("testUser@promactinfo.com");
//        //    Assert.Equal(true, exists);
//        //}

//        /// <summary>
//        /// This test case is used for adding new user
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public async void AddUser()
//        {
//            await AddRole();
//            string id = _userRepository.AddUser(_testUser, StringConstant.CreatedBy).Result;
//            ApplicationUser user = _userManager.FindByIdAsync(id).Result;
//            Assert.NotNull(user);
//        }

//        /// <summary>
//        /// This test case is used for updating user details
//        /// </summary>
//        //[Fact, Trait("Category", "Required")]
//        //public void UpdateUser()
//        //{
//        //    AddRole().Wait();
//        //    //_userRepository.AddUser(_testUser, "Rajdeep");
//        //    var userId = _userRepository.AddUser(_testUser, "Rajdeep").Result;
//        //    var user = _userManager.FindByIdAsync(userId).Result;
//        //    //var user = _dataRepository.FirstOrDefault(u => u.Email == "testUser@promactinfo.com");

//        //    string id = _userRepository.UpdateUserDetails(new UserAc
//        //    {
//        //        Id = user.Id,
//        //        FirstName = "Updated User",
//        //        SlackUserName = "Updated test",
//        //        RoleName = "Employee"
//        //    }, "Rajdeep");

//        //    var editedUser = _userManager.FindByIdAsync(id).Result;
//        //    Assert.Equal("Updated User", editedUser.FirstName);
//        //}

//        /// <summary>
//        /// This test case is used for changing the password of an user
//        /// </summary>
//        //[Fact, Trait("Category", "Required")]
//        //public void ChangePassword()
//        //{
//        //    AddRole().Wait();
//        //    var id = _userRepository.AddUser(_testUser, "Rajdeep").Result;
//        //    var user = _userManager.FindByIdAsync(id).Result;
//        //    //var user = _dataRepository.FirstOrDefault(u => u.Email == "testUser@promactinfo.com");

//        //    var password = _userRepository.ChangePassword(new ChangePasswordViewModel
//        //    {
//        //        OldPassword = "User@123",
//        //        NewPassword = "User@1",
//        //        ConfirmPassword = "User@1",
//        //        Email = user.Email
//        //    });
//        //    var passwordMatch = _userManager.CheckPasswordAsync(user, password).Result;
//        //    Assert.Equal(true, passwordMatch);
//        //}

//        ///// <summary>
//        ///// Test case use for getting user details by its first name
//        ///// </summary>
//        //[Fact, Trait("Category", "Required")]
//        //public void UserDetialByFirstName()
//        //{
//        //    //AddRole();
//        //    string id = _userRepository.AddUser(_testUser, "Siddhartha");
//        //    var user = _userRepository.UserDetialByFirstName("First name");
//        //    Assert.Equal(user.Email, _testUser.Email);
//        //}

//        ///// <summary>
//        ///// Test case use for getting team leader's details by users first name
//        ///// </summary>
//        //[Fact, Trait("Category", "Required")]
//        //public void TeamLeaderByUserId()
//        //{
//        //    AddRole().Wait();
//        //    string id = _userRepository.AddUser(_testUser, "Siddhartha").Result;
//        //    var user = _userRepository.TeamLeaderByUserId("First name").Result;
//        //    Assert.Equal(0, user.Count);
//        //}

       
//        ///// <summary>
//        ///// Test case use for getting management's details by users first name
//        ///// </summary>
//        //[Fact, Trait("Category", "Required")]
//        //public void ManagementByUserId()
//        //{
//        //    AddRole().Wait();
//        //    string id = _userRepository.AddUser(_testUser, "Siddhartha").Result;
//        //    var user = _userRepository.ManagementByUserId().Result;
//        //    Assert.Equal(0, user.Count);
//        //}

//        #endregion

//        private async Task AddRole()
//        {
//            if (!_roleManager.Roles.Any())
//            {
//                List<IdentityRole> roles = new List<IdentityRole>();
//                roles.Add(new IdentityRole { Name = StringConstant.Employee, NormalizedName = StringConstant.NormalizedName });
//                roles.Add(new IdentityRole { Name = StringConstant.Admin, NormalizedName = StringConstant.NormalizedSecond });

//                foreach (var role in roles)
//                {
//                    var roleExit = await _roleManager.RoleExistsAsync(role.Name);
//                    if (!roleExit)
//                    {
//                        var result = await _roleManager.CreateAsync(role);
//                    }
//                }
//            }
//        }

//        private UserAc _testUser = new UserAc()
//        {
//            Email = "testUser@promactinfo.com",
//                FirstName = "First name",
//                LastName = "Last name",
//                IsActive = true,
//                Password = "User@123",
//                UserName = "testUser@pronactinfo.com",
//                SlackUserName = "test",
//                JoiningDate = DateTime.UtcNow,
//                RoleName = StringConstant.Employee
//            };
//    }
//}
