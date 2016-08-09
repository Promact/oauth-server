using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Models.ManageViewModels;

namespace Promact.Oauth.Server.Repository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Registers User
        /// </summary>
        /// <param name="applicationUser">UserAc Application class object</param>
        void AddUser(UserAc newUser, string createdBy);


        /// <summary>
        /// Get Specific User Details By Id
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>UserAc Application class object</returns>
        UserAc GetById(string id);


        /// <summary>
        /// Edites the details of an user
        /// </summary>
        /// <param name="editedUser">UserAc Application class object</param>
        void UpdateUserDetails(UserAc editedUser, string updatedBy);


        /// <summary>
        /// Gets the list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        IEnumerable<UserAc> GetAllUsers();


        /// <summary>
        /// Changes the password of a user
        /// </summary>
        /// <param name="passwordModel">ChangePasswordViewModel type object</param>
        void ChangePassword(ChangePasswordViewModel passwordModel);


        /// <summary>
        /// Finds if a particular user name exists in the database
        /// </summary>
        /// <param name="userName">string userName</param>
        /// <returns> boolean: true if the user name exists, false if does not exist</returns>
        bool FindByUserName(string userName);


        /// <summary>
        /// Finds if a particular email exists in the database
        /// </summary>
        /// <param name="email"></param>
        /// <returns> boolean: true if the email exists, false if does not exist</returns>
        bool FindByEmail(string email);


        /// <summary>
        /// Calls the SendEmailAsync method of MessageServices class for sending email to the newly registered user
        /// </summary>
        /// <param name="user">Object of newly registered User</param>
        void SendEmail(ApplicationUser user);
    }
}
