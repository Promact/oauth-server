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
using Promact.Oauth.Server.Services;

namespace Promact.Oauth.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private IDataRepository<ApplicationUser> _applicationUserDataRepository;
        private UserManager<ApplicationUser> _userManager;
        private IEmailSender _emailSender;

        public UserRepository(IDataRepository<ApplicationUser> applicationUserDataRepository, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _applicationUserDataRepository = applicationUserDataRepository;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        /// <summary>
        /// Creates an entry of the user to the database 
        /// </summary>
        /// <param name="applicationUser">UserAc Application class object</param>
        public void AddUser(UserAc newUser)
        {
            // Create an ApplicationUser type object from UserAc application class onject
            var user = new ApplicationUser
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                UserName = newUser.Email,
                IsActive = newUser.IsActive
            };
            _userManager.CreateAsync(user, "User@123").Wait();
            SendEmail(user);
        }


        /// <summary>
        /// Gets the list of all users in the database
        /// </summary>
        /// <returns>List of all users</returns>
        public IEnumerable<UserAc> GetAllUsers()
        {
            var users = _applicationUserDataRepository.List().ToList();
            var userList = new List<UserAc>();
            foreach (var user in users)
            {
                // Create a list of UserAc application class object from ApplicationUser type object
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
        /// Edits the details of an user of the database
        /// </summary>
        /// <param name="editedUser">UserAc Application class object</param>
        public void UpdateUserDetails(UserAc editedUser)
        {
            // Fetch the user with particular Id and save the updated data
            var user = _userManager.FindByIdAsync(editedUser.Id).Result;
            user.FirstName = editedUser.FirstName;
            user.LastName = editedUser.LastName;
            user.Email = editedUser.Email;
            user.IsActive = editedUser.IsActive;
            var a = _userManager.UpdateAsync(user).Result;
            _applicationUserDataRepository.Save();
        }



        /// <summary>
        /// Fetches the details of the user with the specified id
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>UserAc Application class object</returns>
        public UserAc GetById(string id)
        {
            var users = _applicationUserDataRepository.List().ToList();
            foreach (var user in users)
            {
                //Finds the details of an user with the specified Id
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
            if (user!= null)
            {
                _userManager.ChangePasswordAsync(user, passwordModel.OldPassword, passwordModel.NewPassword).Wait();
            }
        }
        
        /// <summary>
        /// Finds if a particular user exists in the database with the specified user name
        /// </summary>
        /// <param name="userName">string userName</param>
        /// <returns> boolean: true if the user name exists, false if does not exist</returns>
        public bool FindByUserName(string userName)
        {
            var user = _userManager.FindByNameAsync(userName).Result;
            if(user == null)
            {
                return false;
            }
            return true;
        }



        /// <summary>
        /// Finds if a particular user exists in the database with the specified email
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


        /// <summary>
        /// Calls the SendEmailAsync method of MessageServices class for sending email to the newly registered user
        /// </summary>
        /// <param name="user">Object of newly registered User</param>
        public void SendEmail(ApplicationUser user)
        {
            //Create a new message for the email with the required content
            var message = "Welcome to Promact Infotech Private Limited \n"
                            + "Email: " + user.Email 
                            + "\n Password: User@123"
                            + "\n Link: ";
            var result = _emailSender.SendEmailAsync(user.Email, "Login Credentials", message);
        }
    }
}
