using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        public UserRepositoryTest() : base()
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _dataRepository = serviceProvider.GetService<IDataRepository<ApplicationUser>>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
        }

        #region Test Case

        /// <summary>
        /// This test case gets the list of all users
        /// </summary>
        [Fact]
        public void GetAllUser()
        {
            AddUser();
            var users = _userRepository.GetAllUsers();
            Assert.NotNull(users);
        }

        /// <summary>
        /// This test case gets the user by its id
        /// </summary>
        [Fact]
        public void GetUserById()
        {
            AddUser();
            string userId = _dataRepository.FirstOrDefault(x => x.Email == "user@promactinfo.com").Id;

            var user = _userRepository.GetById(userId);

            Assert.NotNull(user);
        }


        /// <summary>
        /// This test case checks if a user exists with the specified Email
        /// </summary>
        [Fact]
        public void FindByEmail()
        {
            AddUser();
            var exists = _userRepository.FindByEmail("user@promactinfo.com");

            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case checks if a user exists with the specified UserName
        /// </summary>
        [Fact]
        public void FindByUserName()
        {
            AddUser();
            var exists = _userRepository.FindByUserName("user@promactinfo.com");

            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case is used for adding new user
        /// </summary>
        [Fact]
        public void AddUser()
        {
            string id = _userRepository.AddUser(new UserAc
            {
                FirstName = "User",
                LastName = "User",
                Email = "user@promactinfo.com",
                UserName = "user@promactinfo.com",
                IsActive = true
            }, "Rajdeep"
            );

            var user = _dataRepository.FirstOrDefault(u => u.Id == id);
            Assert.NotNull(user);
        }

        /// <summary>
        /// This test case is used for updating user details
        /// </summary>
        [Fact]
        public void UpdateUser()
        {
            AddUser();
            var user = _dataRepository.FirstOrDefault(u => u.Email == "user@promactinfo.com");

            string id = _userRepository.UpdateUserDetails(new UserAc
            {
                Id = user.Id,
                FirstName = "Updated User"
            }, "Rajdeep");

            var editedUser = _dataRepository.FirstOrDefault(u => u.Id == id);
            Assert.Equal("Updated User", editedUser.FirstName);
        }

        /// <summary>
        /// This test case is used for changing the password of an user
        /// </summary>
        [Fact]
        public void ChangePassword()
        {
            AddUser();
            var user = _dataRepository.FirstOrDefault(u => u.Email == "user@promactinfo.com");

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


        #endregion
    }
}
