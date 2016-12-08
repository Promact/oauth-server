using AutoMapper;
using Exceptionless.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Models.ManageViewModels;
using Promact.Oauth.Server.Repository.HttpClientRepository;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository
{
    public class UserRepository : IUserRepository
    {

        #region "Private Variable(s)"


        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDataRepository<ApplicationUser> _applicationUserDataRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapperContext;
        private readonly IDataRepository<ProjectUser> _projectUserRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDataRepository<Project> _projectDataRepository;
        private readonly IOptions<AppSettingUtil> _appSettingUtil;
        private readonly ILogger<UserRepository> _logger;
        private readonly IStringConstant _stringConstant;
        private readonly IDataRepository<ProjectUser> _projectUserDataRepository;
        private readonly IHttpClientRepository _httpClientRepository;
        #endregion


        #region "Constructor"


        public UserRepository(IDataRepository<ApplicationUser> applicationUserDataRepository,
            IHostingEnvironment hostingEnvironment, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, IEmailSender emailSender,
            IMapper mapperContext, IDataRepository<ProjectUser> projectUserRepository,
            IProjectRepository projectRepository, IOptions<AppSettingUtil> appSettingUtil,
            IDataRepository<Project> projectDataRepository,
            ILogger<UserRepository> logger, IStringConstant stringConstant,
            IHttpClientRepository httpClientRepository, IDataRepository<ProjectUser> projectUserDataRepository)
        {
            _applicationUserDataRepository = applicationUserDataRepository;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _emailSender = emailSender;
            _mapperContext = mapperContext;
            _projectUserRepository = projectUserRepository;
            _projectRepository = projectRepository;
            _roleManager = roleManager;
            _projectDataRepository = projectDataRepository;
            _appSettingUtil = appSettingUtil;
            _logger = logger;
            _stringConstant = stringConstant;
            _projectUserDataRepository = projectUserDataRepository;
            _httpClientRepository = httpClientRepository;
        }

        #endregion


        #region "Public Method(s)"

        /// <summary>
        /// This method is used to add new user
        /// </summary>
        /// <param name="applicationUser">UserAc Application class object</param>
        public async Task<string> AddUser(UserAc newUser, string createdBy)
        {
            LeaveCalculator LC = new LeaveCalculator();
            LC = CalculateAllowedLeaves(Convert.ToDateTime(newUser.JoiningDate));
            newUser.NumberOfCasualLeave = LC.CasualLeave;
            newUser.NumberOfSickLeave = LC.SickLeave;
            ApplicationUser user = _mapperContext.Map<UserAc, ApplicationUser>(newUser);
            user.UserName = user.Email;
            user.CreatedBy = createdBy;
            user.CreatedDateTime = DateTime.UtcNow;
            string password = GetRandomString();
            var result = _userManager.CreateAsync(user, password);
            IdentityResult resultSuccess = await result;
            result = _userManager.AddToRoleAsync(user, newUser.RoleName);
            SendEmail(user, password);
            resultSuccess = await result;
            return user.Id;
        }

        /// <summary>
        /// Calculat casual leava and sick leave from the date of joining
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private LeaveCalculator CalculateAllowedLeaves(DateTime dateTime)
        {
            double casualAllowed = 0;
            double sickAllowed = 0;
            int day = dateTime.Day;
            int month = dateTime.Month;
            int year = dateTime.Year;
            double casualAllow = Convert.ToDouble(_appSettingUtil.Value.CasualLeave);
            double sickAllow = Convert.ToDouble(_appSettingUtil.Value.SickLeave);
            if (year >= DateTime.Now.Year)
            {
                if (year - DateTime.Now.Year > 365)
                {
                    month = 4;
                    day = 1;
                }
                double totalDays = (DateTime.Now - Convert.ToDateTime(dateTime)).TotalDays;
                if (totalDays > 365)
                {
                    month = 4;
                    day = 1;
                }
                if (month >= 4)
                {
                    if (day <= 15)
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
                else
                {
                    if (day <= 15)
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

                if (casualAllowed.ToString().Contains(".") == true)
                {
                    string splitCasualAllowed = "0." + casualAllowed.ToString().Split('.')[1];
                    double casualAllowedConvertedDouble = Convert.ToDouble(splitCasualAllowed);
                    if (casualAllowedConvertedDouble != 0.5) { casualAllowed = Convert.ToInt32(casualAllowed); }

                }
                else
                {
                    casualAllowed = Convert.ToInt32(casualAllowed);
                }
                if (sickAllowed.ToString().Contains(".") == true)
                {
                    string splitSickAllowed = "0." + sickAllowed.ToString().Split('.')[1];
                    double sickAllowedConvertedDouble = Convert.ToDouble(splitSickAllowed);
                    if (sickAllowedConvertedDouble != 0.5) { sickAllowed = Convert.ToInt32(Math.Floor(sickAllowed)); }
                    if (sickAllowedConvertedDouble > 0.90) { sickAllowed = sickAllowed + 1; }

                }
                else
                {
                    sickAllowed = Convert.ToInt32(Math.Floor(sickAllowed));
                }
            }
            else
            {
                casualAllow = Convert.ToDouble(_appSettingUtil.Value.CasualLeave);
                sickAllowed = Convert.ToDouble(_appSettingUtil.Value.SickLeave);
            }
            LeaveCalculator calculate = new LeaveCalculator
            {
                CasualLeave = casualAllowed,
                SickLeave = sickAllowed
            };

            return calculate;
        }

        public List<RolesAc> GetRoles()
        {
            try
            {
                List<RolesAc> listOfRoleAC = new List<RolesAc>();
                IQueryable<IdentityRole> roles = _roleManager.Roles;
                foreach (IdentityRole identityRole in roles.ToList())
                {
                    RolesAc roleAc = new RolesAc();
                    roleAc.Id = identityRole.Id;
                    roleAc.Name = identityRole.Name;
                    listOfRoleAC.Add(roleAc);
                }
                return listOfRoleAC;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// This method is used for getting the list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        public IEnumerable<UserAc> GetAllUsers()
        {
            IOrderedQueryable<ApplicationUser> users = _userManager.Users.OrderByDescending(x => x.CreatedDateTime);
            List<UserAc> userList = new List<UserAc>();

            foreach (var user in users)
            {
                UserAc listItem = _mapperContext.Map<ApplicationUser, UserAc>(user);
                userList.Add(listItem);
            }
            return userList;
        }


        /// <summary>
        /// This method is used for getting the list of all Employees
        /// </summary>
        /// <returns>List of all Employees</returns>
        public async Task<List<UserAc>> GetAllEmployees()
        {
            List<ApplicationUser> users = await _userManager.Users.Where(user => user.IsActive).OrderBy(user => user.FirstName).ToListAsync();
            return _mapperContext.Map<List<ApplicationUser>, List<UserAc>>(users);
        }


        /// <summary>
        /// This method is used to edit the details of an existing user
        /// </summary>
        /// <param name="editedUser">UserAc Application class object</param>
        public async Task<string> UpdateUserDetails(UserAc editedUser, string updatedBy)
        {
            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.SlackUserName == editedUser.SlackUserName && x.Id != editedUser.Id);
            if (user == null)
            {
                user = await _userManager.FindByIdAsync(editedUser.Id);
                user.FirstName = editedUser.FirstName;
                user.LastName = editedUser.LastName;
                user.Email = editedUser.Email;
                user.IsActive = editedUser.IsActive;
                user.UpdatedBy = updatedBy;
                user.UpdatedDateTime = DateTime.UtcNow;
                user.NumberOfCasualLeave = editedUser.NumberOfCasualLeave;
                user.NumberOfSickLeave = editedUser.NumberOfSickLeave;
                user.SlackUserName = editedUser.SlackUserName;
                ApplicationUser userPreviousInfo = await _userManager.FindByEmailAsync(editedUser.Email);
                await _userManager.UpdateAsync(user);
                IList<string> listofUserRole = await _userManager.GetRolesAsync(user);
                IdentityResult removeFromRole = await _userManager.RemoveFromRoleAsync(user, listofUserRole.FirstOrDefault());
                IdentityResult addNewRole = await _userManager.AddToRoleAsync(user, editedUser.RoleName);
                return user.Id;

            }
            throw new SlackUserNotFound();
        }



        /// <summary>
        /// This method is used to get particular user's details by his/her id
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>UserAc Application class object</returns>
        public async Task<UserAc> GetById(string id)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);
                UserAc requiredUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
                IList<string> identityUserRole = await _userManager.GetRolesAsync(user);
                requiredUser.RoleName = identityUserRole.FirstOrDefault();
                return requiredUser;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }


        /// <summary>
        /// This method is used to change the password of a particular user
        /// </summary>
        /// <param name="passwordModel">ChangePasswordViewModel type object</param>
        public async Task<string> ChangePassword(ChangePasswordViewModel passwordModel)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(passwordModel.Email);
            if (user != null)
            {
                IdentityResult result = await _userManager.ChangePasswordAsync(user, passwordModel.OldPassword, passwordModel.NewPassword);
                if (result.Succeeded)
                {
                    return passwordModel.NewPassword;
                }
                return result.Errors.FirstOrDefault().Description.ToString();
            }
            throw new UserNotFound();
        }

        /// <summary>
        /// This method is used to check if a user already exists in the database with the given userName
        /// </summary>
        /// <param name="userName">string userName</param>
        /// <returns> boolean: true if the user name exists, false if does not exist</returns>
        public async Task<bool> FindByUserName(string userName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Used to fetch the userdetail by given UserName 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns>object of UserAc</returns>
        public async Task<UserAc> GetUserDetail(string UserName)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByNameAsync(UserName);
                UserAc userAc = new UserAc();
                if (user != null)
                {
                    userAc.Email = user.Email;
                    userAc.Id = user.Id;
                    userAc.FirstName = user.FirstName;
                    userAc.LastName = user.LastName;
                    userAc.UserName = user.UserName;
                    // userAc.SlackUserName = user.SlackUserName;
                }
                return userAc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to check if a user already exists in the database with the given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns> boolean: true if the email exists, false if does not exist</returns>
        public async Task<bool> CheckEmailIsExists(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            return user == null ? false : true;
        }


        /// <summary>
        /// Fetches user with the given Slack User Id
        /// </summary>
        /// <param name="slackUserName"></param>
        /// <returns></returns>
        public ApplicationUser FindUserBySlackUserName(string slackUserName)
        {
            ApplicationUser user = _applicationUserDataRepository.FirstOrDefault(x => x.SlackUserName == slackUserName);
            if (user == null)
                throw new SlackUserNotFound();
            else
                return user;
        }


        /// <summary>
        /// This method used for re -send mail for user credentails
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ReSendMail(string id)
        {
            _logger.LogInformation("start Resend Mail Method in User Repository");
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            string newPassword = GetRandomString();
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, newPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Successfully Reset Password");
                if (SendEmail(user, newPassword))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Method to get user details by slackUserId
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>user details</returns>
        public ApplicationUser UserDetialByUserSlackId(string slackUserId)
        {
            ApplicationUser user = _applicationUserDataRepository.FirstOrDefault(x => x.SlackUserId == slackUserId);
            ApplicationUser newUser = new ApplicationUser();
            if (user != null)
            {
                newUser.Id = user.Id;
                newUser.Email = user.Email;
                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.SlackUserId = user.SlackUserId;
                return newUser;
            }
            else
                throw new SlackUserNotFound();
        }


        /// <summary>
        /// Method to get team leader's details by userSlackId
        /// </summary>
        /// <param name="userSlackId"></param>
        /// <returns>list of team leader</returns>
        public async Task<List<ApplicationUser>> TeamLeaderByUserSlackId(string userSlackId)
        {
            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.SlackUserId == userSlackId);
            List<ApplicationUser> teamLeaders = new List<ApplicationUser>();
            if (user != null)
            {
                IQueryable<ProjectUser> projects = _projectUserRepository.Fetch(x => x.UserId == user.Id);
                foreach (var project in projects)
                {
                    ProjectAc projectAc = await _projectRepository.GetProjectByIdAsync(project.ProjectId);
                    string teamLeaderId = projectAc.TeamLeaderId;
                    user = await _userManager.FindByIdAsync(teamLeaderId);
                    //user = _userManager.Users.FirstOrDefault(x => x.Id == teamLeader);
                    ApplicationUser newUser = new ApplicationUser
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        SlackUserId = user.SlackUserId
                    };
                    teamLeaders.Add(newUser);
                }
            }
            return teamLeaders;
        }


        /// <summary>
        /// Method to get management people details
        /// </summary>
        /// <returns>list of management</returns>
        public async Task<List<ApplicationUser>> ManagementDetails()
        {
            IList<ApplicationUser> management = await _userManager.GetUsersInRoleAsync("Admin");
            List<ApplicationUser> managementUser = new List<ApplicationUser>();
            foreach (var user in management)
            {
                var newUser = new ApplicationUser
                {
                    FirstName = user.FirstName,
                    Email = user.Email,
                    SlackUserId = user.SlackUserId
                };
                managementUser.Add(newUser);
            }
            return managementUser;
        }


        /// <summary>
        /// Method to get the number of casual leave allowed to a user by slack user name
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>number of casual leave</returns>
        public LeaveAllowed GetUserAllowedLeaveBySlackId(string slackUserId)
        {
            ApplicationUser user = _applicationUserDataRepository.FirstOrDefault(x => x.SlackUserId == slackUserId);
            LeaveAllowed leaveAllowed = new LeaveAllowed();
            if (user != null)
            {
                leaveAllowed.CasualLeave = user.NumberOfCasualLeave;
                leaveAllowed.SickLeave = user.NumberOfSickLeave;
                return leaveAllowed;
            }
            else
                throw new SlackUserNotFound();
        }

        /// <summary>
        /// Method to check whether user is admin or not
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>true or false</returns>
        public async Task<bool> IsAdmin(string slackUserId)
        {
            ApplicationUser user = _applicationUserDataRepository.FirstOrDefault(x => x.SlackUserId == slackUserId);
            if (user != null)
            {
                bool isAdmin = await _userManager.IsInRoleAsync(user, _stringConstant.Admin);
                return isAdmin;
            }
            else
                throw new SlackUserNotFound();
        }

        /// <summary>
        /// This method is used to Get User details by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>details of user</returns>
        public UserAc UserDetailById(string userId)
        {
            ApplicationUser user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            return GetUser(user);
        }

        /// <summary>
        /// Method is used to get the details of user by using their username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>details of user</returns>
        public async Task<UserAc> GetUserDetailByUserName(string userName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            return GetUser(user);
        }


        /// <summary>
        /// Method to return user role
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetUserRoleAsync(string userId)
        {
            ApplicationUser applicationUser = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == userId);
            string userRole = (await _userManager.GetRolesAsync(applicationUser)).First();
            List<UserRoleAc> UserRoleAcList = new List<UserRoleAc>();
            if (userRole == _stringConstant.RoleAdmin)
            {
                UserRoleAc userRoleAdmin = new UserRoleAc();
                userRoleAdmin.UserName = applicationUser.UserName;
                userRoleAdmin.Name = applicationUser.FirstName + " " + applicationUser.LastName;
                userRoleAdmin.Role = userRole;
                UserRoleAcList.Add(userRoleAdmin);
                List<ApplicationUser> userList = await _applicationUserDataRepository.GetAll().ToListAsync();
                foreach (var userDetails in userList)
                {
                    string roles = (await _userManager.GetRolesAsync(userDetails)).First();
                    if (roles != null && roles == _stringConstant.RoleEmployee)
                    {
                        UserRoleAc userRoleAc = new UserRoleAc();
                        userRoleAc.UserName = userDetails.UserName;
                        userRoleAc.Name = userDetails.FirstName + " " + userDetails.LastName;
                        userRoleAc.Role = userRole;
                        UserRoleAcList.Add(userRoleAc);
                    }
                }
            }
            else
            {
                Project project = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId == applicationUser.Id);
                if (project == null)
                {
                    UserRoleAc usersRolesAc = new UserRoleAc();
                    usersRolesAc.UserName = applicationUser.UserName;
                    usersRolesAc.Role = _stringConstant.RoleEmployee;
                    usersRolesAc.Name = applicationUser.FirstName + " " + applicationUser.LastName;
                    UserRoleAcList.Add(usersRolesAc);
                }
                else
                {
                    UserRoleAc usersRoleAc = new UserRoleAc();
                    usersRoleAc.UserName = applicationUser.UserName;
                    usersRoleAc.Role = _stringConstant.RoleTeamLeader;
                    usersRoleAc.Name = applicationUser.FirstName + " " + applicationUser.LastName;
                    UserRoleAcList.Add(usersRoleAc);
                }
            }
            if (UserRoleAcList == null)
                throw new UserRoleNotFound();
            else
                return UserRoleAcList;

        }

        /// <summary>
        /// Method to return list of users.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetTeamMembersAsync(string userId)
        {
            ApplicationUser applicationUser = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == userId);
            if (applicationUser != null)
            {
                List<UserRoleAc> userRolesAcList = new List<UserRoleAc>();
                UserRoleAc usersRoleAc = new UserRoleAc();
                usersRoleAc.UserName = applicationUser.UserName;
                usersRoleAc.Role = _stringConstant.RoleTeamLeader;
                usersRoleAc.Name = applicationUser.FirstName + " " + applicationUser.LastName;
                userRolesAcList.Add(usersRoleAc);
                Project project = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId == applicationUser.Id);
                IEnumerable<ProjectUser> projectUserList = await _projectUserDataRepository.FetchAsync(x => x.ProjectId == project.Id);

                foreach (var projectUser in projectUserList)
                {
                    UserRoleAc usersRolesAc = new UserRoleAc();
                    ApplicationUser users = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == projectUser.UserId);
                    usersRolesAc.UserName = users.UserName;
                    usersRolesAc.Name = users.FirstName + " " + users.LastName;
                    usersRolesAc.Role = _stringConstant.RoleAdmin;
                    userRolesAcList.Add(usersRolesAc);
                }
                return userRolesAcList;
            }
            else
            {
                throw new UserRoleNotFound();
            }
        }

        /// <summary>
        /// Method to return list of users/employees of the given group name. - JJ
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="UserName"></param>
        /// <returns>list of object of UserAc</returns>
        public async Task<List<UserAc>> GetProjectUserByGroupNameAsync(string GroupName)
        {

            Project project = await _projectDataRepository.FirstOrDefaultAsync(x => x.SlackChannelName == GroupName);
            List<UserAc> userAcList = new List<UserAc>();
            if (project != null)
            {
                IEnumerable<ProjectUser> projectUserList = await _projectUserDataRepository.FetchAsync(x => x.ProjectId == project.Id);
                foreach (var projectUser in projectUserList)
                {
                    ApplicationUser user = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == projectUser.UserId && x.SlackUserId != null);
                    if (user != null)
                    {
                        UserAc userAc = new UserAc();
                        userAc.Id = user.Id;
                        userAc.Email = user.Email;
                        userAc.FirstName = user.FirstName;
                        userAc.IsActive = user.IsActive;
                        userAc.LastName = user.LastName;
                        userAc.UserName = user.UserName;
                        userAc.SlackUserId = user.SlackUserId;
                        userAcList.Add(userAc);
                    }
                }

            }
            if (userAcList == null)
                throw new UserNotFound();
            else
                return userAcList;
        }

        #endregion


        #region Private Methods


        /// <summary>
        /// Fetches the slack real name of the user of the given SlackUserId - JJ
        /// </summary>
        /// <param name="slackUsers"></param>
        /// <param name="slackUserId"></param>
        /// <returns></returns>
        private string GetSlackName(List<SlackUserDetailAc> slackUsers, string slackUserId)
        {
            string slackName = string.Empty;
            foreach (var user in slackUsers)
            {
                if (String.Compare(user.UserId, slackUserId, false) == 0)
                {
                    slackName = user.Name;
                    break;
                }
            }
            return slackName;
        }


        /// <summary>
        /// Method is used to return a user after assigning a role and mapping from ApplicationUser class to UserAc class
        /// </summary>
        /// <param name="user"></param>
        /// <returns>user</returns>
        private UserAc GetUser(ApplicationUser user)
        {
            if (user != null)
            {
                string roles = _userManager.GetRolesAsync(user).Result.First();
                UserAc newUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
                if (String.Compare(roles, _stringConstant.Admin, true) == 0)
                {
                    newUser.Role = roles;
                    return newUser;
                }
                if (String.Compare(roles, _stringConstant.Employee, true) == 0)
                {
                    Project project = _projectDataRepository.FirstOrDefault(x => x.TeamLeaderId.Equals(user.Id));
                    if (project != null)
                    {
                        newUser.Role = _stringConstant.TeamLeader;
                        return newUser;
                    }
                    else
                    {
                        newUser.Role = _stringConstant.Employee;
                        return newUser;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// This method is used to send email to the currently added user
        /// </summary>
        /// <param name="user">Object of newly registered User</param>
        private bool SendEmail(ApplicationUser user, string password)
        {
            _logger.LogInformation("Start Fetch Email Template");
            string path = _hostingEnvironment.ContentRootPath + _stringConstant.UserDetialTemplateFolderPath;
            _logger.LogInformation("ContentRootPath Path:" + _hostingEnvironment.ContentRootPath);
            _logger.LogInformation("Full Path:" + _hostingEnvironment.ContentRootPath + _stringConstant.UserDetialTemplateFolderPath);
            string finaleTemplate = "";
            if (System.IO.File.Exists(path))
            {
                _logger.LogInformation("Email Template Featch successfully");
                finaleTemplate = System.IO.File.ReadAllText(path);
                finaleTemplate = finaleTemplate.Replace(_stringConstant.UserEmail, user.Email).Replace(_stringConstant.UserPassword, password).Replace(_stringConstant.ResertPasswordUserName, user.FirstName);
                _emailSender.SendEmail(user.Email, _stringConstant.LoginCredentials, finaleTemplate);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method used for genrate random string with alphanumeric words and special characters. 
        /// </summary>
        /// <returns></returns>
        private string GetRandomString()
        {
            Random random = new Random();
            //Initialize static Ato,atoz,0to9 and special characters seprated by '|'.
            const string chars = "abcdefghijklmnopqrstuvwxyz|ABCDEFGHIJKLMNOPQRSTUVWXYZ|012345789|@#$%^!&*()";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                //Get random 4 characters from diffrent portion and append on stringbuilder. 
                sb.Append(new string(Enumerable.Repeat(chars.Split('|').ToArray()[i], 3).Select(s => s[random.Next(4)]).ToArray()));
            }
            return sb.ToString();
        }

        #endregion


    }
}