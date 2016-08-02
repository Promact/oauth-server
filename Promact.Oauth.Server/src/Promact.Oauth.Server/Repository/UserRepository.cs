using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Data_Repository;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models.ApplicationClass;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Promact.Oauth.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private IDataRepository<ApplicationUser> applicationUserDataRepository;
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public UserRepository(IDataRepository<ApplicationUser> _applicationUserDataRepository, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            applicationUserDataRepository = _applicationUserDataRepository;
            userManager = _userManager;
            signInManager = _signInManager;
        }


        /// <summary>
        /// Registers User
        /// </summary>
        /// <param name="applicationUser"></param>
        public void AddUser(UserModel newUser)
        {
            var user = new ApplicationUser {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                UserName = newUser.Email,
                Status = newUser.Status
            };
            userManager.CreateAsync(user, newUser.Password).Wait();
            
        }


        /// <summary>
        /// Gets the list of all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserModel> GetAllUsers()
        {
            var users = applicationUserDataRepository.List().ToList();
            var userList = new List<UserModel>();
            foreach (var user in users)
            {
                var list = new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Status = user.Status
                };
                userList.Add(list);
            }
            return userList;

        }


        /// <summary>
        /// Edites the details of an user
        /// </summary>
        /// <param name="editedUser"></param>
        public void UpdateUserDetails(UserModel editedUser)
        {
            var user = new ApplicationUser
            {
                FirstName = editedUser.FirstName,
                LastName = editedUser.LastName,
                Email = editedUser.Email,
                UserName = editedUser.Email,
                Status = editedUser.Status
            };
            //applicationUserDataRepository.Update(user);
            userManager.UpdateAsync(user).Wait();
            applicationUserDataRepository.Save();
        }

        

        /// <summary>
        /// Get Specific User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserModel GetById(string id)
        {
            var users = applicationUserDataRepository.List().ToList();
            foreach (var user in users)
            {
                if (user.Id.Equals(id))
                {
                    var requiredUser = new UserModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Status = user.Status,
                        UserName = user.UserName
                    };
                    return requiredUser; 
                }
            }
            return null;
        }
    }
}
