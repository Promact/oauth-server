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
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Services;
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
        private readonly IHttpClientService _httpClientRepository;
        #endregion

        #region "Constructor"


        public UserRepository(IDataRepository<ApplicationUser> applicationUserDataRepository,
            IHostingEnvironment hostingEnvironment, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, IEmailSender emailSender,
            IMapper mapperContext, IDataRepository<ProjectUser> projectUserRepository,
            IProjectRepository projectRepository, IOptions<AppSettingUtil> appSettingUtil,
            IDataRepository<Project> projectDataRepository,
            ILogger<UserRepository> logger, IStringConstant stringConstant,
            IHttpClientService httpClientRepository, IDataRepository<ProjectUser> projectUserDataRepository)
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
        public async Task<string> AddUserAsync(UserAc newUser, string createdBy)
        {
            LeaveCalculator leaveCalculator = new LeaveCalculator();
            leaveCalculator = CalculateAllowedLeaves(Convert.ToDateTime(newUser.JoiningDate));
            newUser.NumberOfCasualLeave = leaveCalculator.CasualLeave;
            newUser.NumberOfSickLeave = leaveCalculator.SickLeave;
            var user = _mapperContext.Map<UserAc, ApplicationUser>(newUser);
            user.UserName = user.Email;
            user.CreatedBy = createdBy;
            user.CreatedDateTime = DateTime.UtcNow;
            string password = GetRandomString();
            var result = _userManager.CreateAsync(user, password);
            var resultSuccess = await result;
            result = _userManager.AddToRoleAsync(user, newUser.RoleName);
            SendEmail(user, password);
            resultSuccess = await result;
            return user.Id;

        }

        /// <summary>
        /// this mthod is used to get all role
        /// </summary>
        /// <returns></returns>
        public async Task<List<RolesAc>> GetRolesAsync()
        {
            List<RolesAc> listOfRoleAC = new List<RolesAc>();
            var roles = await _roleManager.Roles.ToListAsync();
            foreach (IdentityRole identityRole in roles)
            {
                RolesAc roleAc = new RolesAc();
                roleAc.Id = identityRole.Id;
                roleAc.Name = identityRole.Name;
                listOfRoleAC.Add(roleAc);
            }
            return listOfRoleAC;
        }


        /// <summary>
        /// This method is used for getting the list of all users
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
            return _mapperContext.Map<List<ApplicationUser>, List<UserAc>>(users);
        }


        /// <summary>
        /// This method is used to edit the details of an existing user
        /// </summary>
        /// <param name="editedUser">UserAc Application class object</param>
        public async Task<string> UpdateUserDetailsAsync(UserAc editedUser, string updatedBy)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.SlackUserName == editedUser.SlackUserName && x.Id != editedUser.Id);
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
                var userPreviousInfo = await _userManager.FindByEmailAsync(editedUser.Email);
                await _userManager.UpdateAsync(user);
                IList<string> listofUserRole = await _userManager.GetRolesAsync(user);
                var removeFromRole = await _userManager.RemoveFromRoleAsync(user, listofUserRole.FirstOrDefault());
                var addNewRole = await _userManager.AddToRoleAsync(user, editedUser.RoleName);
                return user.Id;

            }
            throw new SlackUserNotFound();
        }


        /// <summary>
        /// This method is used to get particular user's details by his/her id
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>UserAc Application class object</returns>
        public async Task<UserAc> GetByIdAsync(string id)
        {
            ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);
            if (applicationUser != null)
            {
                UserAc userAc = _mapperContext.Map<ApplicationUser, UserAc>(applicationUser);
                userAc.RoleName = (await _userManager.GetRolesAsync(applicationUser)).First();
                return userAc;
            }
            else
            {
                throw new UserNotFound();
            }

        }


        /// <summary>
        /// This method is used to change the password of a particular user
        /// </summary>
        /// <param name="passwordModel">ChangePasswordViewModel type object</param>
        public async Task<ChangePasswordErrorModel> ChangePasswordAsync(ChangePasswordViewModel passwordModel)
        {
            ChangePasswordErrorModel changePasswordErrorModel = new ChangePasswordErrorModel();
            var user = await _userManager.FindByEmailAsync(passwordModel.Email);
            if (user != null)
            {
                IdentityResult result = await _userManager.ChangePasswordAsync(user, passwordModel.OldPassword, passwordModel.NewPassword);
                if (!result.Succeeded)
                {
                    changePasswordErrorModel.ErrorMessage = result.Errors.FirstOrDefault().Description.ToString();
                }
                return changePasswordErrorModel;
            }
            throw new UserNotFound();
        }

        /// <summary>
        /// This method is used to check if a user already exists in the database with the given userName
        /// </summary>
        /// <param name="userName">string userName</param>
        /// <returns> boolean: true if the user name exists, false if does not exist</returns>
        public async Task<bool> FindByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new UserNotFound();
            }
            return true;
        }

        /// <summary>
        /// Used to fetch the userdetail by given UserName 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns>object of UserAc</returns>
        public async Task<UserAc> GetUserDetailAsync(string UserName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(UserName);
                var userAc = new UserAc();
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
        public async Task<bool> CheckEmailIsExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null ? false : true;
        }


        /// <summary>
        /// Fetches user with the given Slack User Id
        /// </summary>
        /// <param name="slackUserName"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> FindUserBySlackUserNameAsync(string slackUserName)
        {
            var user = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.SlackUserName == slackUserName);
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
        public async Task<bool> ReSendMailAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            string newPassword = GetRandomString();
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, newPassword);
            if (result.Succeeded)
                return SendEmail(user, newPassword);
            return false;
        }

        /// <summary>
        /// Method to get user details by slackUserId
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>user details</returns>
        public ApplicationUser UserDetialByUserSlackId(string slackUserId)
        {

            var user = _applicationUserDataRepository.FirstOrDefault(x => x.SlackUserId == slackUserId);
            var newUser = new ApplicationUser
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SlackUserId = user.SlackUserId
            };
            return newUser;
        }

        /// <summary>
        /// Method to get team leader's details by userSlackId
        /// </summary>
        /// <param name="userSlackId"></param>
        /// <returns>list of team leader</returns>
        public async Task<List<ApplicationUser>> TeamLeaderByUserSlackIdAsync(string userSlackId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.SlackUserId == userSlackId);
            var projects = _projectUserRepository.Fetch(x => x.UserId == user.Id);
            List<ApplicationUser> teamLeaders = new List<ApplicationUser>();
            foreach (var project in projects)
            {
                var teamLeaderId = await _projectRepository.GetProjectByIdAsync(project.ProjectId);
                var teamLeader = teamLeaderId.TeamLeaderId;
                user = await _userManager.FindByIdAsync(teamLeader);
                //user = _userManager.Users.FirstOrDefault(x => x.Id == teamLeader);
                var newUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    SlackUserId = user.SlackUserId
                };
                teamLeaders.Add(newUser);
            }
            return teamLeaders;
        }

        /// <summary>
        /// Method to get management people details
        /// </summary>
        /// <returns>list of management</returns>
        public async Task<List<ApplicationUser>> ManagementDetailsAsync()
        {
            var management = await _userManager.GetUsersInRoleAsync("Admin");
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
            var user = _applicationUserDataRepository.FirstOrDefault(x => x.SlackUserId == slackUserId);
            LeaveAllowed leaveAllowed = new LeaveAllowed();
            leaveAllowed.CasualLeave = user.NumberOfCasualLeave;
            leaveAllowed.SickLeave = user.NumberOfSickLeave;
            return leaveAllowed;
        }

        /// <summary>
        /// Method to check whether user is admin or not
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>true or false</returns>
        public async Task<bool> IsAdminAsync(string slackUserId)
        {
            var user = _applicationUserDataRepository.FirstOrDefault(x => x.SlackUserId == slackUserId);
            var isAdmin = await _userManager.IsInRoleAsync(user, _stringConstant.Admin);
            return isAdmin;
        }

        /// <summary>
        /// This method is used to Get User details by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>details of user</returns>
        public async Task<UserAc> UserDetailByIdAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return await GetUserAsync(user);
        }

        /// <summary>
        /// Method to return user role
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetUserRoleAsync(string userId)
        {
            ApplicationUser applicationUser = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == userId);
            var userRole = (await _userManager.GetRolesAsync(applicationUser)).First();
            List<UserRoleAc> userRoleAcList = new List<UserRoleAc>();
            if (userRole == _stringConstant.RoleAdmin)
            {
                var userRoleAdmin = new UserRoleAc(applicationUser.Id, applicationUser.UserName, applicationUser.FirstName + " " + applicationUser.LastName, userRole);
                userRoleAcList.Add(userRoleAdmin);
                var userList = await _applicationUserDataRepository.GetAll().ToListAsync();
                foreach (var user in userList)
                {
                    var roles = (await _userManager.GetRolesAsync(user)).First();
                    if (roles != null && roles == _stringConstant.RoleEmployee)
                    {
                        var userRoleAc = new UserRoleAc(user.Id, user.UserName, user.FirstName + " " + user.LastName, userRole);
                        userRoleAcList.Add(userRoleAc);
                    }
                }
            }
            else
            {
                var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId == applicationUser.Id);
                var userRoleAc = new UserRoleAc(applicationUser.Id, applicationUser.UserName, applicationUser.FirstName + " " + applicationUser.LastName, (project != null ? _stringConstant.RoleTeamLeader : _stringConstant.RoleEmployee));
                userRoleAcList.Add(userRoleAc);
            }
            return userRoleAcList;
        }

        /// <summary>
        /// Method to return list of users.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetTeamMembersAsync(string userId)
        {
            ApplicationUser applicationUser = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == userId);
            var userRolesAcList = new List<UserRoleAc>();
            var userRoleAc = new UserRoleAc(applicationUser.Id, applicationUser.UserName, applicationUser.FirstName + " " + applicationUser.LastName, _stringConstant.RoleTeamLeader);
            userRolesAcList.Add(userRoleAc);
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId == applicationUser.Id);
            var projectUserList = await _projectUserDataRepository.FetchAsync(x => x.ProjectId == project.Id);

            foreach (var projectUser in projectUserList)
            {
                var users = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == projectUser.UserId);
                var usersRoleAc = new UserRoleAc(users.Id, users.UserName, users.FirstName + " " + users.LastName, _stringConstant.RoleAdmin);
                userRolesAcList.Add(usersRoleAc);
            }
            return userRolesAcList;
        }

        /// <summary>
        /// Method to return list of users/employees of the given group name. - JJ
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="UserName"></param>
        /// <returns>list of object of UserAc</returns>
        public async Task<List<UserAc>> GetProjectUserByGroupNameAsync(string GroupName)
        {
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.SlackChannelName == GroupName);
            var userAcList = new List<UserAc>();
            if (project != null)
            {
                var projectUserList = await _projectUserDataRepository.FetchAsync(x => x.ProjectId == project.Id);
                foreach (var projectUser in projectUserList)
                {
                    var user = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id == projectUser.UserId && x.SlackUserId != null);
                    if (user != null)
                    {
                        var userAc = new UserAc();
                        var userAC = _mapperContext.Map<ApplicationUser, UserAc>(user);
                        userAcList.Add(userAc);
                    }
                }

            }
            if (userAcList == null)
                throw new UserNotFound();
            else
                return userAcList;
        }

        /// <summary>
        /// The method is used to get list of projects along with its users for a specific teamleader 
        /// </summary>
        /// <param name="teamLeaderId"></param>
        /// <returns>list of projects with users for a specific teamleader</returns>
        public async Task<List<UserAc>> GetProjectUsersByTeamLeaderIdAsync(string teamLeaderId)
        {
            List<UserAc> projectUsers = new List<UserAc>();
            //Get projects for that specific teamleader
            List<Project> projects = (await _projectDataRepository.FetchAsync(x => x.TeamLeaderId.Equals(teamLeaderId))).ToList();

            if (projects.Any())
            {
                //Get details of teamleader
                ApplicationUser teamLeader = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id.Equals(teamLeaderId));
                if (teamLeader != null)
                {
                    UserAc projectTeamLeader = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
                    projectTeamLeader.Role = _stringConstant.TeamLeader;
                    projectUsers.Add(projectTeamLeader);
                }

                //Get details of employees for projects with that particular teamleader 
                foreach (var project in projects)
                {
                    List<ProjectUser> projectUsersList = (await _projectUserRepository.FetchAsync(x => x.ProjectId == project.Id)).ToList();
                    foreach (var projectUser in projectUsersList)
                    {
                        ApplicationUser user = await _applicationUserDataRepository.FirstOrDefaultAsync(x => x.Id.Equals(projectUser.UserId));
                        if (user != null)
                        {
                            var Roles = (await _userManager.GetRolesAsync(user)).First();
                            UserAc employee = _mapperContext.Map<ApplicationUser, UserAc>(user);
                            employee.Role = Roles;
                            //Checking if employee is already present in the list or not
                            if (!projectUsers.Any(x => x.Id == employee.Id))
                            {
                                projectUsers.Add(employee);
                            }
                        }
                    }
                }
            }
            return projectUsers;
        }
        #endregion

        #region Private Methods


        /// <summary>
        /// Get slack user details of the given slack id from slack server 
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns></returns>
        private async Task<SlackUserDetailAc> GetSlackUserById(string slackUserId)
        {
            _logger.LogInformation("User Repository - GetSlackUserByI. Url: " + _appSettingUtil.Value.PromactErpUrl);
            var responseResult = await _httpClientRepository.GetAsync(_appSettingUtil.Value.PromactErpUrl, _stringConstant.SlackUserByIdUrl + slackUserId);
            _logger.LogInformation("User Repository: GetSlackUserById. ReponseResult: " + responseResult);
            // Transforming Json String to object type List of SlackUserDetailAc
            var data = JsonConvert.DeserializeObject<SlackUserDetailAc>(responseResult);
            return data;
        }


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
        private async Task<UserAc> GetUserAsync(ApplicationUser user)
        {
            //Gets a list of roles the specified user belongs to
            string roles = (await _userManager.GetRolesAsync(user)).First();
            UserAc newUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
            //assign role
            if (String.Compare(roles, _stringConstant.Admin, true) == 0)
            {
                newUser.Role = roles;
            }
            else if (String.Compare(roles, _stringConstant.Employee, true) == 0)
            {
                Project project = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId.Equals(user.Id));
                if (project != null)
                {
                    newUser.Role = _stringConstant.TeamLeader;
                }
                else
                {
                    newUser.Role = _stringConstant.Employee;
                }
            }
            return newUser;
        }

        /// <summary>
        /// This method is used to send email to the currently added user
        /// </summary>
        /// <param name="user">Object of newly registered User</param>
        private bool SendEmail(ApplicationUser user, string password)
        {
            string path = _hostingEnvironment.ContentRootPath + _stringConstant.UserDetialTemplateFolderPath;
            string finaleTemplate = "";
            if (System.IO.File.Exists(path))
            {
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

        /// <summary>
        /// Calculat casual leava and sick leave from the date of joining
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private LeaveCalculator CalculateAllowedLeaves(DateTime dateTime)
        {
            double casualAllowed = 0;
            double sickAllowed = 0;
            var day = dateTime.Day;
            var month = dateTime.Month;
            var year = dateTime.Year;
            double casualAllow = _appSettingUtil.Value.CasualLeave;
            double sickAllow = _appSettingUtil.Value.SickLeave;
            if (year >= DateTime.Now.Year)
            {
                double totalDays = (DateTime.Now - Convert.ToDateTime(dateTime)).TotalDays;
                //If an employee joins between 1st to 15th of month, then he/she will be eligible for that particular month's leaves 
                //and if he/she joins after 16th of month, he/she will not be eligible for that month's leaves.

                //In Our Project we consider Leave renewal on 1st april
                if (month >= 4)
                {
                    //if first 15 days of month april to December then substact 4 other wise substact 3
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
                    //if first 15 days of month January to March then add 8 other wise add 9
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

                // If calculated casualAllowed is exact 0.5 then it's considered half day casual leave
                if (casualAllowed % 1!=0)
                {
                    double CasualAlloweddecimal = casualAllowed - Math.Floor(casualAllowed);
                    if (CasualAlloweddecimal != 0.5) { casualAllowed = Convert.ToInt32(casualAllowed); }
                }

                // If calculated sickAllowed is exact 0.5 then it's considered half day sick leave 
                // If calculated sickAllowed is more than  0.90 then add one leave in sick leave 
                if (sickAllowed % 1 !=0)
                {
                    double sickAlloweddecimal= sickAllowed - Math.Floor(sickAllowed);
                    if (sickAlloweddecimal != 0.5) { sickAllowed = Convert.ToInt32(Math.Floor(sickAllowed)); }
                    if (sickAlloweddecimal > 0.90) { sickAllowed = sickAllowed + 1; }

                }
            }
            else
            {
                casualAllow = _appSettingUtil.Value.CasualLeave;
                sickAllowed = _appSettingUtil.Value.SickLeave;
            }
            LeaveCalculator calculate = new LeaveCalculator
            {
                CasualLeave = casualAllowed,
                SickLeave = sickAllowed
            };
            return calculate;
        }

        #endregion


    }
}