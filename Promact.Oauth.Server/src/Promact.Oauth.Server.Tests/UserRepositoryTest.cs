using Microsoft.AspNetCore.Identity;
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
using Promact.Oauth.Server.Constants;

namespace Promact.Oauth.Server.Tests
{
    public class UserRepositoryTest : BaseProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly IDataRepository<ApplicationUser> _dataRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserAc _testUser;
        private readonly PromactOauthDbContext context;

        public UserRepositoryTest() : base()
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _dataRepository = serviceProvider.GetService<IDataRepository<ApplicationUser>>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            context = serviceProvider.GetService<PromactOauthDbContext>();
            _testUser = new UserAc()
            {
                Email = StringConstant.EmailUser,
                FirstName = StringConstant.FirstName,
                LastName = StringConstant.LastName,
                IsActive = StringConstant.IsActive,
                Password = StringConstant.Password,
                UserName = StringConstant.UserName,
                SlackUserName = StringConstant.SlackUserName,
                JoiningDate = DateTime.Now
            };
        }

        #region Test Case

        /// <summary>
        /// This test case gets the list of all users
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetAllUser()
        {
            AddRole();
            _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            Task<IEnumerable<UserAc>> users = _userRepository.GetAllUsers();
            Assert.NotNull(users);
        }

        /// <summary>
        /// This test case gets the user by its id
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetUserById()
        {
            AddRole();
            var id = _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            Task<UserAc> testUser = _userRepository.GetById(id);
            Assert.NotNull(testUser);
        }


        /// <summary>
        /// This test case checks if a user exists with the specified Email
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void FindByEmail()
        {
            AddRole();
            _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            var exists = _userRepository.FindByEmail(StringConstant.EmailUser);

            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case checks if a user exists with the specified UserName
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void FindByUserName()
        {
            AddRole();
            _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            var exists = _userRepository.FindByUserName(StringConstant.EmailUser);

            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case is used for adding new user
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void AddUser()
        {
            AddRole();
            string id = _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            var user = _dataRepository.FirstOrDefault(u => u.Id == id);
            Assert.NotNull(user);
        }

        /// <summary>
        /// This test case is used for updating user details
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void UpdateUser()
        {
            AddRole();
            _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            var user = _dataRepository.FirstOrDefault(u => u.Email == StringConstant.EmailUser);

            string id = _userRepository.UpdateUserDetails(new UserAc
            {
                Id = user.Id,
                FirstName = StringConstant.UpadteFirstName,
                SlackUserName= StringConstant.UpdateSlackUserName
            }, StringConstant.CreatedBy);

            var editedUser = _dataRepository.FirstOrDefault(u => u.Id == id);
            Assert.Equal("Updated User", editedUser.FirstName);
        }

        /// <summary>
        /// This test case is used for changing the password of an user
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void ChangePassword()
        {
            AddRole();
            _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            var user = _dataRepository.FirstOrDefault(u => u.Email == StringConstant.EmailUser);

            var password = _userRepository.ChangePassword(new ChangePasswordViewModel
            {
                OldPassword = StringConstant.Password,
                NewPassword = StringConstant.NewPassword,
                ConfirmPassword = StringConstant.ConfirmPassword,
                Email = user.Email
            });

            var passwordMatch = _userManager.CheckPasswordAsync(user, password).Result;

            Assert.Equal(true, passwordMatch);
        }

        /// <summary>
        /// Test case use for getting user details by its first name
        /// </summary>
        //[Fact, Trait("Category", "Required")]
        //public void UserDetialByFirstName()
        //{
        //    AddRole();
        //    string id = _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
        //    var user = _userRepository.UserDetialByFirstName(StringConstant.FirstName);
        //    Assert.Equal(user.Id, id);
        //}

        /// <summary>
        /// Test case use for getting team leader's details by users first name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task TeamLeaderByUserId()
        {
            AddRole();
            string id = _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            var user = await _userRepository.TeamLeaderByUserId(StringConstant.FirstName);
            Assert.Equal(0, user.Count);
        }

        /// <summary>
        /// Test case use for getting management's details by users first name
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async Task ManagementByUserId()
        {
            AddRole();
            string id = _userRepository.AddUser(_testUser, StringConstant.CreatedBy);
            var user = await _userRepository.ManagementByUserId();
            Assert.Equal(0, user.Count);
        }
        #endregion

        private void AddRole()
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!roleManager.Roles.Any())
            {
                List<IdentityRole> roles = new List<IdentityRole>();
                roles.Add(new IdentityRole { Name = StringConstant.RoleName, NormalizedName = StringConstant.NormalizedName });
                roles.Add(new IdentityRole { Name = StringConstant.RoleNameSecond, NormalizedName = StringConstant.NormalizedSecond });

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
    }
}
