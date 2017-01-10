using System.Collections.Generic;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Models.ManageViewModels;

namespace Promact.Oauth.Server.Repository
{
    public interface IUserRepository
    {
        /// <summary>
        /// This method is used to add new user
        /// </summary>
        /// <param name="newUser">Passed userAC object</param>
        /// <param name="createdBy">Passed id of user who has created this user.</param>
        /// <returns>Added user id</returns>
        Task<string> AddUserAsync(UserAc newUser, string createdBy);

        /// <summary>
        /// This method used for get user detail by user id 
        /// </summary>
        /// <param name="id">Passed user id</param>
        /// <returns>UserAc application class object</returns>
        Task<UserAc> GetByIdAsync(string id);

        /// <summary>
        /// This method is used to edit the details of an existing user
        /// </summary>
        /// <param name="editedUser">Passed UserAc object</param>
        /// <param name="updatedBy">Passed id of user who has updated this user.</param>
        /// <returns>Updated user id.</returns>
        Task<string> UpdateUserDetailsAsync(UserAc editedUser, string updatedBy);

        /// <summary>
        /// This method is used for fetching the list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        Task<IEnumerable<UserAc>> GetAllUsersAsync();

        /// <summary>
        /// This method  get the list of Employees
        /// </summary>
        /// <returns>List of all Employees</returns>
        Task<IEnumerable<UserAc>> GetAllEmployeesAsync();

        /// <summary>
        ///This method is used to get all role. -An
        /// </summary>
        /// <returns>List of user roles</returns>
        Task<List<RolesAc>> GetRolesAsync();


        /// <summary>
        /// This method is used to change the password of a particular user. -An
        /// </summary>
        /// <param name="passwordModel">Passed changePasswordViewModel object(OldPassword,NewPassword,ConfirmPassword,Email)</param>
        /// <returns>If password is changed successfully, return empty otherwise error message.</returns>
        Task<ChangePasswordErrorModel> ChangePasswordAsync(ChangePasswordViewModel passwordModel);


        /// <summary>
        /// This method is used to check if a user already exists in the database with the given userName
        /// </summary>
        /// <param name="userName">Passed userName</param>
        /// <returns>boolean: true if the user name exists,otherwise throw UserNotFound exception.</returns>
        Task<bool> FindByUserNameAsync(string userName);


        /// <summary>
        /// This method is used to check email is already exists in database.
        /// </summary>
        /// <param name="email">Passed user email address</param>
        /// <returns> boolean: true if the email exists, false if does not exist</returns>
        Task<bool> CheckEmailIsExistsAsync(string email);


        /// <summary>
        /// Fetch user with given slack user name
        /// </summary>
        /// <param name="slackUserName">Passed slack user name</param>
        /// <returns>If user is exists return user otherwise throw SlackUserNotFound exception.</returns>
        Task<ApplicationUser> FindUserBySlackUserNameAsync(string slackUserName);


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
        Task<List<ApplicationUser>> TeamLeaderByUserSlackIdAsync(string userSlackId);

        /// <summary>
        /// Method to get list of management people
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ApplicationUser>> ManagementDetailsAsync();

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
        LeaveAllowed GetUserAllowedLeaveBySlackId(string slackUserId);

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
        Task ReSendMailAsync(string id);

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
        /// <param name="slackChannelName"></param>
        /// <returns>object of UserAc</returns>
        Task<List<UserAc>> GetProjectUserBySlackChannelNameAsync(string slackChannelName);

        /// <summary>
        /// The method is used to get list of projects along with its users for a specific teamleader 
        /// </summary>
        /// <param name="teamLeaderId"></param>
        /// <returns>list of projects with users for a specific teamleader</returns>
        Task<List<UserAc>> GetProjectUsersByTeamLeaderIdAsync(string teamLeaderId);

    }
}