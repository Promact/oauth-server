using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Data_Repository;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Promact.Oauth.Server.Models.ManageViewModels;

namespace Promact.Oauth.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private IDataRepository<ApplicationUser> _applicationUserDataRepository;
        private UserManager<ApplicationUser> _userManager;

        public UserRepository(IDataRepository<ApplicationUser> applicationUserDataRepository, UserManager<ApplicationUser> userManager)
        {
            _applicationUserDataRepository = applicationUserDataRepository;
            _userManager = userManager;
        }


        /// <summary>
        /// Registers User
        /// </summary>
        /// <param name="applicationUser">UserAc Application class object</param>
        public void AddUser(UserAc newUser)
        {
            var user = new ApplicationUser
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                UserName = newUser.UserName,
                IsActive = newUser.IsActive
            };
            _userManager.CreateAsync(user, "User@123").Wait();
        }


        /// <summary>
        /// Gets the list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        public IEnumerable<UserAc> GetAllUsers()
        {
            var users = _applicationUserDataRepository.List().ToList();
            var userList = new List<UserAc>();
            foreach (var user in users)
            {
                var list = new UserAc
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    UserName = user.UserName
                };
                userList.Add(list);
            }
            return userList;

        }


        /// <summary>
        /// Edites the details of an user
        /// </summary>
        /// <param name="editedUser">UserAc Application class object</param>
        public void UpdateUserDetails(UserAc editedUser)
        {
            var user = _userManager.FindByIdAsync(editedUser.Id).Result;
            user.FirstName = editedUser.FirstName;
            user.LastName = editedUser.LastName;
            user.Email = editedUser.Email;
            user.IsActive = editedUser.IsActive;
            var a = _userManager.UpdateAsync(user).Result;
            _applicationUserDataRepository.Save();
        }



        /// <summary>
        /// Get Specific User Details By Id
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>UserAc Application class object</returns>
        public UserAc GetById(string id)
        {
            var users = _applicationUserDataRepository.List().ToList();
            foreach (var user in users)
            {
                if (user.Id.Equals(id))
                {
                    var requiredUser = new UserAc
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        IsActive = user.IsActive,
                        UserName = user.UserName
                    };
                    return requiredUser;
                }
            }
            return null;
        }


        /// <summary>
        /// Changes the password of a user
        /// </summary>
        /// <param name="passwordModel">ChangePasswordViewModel type object</param>
        public void ChangePassword(ChangePasswordViewModel passwordModel)
        {
            var user = _userManager.FindByEmailAsync(passwordModel.Email).Result;
            if(user!= null)
            {
                _userManager.ChangePasswordAsync(user, passwordModel.OldPassword, passwordModel.NewPassword).Wait();
            }
        }


        /// <summary>
        /// Finds if a particular user name exists in the database
        /// </summary>
        /// <param name="userName">string userName</param>
        /// <returns> boolean: true if the user name exists, false if does not exist</returns>
        public bool FindByUserName(string userName)
        {
            var user = _userManager.FindByNameAsync(userName).Result;
            if(user == null || userName.Equals(user.UserName))
            {
                return false;
            }
            return true;
        }



        /// <summary>
        /// Finds if a particular email exists in the database
        /// </summary>
        /// <param name="email"></param>
        /// <returns> boolean: true if the email exists, false if does not exist</returns>
        public bool FindByEmail(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if(user == null)
            {
                return false;
            }
            return true;
        }
    }
}
