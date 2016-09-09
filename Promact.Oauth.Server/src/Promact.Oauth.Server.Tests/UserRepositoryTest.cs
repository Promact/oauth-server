using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Promact.Oauth.Server.Constants;
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
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PromactOauthDbContext _db;
        private readonly IMapper _mapperContext;
        public UserRepositoryTest() : base()
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            //_userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            //_roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _db = serviceProvider.GetService<PromactOauthDbContext>();
            _mapperContext = serviceProvider.GetService<IMapper>();
        }

        #region Test Case

        /// <summary>
        /// This test case gets the list of all users
        /// </summary>
        //[Fact, Trait("Category", "Required")]
        //public void GetAllUser()
        //{
        //    AddRole();
        //    var id = _userRepository.AddUser(_testUser, StringConstant.RawFirstNameForTest).Result;
        //    IEnumerable<UserAc> users = _userRepository.GetAllUsers();
        //    Assert.Equal(1, users.Count());
        //}

        //        ///// <summary>
        //        ///// This test case gets the user by its id
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public void GetUserById()
        //{
        //    UserAc user = new UserAc()
        //    {
        //        Email = "testUser2@promactinfo.com",
        //        FirstName = "First name 2",
        //        LastName = "Last name 2",
        //        IsActive = true,
        //        Password = "User@123",
        //        UserName = "testUser2@promactinfo.com",
        //        SlackUserName = "test",
        //        RoleName = StringConstant.Employee
        //    };
        //    AddRole().Wait();
        //    var id = _userRepository.AddUser(user, "Rajdeep").Result;
        //    UserAc testUser = _userRepository.GetById(id).Result;
        //    Assert.NotNull(testUser);
        //}

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
        [Fact, Trait("Category", "Required")]
        public void AddUser()
        {
            var mockApplicationUser = new Mock<UserManager<ApplicationUser>>();
            var user = _mapperContext.Map<UserAc, ApplicationUser>(_testUser);
            mockApplicationUser.Setup(x => x.AddToRoleAsync(user, StringConstant.Employee)).Returns(Task.FromResult(IdentityResult.Success));
            string id = _userRepository.AddUser(_testUser, StringConstant.CreatedBy).Result;
            //ApplicationUser user = _userManager.FindByIdAsync(id).Result;
            Assert.NotNull(id);
        }

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

        ///// <summary>
        ///// Test case use for getting user details by its first name
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public void UserDetail()
        //{
        //    GenerateTestUser();
        //    AddRole();
        //    string id = _userRepository.AddUser(userLocal, "siddhartha");
        //    var user = _userRepository.UserDetialByUserSlackName("myslackname");
        //    Assert.Equal(user.Email, userLocal.Email);
        //}

        ///// <summary>
        ///// Test case use for getting management's details by users first name
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task ManagementDetails()
        //{
        //    GenerateTestUser();
        //    AddRole();
        //    string id = _userRepository.AddUser(_testUser, "Siddhartha");
        //    var user = await _userRepository.TeamLeaderByUserSlackName("test");
        //    Assert.Equal(0, user.Count);
        //}

        ///// <summary>
        ///// Test case to get user's number of casual leave
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public async Task ManagementDetails()
        //{
        //    GenerateTestUser();
        //    AddRole();
        //    string id = _userRepository.AddUser(_testUser, "Siddhartha");
        //    var user = await _userRepository.ManagementDetails();
        //    Assert.Equal(0, user.Count);
        //}

        ///// <summary>
        ///// Test case to get user's number of casual leave
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public void GetUserCasualLeaveBySlackName()
        //{
        //    GenerateTestUser();
        //    AddRole();
        //    var id = _userRepository.AddUser(userLocal, "Siddhartha");
        //    var casualLeave = _userRepository.GetUserCasualLeaveBySlackName(userLocal.SlackUserName);
        //    Assert.Equal(8,casualLeave);
        //}

        ///// <summary>
        ///// Test case to get user's number of casual leave
        ///// </summary>
        //[Fact, Trait("Category", "Required")]
        //public void GetUserCasualLeaveBySlackName()
        //{
        //    GenerateTestUser();
        //    AddRole();
        //    var id = _userRepository.AddUser(userLocal, "Siddhartha");
        //    var casualLeave = _userRepository.GetUserCasualLeaveBySlackName(userLocal.SlackUserName);
        //    Assert.Equal(8,casualLeave);
        //}
        #endregion
        //private void AddRole()
        //{
        //    if(!_db.Roles.Any())
        //    //if (!_roleManager.Roles.Any())
        //    {
        //        List<IdentityRole> roles = new List<IdentityRole>();
        //        roles.Add(new IdentityRole { Name = StringConstant.Employee, NormalizedName = StringConstant.NormalizedName });
        //        roles.Add(new IdentityRole { Name = StringConstant.Admin, NormalizedName = StringConstant.NormalizedSecond });

        //        foreach (var role in roles)
        //        {
        //            //var roleExist = _db.Roles.
        //            //var roleExit = await _roleManager.RoleExistsAsync(role.Name);
        //            //if (!roleExit)
        //            //{
        //            var result = _db.Roles.Add(role);
        //            //}
        //        }
        //        _db.SaveChanges();
        //    }
        //}

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


        //private UserAc userLocal = new UserAc()
        //{
        //    Email = "testing@promactinfo.com",
        //    UserName = "testing@promactinfo.com",
        //    FirstName = "Myfirsttest",
        //    LastName = "testing",
        //    JoiningDate = DateTime.ParseExact("02-09-2016", "dd-MM-yyyy", null),
        //    //JoiningDate = DateTime.UtcNow,
        //    SlackUserName = "myslackname"
        //};
        //~UserRepositoryTest()
        //{
        //    _db.Dispose();
        //}
    }
}
