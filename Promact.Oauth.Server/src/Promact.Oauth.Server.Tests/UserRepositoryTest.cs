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
        [Fact, Trait("Category", "Required")]
        public void GetAllUser()
        {
            _userRepository.AddUser(testUser, "Rajdeep");
            var users = _userRepository.GetAllUsers();
            Assert.NotNull(users);
        }

        /// <summary>
        /// This test case gets the user by its id
        /// </summary>
        //[Fact, Trait("Category", "Required")]
        //public void GetUserById()
        //{

        //    UserAc user = new UserAc()
        //    {
        //        Email = "testUser@promactinfo.com",
        //        FirstName = "First namefgs",
        //        LastName = "Last namegfdsgs",
        //        IsActive = true,
        //        Password = "User@123fgs",
        //        UserName = "testUser@pronactinfgsdfo.com"
        //    };
        //    var id = _userRepository.AddUser(user, "Rajdeep");
        //    var testUser = _userRepository.GetById(id);

        //    Assert.NotNull(testUser);
        //}


        /// <summary>
        /// This test case checks if a user exists with the specified Email
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void FindByEmail()
        {
            _userRepository.AddUser(testUser, "Rajdeep");
            var exists = _userRepository.FindByEmail("testUser@promactinfo.com");

            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case checks if a user exists with the specified UserName
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void FindByUserName()
        {
            _userRepository.AddUser(testUser, "Rajdeep");
            var exists = _userRepository.FindByUserName("testUser@promactinfo.com");

            Assert.Equal(true, exists);
        }

        /// <summary>
        /// This test case is used for adding new user
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void AddUser()
        {
            string id = _userRepository.AddUser(testUser, "Rajdeep");
            var user = _dataRepository.FirstOrDefault(u => u.Id == id);
            Assert.NotNull(user);
        }

        /// <summary>
        /// This test case is used for updating user details
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void UpdateUser()
        {
            _userRepository.AddUser(testUser, "Rajdeep");
            var user = _dataRepository.FirstOrDefault(u => u.Email == "testUser@promactinfo.com");

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
        [Fact, Trait("Category", "Required")]
        public void ChangePassword()
        {
            _userRepository.AddUser(testUser, "Rajdeep");
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


        #endregion



        #region "Test User"

        UserAc testUser = new UserAc()
        {
            Email = "testUser@promactinfo.com",
            FirstName = "First name",
            LastName = "Last name",
            IsActive = true,
            Password = "User@123",
            UserName = "testUser@pronactinfo.com"
        };

        #endregion
    }
}
