using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Data_Repository;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models.ApplicationClass;

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
            userManager.CreateAsync(user, "User@123").Wait();
            string designation = newUser.Designation.ToString();
            userManager.AddToRoleAsync(user, designation).Wait();
            
        }


        /// <summary>
        /// Gets the list of all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return applicationUserDataRepository.List().ToList();
        }


        /// <summary>
        /// Edites the details of an user
        /// </summary>
        /// <param name="editedUser"></param>
        public void UpdateUserDetails(ApplicationUser editedUser)
        {
            applicationUserDataRepository.Update(editedUser);
        }


        
    }
}
