//using Microsoft.AspNetCore.Identity;
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
//using Xunit;

//namespace Promact.Oauth.Server.Tests
//{
//    public class UserRepositoryTest : BaseProvider
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly IDataRepository<ApplicationUser> _dataRepository;
//        private readonly UserManager<ApplicationUser> _userManager;

//        private readonly UserAc _testUser;

//        public UserRepositoryTest() : base()
//        {
//            _userRepository = serviceProvider.GetService<IUserRepository>();
//            _dataRepository = serviceProvider.GetService<IDataRepository<ApplicationUser>>();
//            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

//            _testUser = new UserAc()
//            {
//                Email = "testUser@promactinfo.com",
//                FirstName = "First name",
//                LastName = "Last name",
//                IsActive = true,
//                Password = "User@123",
//                UserName = "testUser@pronactinfo.com",
//                SlackUserName = "test",
//                JoiningDate = DateTime.Now
//            };
//        }

//        #region Test Case

//        /// <summary>
//        /// This test case gets the list of all users
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public void GetAllUser()
//        {
//            //AddRole();
//            _userRepository.AddUser(_testUser, "Rajdeep");
//            IEnumerable<UserAc> users = _userRepository.GetAllUsers().Result;
//            Assert.NotNull(users);
//        }

//        /// <summary>
//        /// This test case gets the user by its id
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public void GetUserById()
//        {

//            UserAc user = new UserAc()
//            {
//                Email = "testUser2@promactinfo.com",
//                FirstName = "First name 2",
//                LastName = "Last name 2",
//                IsActive = true,
//                Password = "User@123",
//                UserName = "testUser2@promactinfo.com",
//                SlackUserName = "test",
//                RoleName = StringConstant.RoleName
//            };
//            //AddRole();
//            var id = _userRepository.AddUser(user, "Rajdeep");
//            UserAc testUser = _userRepository.GetById(id).Result;

//            Assert.NotNull(testUser);
//        }


//        /// <summary>
//        /// This test case checks if a user exists with the specified Email
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public void FindByEmail()
//        {
//            //AddRole();
//            var result = _userRepository.AddUser(_testUser, "Rajdeep");
//            var exists = _userRepository.FindByEmail("testUser@promactinfo.com");
//            Assert.Equal(true, exists);
//        }

//        /// <summary>
//        /// This test case checks if a user exists with the specified UserName
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public void FindByUserName()
//        {
//            //AddRole();
//            _userRepository.AddUser(_testUser, "Rajdeep");
//            var exists = _userRepository.FindByUserName("testUser@promactinfo.com");
//            Assert.Equal(true, exists);
//        }

//        /// <summary>
//        /// This test case is used for adding new user
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public void AddUser()
//        {
//            //AddRole();
//            string id = _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
//            var user = _dataRepository.FirstOrDefault(u => u.Id == id);
//            Assert.NotNull(user);
//        }

//        /// <summary>
//        /// This test case is used for updating user details
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public void UpdateUser()
//        {
//            //AddRole();
//            _userRepository.AddUser(_testUser, "Rajdeep");
//            var user = _dataRepository.FirstOrDefault(u => u.Email == "testUser@promactinfo.com");

//            string id = _userRepository.UpdateUserDetails(new UserAc
//            {
//                Id = user.Id,
//                FirstName = "Updated User",
//                SlackUserName = "Updated test",
//                RoleName = "Employee"
//            }, "Rajdeep");

//            var editedUser = _dataRepository.FirstOrDefault(u => u.Id == id);
//            Assert.Equal("Updated User", editedUser.FirstName);
//        }

//        /// <summary>
//        /// This test case is used for changing the password of an user
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public void ChangePassword()
//        {
//            //AddRole();
//            _userRepository.AddUser(_testUser, "Rajdeep");
//            var user = _dataRepository.FirstOrDefault(u => u.Email == "testUser@promactinfo.com");

//            var password = _userRepository.ChangePassword(new ChangePasswordViewModel
//            {
//                OldPassword = "User@123",
//                NewPassword = "User@1",
//                ConfirmPassword = "User@1",
//                Email = user.Email
//            });
//            var passwordMatch = _userManager.CheckPasswordAsync(user, password).Result;
//            Assert.Equal(true, passwordMatch);
//        }

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

//        /// <summary>
//        /// Test case use for getting team leader's details by users first name
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public void TeamLeaderByUserId()
//        {
//            //AddRole();
//            string id = _userRepository.AddUser(_testUser, "Siddhartha");
//            var user = _userRepository.TeamLeaderByUserId("First name").Result;
//            Assert.Equal(0, user.Count);
//        }

//        /// <summary>
//        /// Test case use for getting management's details by users first name
//        /// </summary>
//        [Fact, Trait("Category", "Required")]
//        public void ManagementByUserId()
//        {
//            // AddRole();
//            string id = _userRepository.AddUser(_testUser, "Siddhartha");
//            var user = _userRepository.ManagementByUserId().Result;
//            Assert.Equal(0, user.Count);
//        }


//        #endregion

//        //private void AddRole()
//        //{
//        //    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//        //    if (!roleManager.Roles.Any())
//        //    {
//        //        List<IdentityRole> roles = new List<IdentityRole>();
//        //        roles.Add(new IdentityRole { Name = StringConstant.RoleName, NormalizedName = StringConstant.NormalizedName });
//        //        roles.Add(new IdentityRole { Name = StringConstant.RoleNameSecond, NormalizedName = StringConstant.NormalizedSecond });

//        //        foreach (var role in roles)
//        //        {
//        //            var roleExit = roleManager.RoleExistsAsync(role.Name).Result;
//        //            if (!roleExit)
//        //            {
//        //                var result = roleManager.CreateAsync(role).Result;
//        //            }
//        //        }
//        //    }
//        //}

//    }
//}
