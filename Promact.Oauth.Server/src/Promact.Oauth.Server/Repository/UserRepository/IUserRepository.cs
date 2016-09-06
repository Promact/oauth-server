using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Promact.Oauth.Server.Repository
{
    public interface IUserRepository
    {
        /// <summary>
        /// This method is used for adding user and return its id
        /// </summary>
        /// <param name="applicationUser">UserAc Application class object</param>
        Task<string> AddUser(UserAc newUser, string createdBy);


        /// <summary>
        /// This method used for get user detail by user id 
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>UserAc Application class object</returns>
        Task<UserAc> GetById(string id);


        /// <summary>
        /// This method used for update user and return its id
        /// </summary>
        /// <param name="editedUser">UserAc Application class object</param>
        string UpdateUserDetails(UserAc editedUser, string updatedBy);


        /// <summary>
        /// This method used forget list of users
        /// </summary>
        /// <returns>List of all users</returns>
        Task<IEnumerable<UserAc>> GetAllUsers();

        /// <summary>
        /// This method used for get role list. 
        /// </summary>
        /// <returns></returns>
        List<RolesAc> GetRoles();


        /// <summary>
        /// This method is used for changing the password of an user
        /// </summary>
        /// <param name="passwordModel">ChangePasswordViewModel type object</param>
        string ChangePassword(ChangePasswordViewModel passwordModel);


        /// <summary>
        /// This method finds if a user already exists with the specified UserName
        /// </summary>
        /// <param name="userName">string userName</param>
        /// <returns> boolean: true if the user name exists, false if does not exist</returns>
        bool FindByUserName(string userName);


        /// <summary>
        /// This method finds if a user already exists with the specified Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns> boolean: true if the email exists, false if does not exist</returns>
        bool FindByEmail(string email);

        bool FindUserBySlackUserName(string slackUserName);

        /// <summary>
        /// This method is used to send email to the currently added user
        /// </summary>
        /// <param name="user">Object of newly registered User</param>
        void SendEmail(ApplicationUser user);

        /// <summary>
        /// Method is used to Get User details by firstname
        /// </summary>
        /// <param name="firstname"></param>
        /// <returns></returns>
        ApplicationUser UserDetialByFirstName(string firstname);

        /// <summary>
        /// Method is used to get team leader's list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ApplicationUser>> TeamLeaderByUserId(string userId);

        /// <summary>
        /// Method to get list of management people
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ApplicationUser>> ManagementByUserId();

        /// <summary>
        /// Used to fetch the userdetail by given UserName 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns>object of UserAc</returns>
        UserAc GetUserDetail(string UserName);

        /// <summary>
        /// Method is used to Get User details by Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ApplicationUser UserDetailById(string employeeId);

    }
}
