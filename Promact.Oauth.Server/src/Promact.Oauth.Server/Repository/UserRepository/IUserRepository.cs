﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Models.ManageViewModels;

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
        Task<string> UpdateUserDetails(UserAc editedUser, string updatedBy);


        /// <summary>
        /// This method used forget list of users
        /// </summary>
        /// <returns>List of all users</returns>
        IEnumerable<UserAc> GetAllUsers();

        /// <summary>
        /// This method  get the list of Employees
        /// </summary>
        /// <returns>List of all Employees</returns>
        Task<List<UserAc>> GetAllEmployees();
        /// <summary>
        /// This method used for get role list. 
        /// </summary>
        /// <returns></returns>
        List<RolesAc> GetRoles();


        /// <summary>
        /// This method is used for changing the password of an user
        /// </summary>
        /// <param name="passwordModel">ChangePasswordViewModel type object</param>
        Task<string> ChangePassword(ChangePasswordViewModel passwordModel);


        /// <summary>
        /// This method finds if a user already exists with the specified UserName
        /// </summary>
        /// <param name="userName">string userName</param>
        /// <returns> boolean: true if the user name exists, false if does not exist</returns>
        Task<bool> FindByUserName(string userName);


        /// <summary>
        /// This method finds if a user already exists with the specified Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns> boolean: true if the email exists, false if does not exist</returns>
        Task<bool> CheckEmailIsExists(string email);


        /// <summary>
        /// Fetches user with the given Slack User Id
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns></returns>
        ApplicationUser FindUserBySlackUserId(string slackUserId);

        /// <summary>
        /// This method is used to send email to the currently added user
        /// </summary>
        /// <param name="user">Object of newly registered User</param>
        //void SendEmail(ApplicationUser user);

        /// <summary>
        /// Method is used to Get User details by slack user id
        /// </summary>
        /// <param name="userSlackId"></param>
        /// <returns></returns>
        ApplicationUser UserDetialByUserSlackId(string userSlackId);

        /// <summary>
        /// Method is used to get team leader's list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ApplicationUser>> TeamLeaderByUserSlackId(string userSlackName);

        /// <summary>
        /// Method to get list of management people
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ApplicationUser>> ManagementDetails();

        /// <summary>
        /// Used to fetch the userdetail by given UserName 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns>object of UserAc</returns>
        Task<UserAc> GetUserDetail(string UserName);
       


        /// <summary>
        /// This method is used to Get User details by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>details of user</returns>
        UserAc UserDetailById(string userId);

        /// <summary>
        /// Method is used to get the details of user by using their username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>details of user</returns>
        Task<UserAc> GetUserDetailByUserName(string UserName);

        /// <summary>
        /// Method to get the number of casual leave allowed to a user by slackUserId
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>number of casual leave</returns>
        LeaveAllowed GetUserAllowedLeaveBySlackId(string slackUserId);

        /// <summary>
        /// Method to check whether user is admin or not
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>true or false</returns>
        Task<bool> IsAdmin(string slackUserId);

        /// <summary>
        /// This method used for re -send mail for user credentails
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ReSendMail(string id);


        /// <summary>
        /// Fetches the list of Slack User Details
        /// </summary>
        /// <returns></returns>
        Task<List<SlackUserDetailAc>> GetSlackUserDetails();
     }
}
