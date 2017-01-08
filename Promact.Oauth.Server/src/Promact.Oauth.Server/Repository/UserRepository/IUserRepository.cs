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
        /// Fetches user with the given Slack User Name
        /// </summary>
        /// <param name="slackUserName"></param>
        /// <returns></returns>
        ApplicationUser FindUserBySlackUserName(string slackUserName);

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
        Task<ApplicationUser> UserDetialByUserSlackIdAsync(string userSlackId);

        /// <summary>
        /// Method is used to get team leader's list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ApplicationUser>> TeamLeaderByUserSlackIdAsync(string userSlackName);

        /// <summary>
        /// Method to get list of management people
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ApplicationUser>> ManagementDetailsAsync();

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
        Task<UserAc> UserDetailByIdAsync(string userId);

        /// <summary>
        /// Method to get the number of casual leave allowed to a user by slackUserId
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>number of casual leave</returns>
        Task<LeaveAllowed> GetUserAllowedLeaveBySlackIdAsync(string slackUserId);

        /// <summary>
        /// Method to check whether user is admin or not
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>true or false</returns>
        Task<bool> IsAdminAsync(string slackUserId);

        /// <summary>
        /// This method used for re -send mail for user credentails
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ReSendMail(string id);

        /// <summary>
        /// Method to get User Role
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<UserRoleAc>> GetUserRoleAsync(string userId);

        /// <summary>
        /// Method to get list of TeamMember By TeamLeaderId
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<List<UserRoleAc>> GetTeamMembersAsync(string userId);

        /// <summary>
        /// This method is used to fetch list of users/employees of the given group name. - JJ
        /// </summary>
        /// <param name="GroupName"></param>
        /// <returns>object of UserAc</returns>
        Task<List<UserAc>> GetProjectUserByGroupNameAsync(string GroupName);

        /// <summary>
        /// The method is used to get list of projects along with its users for a specific teamleader 
        /// </summary>
        /// <param name="teamLeaderId"></param>
        /// <returns>list of projects with users for a specific teamleader</returns>
        Task<List<UserAc>> GetProjectUsersByTeamLeaderIdAsync(string teamLeaderId);

    }
}