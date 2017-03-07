using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Models.ManageViewModels;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Services;
using Promact.Oauth.Server.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        #region "Private Variable(s)"



        private readonly IDataRepository<ApplicationUser, PromactOauthDbContext> _applicationUserDataRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapperContext;
        private readonly IDataRepository<ProjectUser, PromactOauthDbContext> _projectUserRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDataRepository<Project, PromactOauthDbContext> _projectDataRepository;
        private readonly IOptions<AppSettingUtil> _appSettingUtil;
        private readonly IStringConstant _stringConstant;
        private readonly IDataRepository<ProjectUser, PromactOauthDbContext> _projectUserDataRepository;
        private readonly IEmailUtil _emailUtil;

        #endregion

        #region "Constructor"

        public UserRepository(IDataRepository<ApplicationUser, PromactOauthDbContext> applicationUserDataRepository,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, IEmailSender emailSender,
            IMapper mapperContext, IDataRepository<ProjectUser, PromactOauthDbContext> projectUserRepository,
            IProjectRepository projectRepository, IOptions<AppSettingUtil> appSettingUtil,
            IDataRepository<Project, PromactOauthDbContext> projectDataRepository,
            IStringConstant stringConstant,
            IDataRepository<ProjectUser, PromactOauthDbContext> projectUserDataRepository, IEmailUtil emailUtil)
        {
            _applicationUserDataRepository = applicationUserDataRepository;
            _userManager = userManager;
            _emailSender = emailSender;
            _mapperContext = mapperContext;
            _projectUserRepository = projectUserRepository;
            _projectRepository = projectRepository;
            _roleManager = roleManager;
            _projectDataRepository = projectDataRepository;
            _appSettingUtil = appSettingUtil;
            _stringConstant = stringConstant;
            _projectUserDataRepository = projectUserDataRepository;
            _emailUtil = emailUtil;
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// This method is used to add new user
        /// </summary>
        /// <param name="newUser">Passed userAC object</param>
        /// <param name="createdBy">Passed id of user who has created this user.</param>
        /// <returns>Added user id</returns>
        public async Task<string> AddUserAsync(UserAc newUser, string createdBy)
        {
            LeaveAllowed leaveAllowed = CalculateAllowedLeaves(Convert.ToDateTime(newUser.JoiningDate));
            newUser.NumberOfCasualLeave = leaveAllowed.CasualLeave;
            newUser.NumberOfSickLeave = leaveAllowed.SickLeave;
            var user = _mapperContext.Map<UserAc, ApplicationUser>(newUser);
            user.UserName = user.Email;
            user.CreatedBy = createdBy;
            user.CreatedDateTime = DateTime.UtcNow;
            string password = GetRandomString();//get readom password.
            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, newUser.RoleName);//add role of new user.
            SendEmail(user.FirstName, user.Email, password);//send mail with generated password of new user. 
            return user.Id;
        }

        /// <summary>
        ///This method is used to get all role. -An
        /// </summary>
        /// <returns>List of user roles</returns>
        public async Task<List<RolesAc>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return _mapperContext.Map<List<IdentityRole>, List<RolesAc>>(roles);
        }

        /// <summary>
        /// This method is used for fetching the list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        public async Task<IEnumerable<UserAc>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.OrderByDescending(x => x.CreatedDateTime).ToListAsync();
            return _mapperContext.Map<IEnumerable<ApplicationUser>, IEnumerable<UserAc>>(users);
        }

        /// <summary>
        /// This method is used for getting the list of all Employees
        /// </summary>
        /// <returns>List of all Employees</returns>
        public async Task<IEnumerable<UserAc>> GetAllEmployeesAsync()
        {
            var users = await _userManager.Users.Where(user => user.IsActive).OrderBy(user => user.FirstName).ToListAsync();
            return _mapperContext.Map<IEnumerable<ApplicationUser>, IEnumerable<UserAc>>(users);
        }

        /// <summary>
        /// This method is used to edit the details of an existing user
        /// </summary>
        /// <param name="editedUser">Passed UserAc object</param>
        /// <param name="updatedBy">Passed id of user who has updated this user.</param>
        /// <returns>Updated user id.</returns>
        public async Task<string> UpdateUserDetailsAsync(UserAc editedUser, string updatedBy)
        {
            var user = await _userManager.FindByIdAsync(editedUser.Id);
            user.FirstName = editedUser.FirstName;
            user.LastName = editedUser.LastName;
            user.Email = editedUser.Email;
            user.IsActive = editedUser.IsActive;
            user.UpdatedBy = updatedBy;
            user.UpdatedDateTime = DateTime.UtcNow;
            user.NumberOfCasualLeave = editedUser.NumberOfCasualLeave;
            user.NumberOfSickLeave = editedUser.NumberOfSickLeave;
            await _userManager.UpdateAsync(user);
            //get user roles
            IList<string> listofUserRole = await _userManager.GetRolesAsync(user);
            //remove user role 
            await _userManager.RemoveFromRoleAsync(user, listofUserRole.First());
            //add new role of user.
            await _userManager.AddToRoleAsync(user, editedUser.RoleName);
            return user.Id;
        }

        /// <summary>
        /// This method is used to delete the user.
        /// </summary>
        /// <param name="id">passed userId</param>
        /// <returns></returns>
        public async Task<string> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var projects = await _projectDataRepository.Fetch(x => x.TeamLeaderId == id).ToListAsync();
            var projectUsers = await _projectUserDataRepository.Fetch(x => x.UserId == id).ToListAsync();
            var projectList = await _projectDataRepository.Fetch(x => projectUsers.Select(y => y.ProjectId).Contains(x.Id)).ToListAsync();
            projects = projects.Union(projectList).ToList();
            if (!projects.Any())
            {
                await _userManager.DeleteAsync(user);
                return String.Empty;
            }
            return String.Join(",", projects.Select(x => x.Name));
        }
        /// <summary>
        ///  This method used for get user detail by user id 
        /// </summary>
        /// <param name="id">Passed user id</param>
        /// <returns>UserAc application class object</returns>
        public async Task<UserAc> GetByIdAsync(string id)
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser != null)
            {
                UserAc userAc = _mapperContext.Map<ApplicationUser, UserAc>(applicationUser);
                userAc.RoleName = (await _userManager.GetRolesAsync(applicationUser)).First();
                return userAc;
            }
            throw new UserNotFound();
        }

        /// <summary>
        /// This method is used to change the password of a particular user. -An
        /// </summary>
        /// <param name="passwordModel">Passed changePasswordViewModel object(OldPassword,NewPassword,ConfirmPassword,Email)</param>
        /// <returns>If password is changed successfully, return empty otherwise error message.</returns>
        public async Task<ChangePasswordErrorModel> ChangePasswordAsync(ChangePasswordViewModel passwordModel)
        {
            var user = await _userManager.FindByEmailAsync(passwordModel.Email);
            if (user != null)
            {
                ChangePasswordErrorModel changePasswordErrorModel = new ChangePasswordErrorModel();
                IdentityResult result = await _userManager.ChangePasswordAsync(user, passwordModel.OldPassword, passwordModel.NewPassword);
                if (!result.Succeeded)//When password not changed successfully then error message will be added in changePasswordErrorModel
                {
                    changePasswordErrorModel.ErrorMessage = result.Errors.First().Description;
                }
                return changePasswordErrorModel;
            }
            throw new UserNotFound();
        }


        /// <summary>
        /// This method is used to check email is already exists in database.
        /// </summary>
        /// <param name="email">Passed user email address</param>
        /// <returns> boolean: true if the email exists, false if does not exist</returns>
        public async Task<bool> CheckEmailIsExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        /// <summary>
        /// This method used for re-send mail for user credentials. -An
        /// </summary>
        /// <param name="id">passed user id</param>
        /// <returns></returns>
        public async Task ReSendMailAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            string newPassword = GetRandomString();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user); //genrate passsword reset token
            await _userManager.ResetPasswordAsync(user, token, newPassword);
            SendEmail(user.FirstName, user.Email, newPassword);
        }

        #region External Call Methods
        /// <summary>
        /// Method to get user details by slackUserId -SD
        /// </summary>
        /// <param name="slackUserId">User's slack userId</param>
        /// <returns>user details</returns>
        public async Task<UserAc> UserBasicDetialByUserIdAsync(string userId)
        {
            var user = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                var userAc = _mapperContext.Map<ApplicationUser, UserAc>(user);
                return userAc;
            }
            else
                throw new SlackUserNotFound();
        }

        /// <summary>
        /// Method to get list of team leader's details by userSlackId -SD
        /// </summary>
        /// <param name="userSlackId">User's slack userId</param>
        /// <returns>list of team leader</returns>
        public async Task<List<UserAc>> ListOfTeamLeaderByUserIdAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                var projects = await _projectUserRepository.FetchAsync(x => x.UserId == user.Id);
                var projectIds = projects.Select(x => x.ProjectId).ToList();
                List<UserAc> teamLeaders = new List<UserAc>();
                foreach (var projectId in projectIds)
                {
                    var projectDetails = await _projectRepository.GetProjectByIdAsync(projectId);
                    var teamLeaderId = projectDetails.TeamLeaderId;
                    user = await _userManager.FindByIdAsync(teamLeaderId);
                    var userAc = _mapperContext.Map<ApplicationUser, UserAc>(user);
                    teamLeaders.Add(userAc);
                }
                return teamLeaders;
            }
            else
                throw new SlackUserNotFound();
        }

        /// <summary>
        /// Method to get management people details - SD
        /// </summary>
        /// <returns>list of management</returns>
        public async Task<List<UserAc>> ListOfManagementDetailsAsync()
        {
            var management = await _userManager.GetUsersInRoleAsync(_stringConstant.Admin);
            if (management.Any())
            {
                List<UserAc> managementUser = new List<UserAc>();
                foreach (var user in management)
                {
                    var userAc = _mapperContext.Map<ApplicationUser, UserAc>(user);
                    managementUser.Add(userAc);
                }
                return managementUser;
            }
            else
                throw new FailedToFetchDataException();
        }


        /// <summary>
        /// Method to get the number of casual leave allowed to a user by slack user name -SD
        /// </summary>
        /// <param name="slackUserId">User's slack userId</param>
        /// <returns>number of casual leave</returns>
        public async Task<LeaveAllowed> GetUserAllowedLeaveByUserIdAsync(string userId)
        {
            var user = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                LeaveAllowed leaveAllowed = new LeaveAllowed();
                leaveAllowed.CasualLeave = user.NumberOfCasualLeave;
                leaveAllowed.SickLeave = user.NumberOfSickLeave;
                return leaveAllowed;
            }
            else
                throw new SlackUserNotFound();
        }

        /// <summary>
        /// Method to check whether user is admin or not - SD
        /// </summary>
        /// <param name="slackUserId">User's slack userId</param>
        /// <returns>true if user is admin else false</returns>
        public async Task<bool> IsAdminAsync(string userId)
        {
            var user = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                return await _userManager.IsInRoleAsync(user, _stringConstant.Admin);
            }
            else
                throw new SlackUserNotFound();
        }
        #endregion

        /// <summary>
        /// This method is used to Get User details by Id  - GA
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>details of user</returns>
        public async Task<UserAc> UserDetailByIdAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(x => x.Id == userId);
            return await GetUserAsync(user);
        }


        /// <summary>
        /// Method to return user/users infromation. - RS
        /// </summary>
        /// <param name="userId">passed user id</param>
        /// <returns>user/users information</returns>
        public async Task<List<UserRoleAc>> GetUserRoleAsync(string userId)
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(userId);
            var employeeRole = (await _userManager.GetRolesAsync(applicationUser)).First();
            List<UserRoleAc> userRoleAcList = new List<UserRoleAc>();

            if (employeeRole == _stringConstant.Admin) //If login user is admin then return all active users with role.
            {
                //getting the all user infromation. 
                var userRole = new UserRoleAc(applicationUser.Id, applicationUser.UserName, applicationUser.FirstName + " " + applicationUser.LastName, employeeRole);
                userRoleAcList.Add(userRole);
                //getting employee role id. 
                var roleId = (await _roleManager.Roles.SingleAsync(x => x.Name == _stringConstant.Employee)).Id;
                //getting active employee list.
                var userList = await _applicationUserDataRepository.Fetch(y => y.IsActive && y.Roles.Any(x => x.RoleId == roleId)).ToListAsync();
                foreach (var user in userList)
                {
                    var userRoleAc = new UserRoleAc(user.Id, user.UserName, user.FirstName + " " + user.LastName, employeeRole);
                    userRoleAcList.Add(userRoleAc);
                }
            }
            else //If login user is team leader/employee then return own infromation with his role.
            {
                //check login user is teamLeader or not.
                var isProjectExists = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId == applicationUser.Id);
                //If isProjectExists is null then user role is employee other wise user role is teamleader. 
                var userRoleAc = new UserRoleAc(applicationUser.Id, applicationUser.UserName, applicationUser.FirstName + " " + applicationUser.LastName, (isProjectExists != null ? _stringConstant.TeamLeader : _stringConstant.Employee));
                userRoleAcList.Add(userRoleAc);
            }
            return userRoleAcList;
        }

        /// <summary>
        /// Method to return list of teamMembers. - RS
        /// </summary>
        /// <param name="userId">Passed user id</param>
        /// <returns>teamMembers information</returns>
        public async Task<List<UserRoleAc>> GetTeamMembersAsync(string userId)
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(userId);
            var userRoleAcList = new List<UserRoleAc>();
            var userRoleAc = new UserRoleAc(applicationUser.Id, applicationUser.UserName, applicationUser.FirstName + " " + applicationUser.LastName, _stringConstant.TeamLeader);
            userRoleAcList.Add(userRoleAc);
            //Get projectid's for that specific teamleader
            IEnumerable<int> projectIds = await _projectDataRepository.Fetch(x => x.TeamLeaderId.Equals(applicationUser.Id)).Select(y => y.Id).ToListAsync();
            //Get distinct userid's for projects with that particular teamleader 
            var userIdList = await _projectUserRepository.Fetch(x => projectIds.Contains(x.ProjectId)).Select(y => y.UserId).Distinct().ToListAsync();
            //getting list of user infromation.
            var userList = await _applicationUserDataRepository.Fetch(x => userIdList.Contains(x.Id)).ToListAsync();
            foreach (var user in userList)
            {
                var usersRoleAc = new UserRoleAc(user.Id, user.UserName, user.FirstName + " " + user.LastName, _stringConstant.Employee);
                userRoleAcList.Add(usersRoleAc);
            }
            return userRoleAcList;
        }

        /// <summary>
        /// The method is used to get list of projects along with its users for a specific teamleader  - GA
        /// </summary>
        /// <param name="teamLeaderId">Id of the teamleader</param>
        /// <returns>list of projects with users for a specific teamleader</returns>
        public async Task<List<UserAc>> GetProjectUsersByTeamLeaderIdAsync(string teamLeaderId)
        {
            List<UserAc> userAcList = new List<UserAc>();
            //Get projectid's for that specific teamleader
            IEnumerable<int> projectIds = await _projectDataRepository.Fetch(x => x.TeamLeaderId.Equals(teamLeaderId)).Select(y => y.Id).ToListAsync();
            //Get details of teamleader
            ApplicationUser teamLeader = await _applicationUserDataRepository.FirstAsync(x => x.Id.Equals(teamLeaderId));
            UserAc projectTeamLeader = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
            projectTeamLeader.Role = _stringConstant.TeamLeader;
            userAcList.Add(projectTeamLeader);

            //Get details of distinct employees for projects with that particular teamleader
            var userIds = await _projectUserRepository.Fetch(x => projectIds.Contains(x.ProjectId)).Select(y => y.UserId).Distinct().ToListAsync();
            foreach (var userId in userIds)
            {
                ApplicationUser user = await _applicationUserDataRepository.FirstAsync(x => x.Id.Equals(userId));
                UserAc userAc = _mapperContext.Map<ApplicationUser, UserAc>(user);
                userAc.Role = _stringConstant.Employee;
                userAcList.Add(userAc);
            }
            return userAcList;
        }

        /// <summary>
        /// This method used for get list of user emails based on role.
        /// </summary>
        /// <returns>list of teamleader ,managment and employee email</returns>
        public async Task<UserEmailListAc> GetUserEmailListBasedOnRoleAsync()
        {
            UserEmailListAc userEmailListAC = new UserEmailListAc();
            //Get all managment email list 
            var roleIds = await _roleManager.Roles.Where(x => !x.Name.Equals(_stringConstant.Employee)).Select(x => x.Id).ToListAsync();
            userEmailListAC.Management = await _userManager.Users.Where(x => x.Roles.Any(y => roleIds.Contains(y.RoleId)) && x.IsActive).Select(x => x.Email).Distinct().ToListAsync();
            //Get all teamLeader list 
            var teamLeadersIds = await _projectDataRepository.GetAll().Select(x => x.TeamLeaderId).Distinct().ToListAsync();
            userEmailListAC.TeamLeader = await _userManager.Users.Where(x => teamLeadersIds.Contains(x.Id) && x.IsActive).Select(x => x.Email).Distinct().ToListAsync();
            //Get all teamMember list
            userEmailListAC.TamMemeber = await _projectUserDataRepository.Fetch(x => x.User.IsActive).Select(x => x.User.Email).Distinct().ToListAsync();
            return userEmailListAC;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is used to send email to the currently added user. -An
        /// </summary>
        /// <param name="firstName">Passed user first name</param>
        /// <param name="email">Passed user email</param>
        /// <param name="password">Passed password</param>
        private void SendEmail(string firstName, string email, string password)
        {
            string finaleTemplate = _emailUtil.GetEmailTemplateForUserDetail(firstName, email, password);
            _emailSender.SendEmail(email, _stringConstant.LoginCredentials, finaleTemplate);
        }

        /// <summary>
        /// Method is used to return a user after assigning a role and mapping from ApplicationUser class to UserAc class - GA
        /// </summary>
        /// <param name="user">Details of application user</param>
        /// <returns>user</returns>
        private async Task<UserAc> GetUserAsync(ApplicationUser user)
        {
            //Gets a list of roles the specified user belongs to
            string roles = (await _userManager.GetRolesAsync(user)).First();
            UserAc newUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
            //assign role
            if (roles.Equals(_stringConstant.Admin))
            {
                newUser.Role = roles;
            }
            else
            {
                Project project = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId.Equals(user.Id));
                newUser.Role = (project != null) ? _stringConstant.TeamLeader : _stringConstant.Employee;
            }
            return newUser;
        }

        /// <summary>
        /// This method used for genrate random string with alphanumeric words and special characters. -An
        /// </summary>
        /// <returns>Random string</returns>
        private string GetRandomString()
        {
            Random random = new Random();
            //Initialize static Ato,atoz,0to9 and special characters seprated by '|'.
            string chars = _stringConstant.RandomString;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                //Get random 4 characters from diffrent portion and append on stringbuilder. 
                stringBuilder.Append(new string(Enumerable.Repeat(chars.Split('|').ToArray()[i], 3).Select(s => s[random.Next(4)]).ToArray()));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Calculate casual leave and sick leave from the date of joining - RS
        /// </summary>
        /// <param name="dateTime">passed joining date</param>
        /// <returns>LeaveAllowed</returns>
        private LeaveAllowed CalculateAllowedLeaves(DateTime dateTime)
        {
            double casualAllowed;
            double sickAllowed;
            var month = dateTime.Month;
            //if joining year are more then current year or difference of current year and joining year is 1 then calculate casual Allow and sick Allow
            //other wise no need to be calculation directly set default value CasualLeave(14) and SickLeave(7).  
            if (dateTime.Year >= DateTime.Now.Year || (DateTime.Now.Year - dateTime.Year) == 1)
            {
                double casualAllow = _appSettingUtil.Value.CasualLeave;
                double sickAllow = _appSettingUtil.Value.SickLeave;
                //If an employee joins between 1st to 15th of month, then he/she will be eligible for that particular month's leaves 
                //and if he/she joins after 15th of month, he/she will not be eligible for that month's leaves.

                //In Our Project we consider Leave renewal on 1st april
                if (month >= 4)
                {
                    //if first 15 days of month april to December then substact 4 other wise substact 3 in month
                    //this setting for the employee eligible for that particular month's leaves or not
                    if (dateTime.Day <= 15)
                    {

                        casualAllowed = (casualAllow / 12) * (12 - (month - 4));
                        sickAllowed = (sickAllow / 12) * (12 - (month - 4));
                    }
                    else
                    {
                        casualAllowed = (casualAllow / 12) * (12 - (month - 3));
                        sickAllowed = (sickAllow / 12) * (12 - (month - 3));
                    }
                }
                else //calculate casual allowed and sick allowed for first three month.
                {
                    //if joining year are more then current year then calculate casual allowed and sick allowed   
                    if (dateTime.Year >= DateTime.Now.Year)
                    {
                        //if first 15 days of month January to March then add 8 other wise add 9 in month
                        //this setting for the employee eligible for that particular month's leaves or not
                        if (dateTime.Day <= 15)
                        {
                            casualAllowed = (casualAllow / 12) * (12 - (month + 8));
                            sickAllowed = (sickAllow / 12) * (12 - (month + 8));
                        }
                        else
                        {
                            casualAllowed = (casualAllow / 12) * (12 - (month + 9));
                            sickAllowed = (sickAllow / 12) * (12 - (month + 9));
                        }
                    }
                    else
                    {
                        casualAllowed = casualAllow;
                        sickAllowed = sickAllow;
                    }
                }
                var tolerance = 0.0001;
                if (Math.Abs(casualAllowed % 1) > tolerance) //check casualAllowed has decimal value
                {
                    double casualAlloweddecimal = casualAllowed - Math.Floor(casualAllowed);
                    // If calculated casualAllowed decimal value is exact 0.5 then it's considered half day casual leave
                    if (Math.Abs(casualAlloweddecimal - 0.5) > tolerance) { casualAllowed = Convert.ToInt32(casualAllowed); }
                }
                if (Math.Abs(sickAllowed % 1) > tolerance)//check sickAllowed has decmial value
                {
                    double sickAlloweddecimal = sickAllowed - Math.Floor(sickAllowed);
                    // If calculated sickAllowed decimal value is exact 0.5 then it's considered half day sick leave 
                    // If calculated sickAllowed decimal value is more than  0.90 then add one leave in sick leave 
                    if (sickAlloweddecimal > 0.90) { sickAllowed = Convert.ToInt32(Math.Ceiling(sickAllowed)); }
                    else if (Math.Abs(sickAlloweddecimal - 0.5) > tolerance) { sickAllowed = Convert.ToInt32(Math.Floor(sickAllowed)); }
                }
            }
            else
            {
                casualAllowed = _appSettingUtil.Value.CasualLeave;
                sickAllowed = _appSettingUtil.Value.SickLeave;
            }
            LeaveAllowed leaveAllowed = new LeaveAllowed
            {
                CasualLeave = casualAllowed,
                SickLeave = sickAllowed
            };
            return leaveAllowed;
        }



        #endregion


    }
}