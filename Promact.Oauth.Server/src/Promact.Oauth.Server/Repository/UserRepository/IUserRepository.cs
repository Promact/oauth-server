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
        /// This method is used to delete the user.
        /// </summary>
        /// <param name="id">passed userId</param>
        /// <returns></returns>
        Task<string> DeleteUserAsync(string id);

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
        /// This method is used to check email is already exists in database.
        /// </summary>
        /// <param name="email">Passed user email address</param>
        /// <returns> boolean: true if the email exists, false if does not exist</returns>
        Task<bool> CheckEmailIsExistsAsync(string email);

        /// <summary>
        /// Method is used to Get User details by slack user id - SD
        /// </summary>
        /// <param name="userSlackId"></param>
        /// <returns></returns>
        Task<UserAc> UserBasicDetialByUserIdAsync(string userId);


        /// <summary>
        /// Method is used to get team leader's list - SD
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserAc>> ListOfTeamLeaderByUserIdAsync(string userId);


        /// <summary>
        /// Method to get list of management people - SD
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserAc>> ListOfManagementDetailsAsync();


        /// <summary>
        /// This method is used to Get User details by Id - GA
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>details of user</returns>
        Task<UserAc> UserDetailByIdAsync(string userId);


        /// <summary>
        /// Method to get the number of casual leave allowed to a user by slackUserId - SD
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>number of casual leave</returns>
        Task<LeaveAllowed> GetUserAllowedLeaveByUserIdAsync(string userId);


        /// <summary>
        /// Method to check whether user is admin or not - SD
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>true or false</returns>
        Task<bool> IsAdminAsync(string userId);


        /// <summary>
        /// This method used for re -send mail for user credentails -An
        /// </summary>
        /// <param name="id">Passed user id.</param>
        /// <returns></returns>
        Task ReSendMailAsync(string id);


        /// <summary>
        /// Method to return user/users infromation. - RS
        /// </summary>
        /// <param name="userId">passed user id</param>
        /// <returns>users/user information</returns>
        Task<List<UserRoleAc>> GetUserRoleAsync(string userId);


        /// <summary>
        /// Method to return list of teamMembers. - RS
        /// </summary>
        /// <param name="userId">Passed user id</param>
        /// <returns>teamMembers information</returns>
        Task<List<UserRoleAc>> GetTeamMembersAsync(string userId);

        /// <summary>
        /// The method is used to get list of projects along with its users for a specific teamleader - GA
        /// </summary>
        /// <param name="teamLeaderId">Id of the teamleader</param>
        /// <returns>list of projects with users for a specific teamleader</returns>
        Task<List<UserAc>> GetProjectUsersByTeamLeaderIdAsync(string teamLeaderId);

        /// <summary>
        /// This method used for get list of user emails based on role. -an
        /// </summary>
        /// <returns>list of teamleader ,managment and employee email</returns>
        Task<UserEmailListAc> GetUserEmailListBasedOnRoleAsync();

    }
}