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
using Promact.Oauth.Server.Exception_Handler;
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
            IHttpClientRepository httpClientRepository)
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
            //try
            //{
            LeaveCalculator LC = new LeaveCalculator();
            LC = CalculateAllowedLeaves(Convert.ToDateTime(newUser.JoiningDate));
            newUser.NumberOfCasualLeave = LC.CasualLeave;
            newUser.NumberOfSickLeave = LC.SickLeave;
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
            //}

            //catch (Exception ex)
            //{
            //    throw ex;
            //}
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
                var roles = _roleManager.Roles;
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
            var users = await _userManager.Users.Where(user => user.IsActive).OrderBy(user => user.FirstName).ToListAsync();
            return _mapperContext.Map<List<ApplicationUser>, List<UserAc>>(users);
        }


        /// <summary>
        /// This method is used to edit the details of an existing user
        /// </summary>
        /// <param name="editedUser">UserAc Application class object</param>
        public async Task<string> UpdateUserDetails(UserAc editedUser, string updatedBy)
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
            var user = await _userManager.FindByEmailAsync(passwordModel.Email);
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
            var user = await _userManager.FindByNameAsync(userName);
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
        public async Task<bool> CheckEmailIsExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null ? false : true;
        }


        /// <summary>
        /// Fetches user with the given Slack User Id
        /// </summary>
        /// <param name="slackUserName"></param>
        /// <returns></returns>
        public ApplicationUser FindUserBySlackUserName(string slackUserName)
        {
            var user = _applicationUserDataRepository.FirstOrDefault(x => x.SlackUserName == slackUserName);
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
            var user = await _userManager.FindByIdAsync(id);
            string newPassword = GetRandomString();
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
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
        public async Task<List<ApplicationUser>> TeamLeaderByUserSlackId(string userSlackId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.SlackUserId == userSlackId);
            var projects = _projectUserRepository.Fetch(x => x.UserId == user.Id);
            List<ApplicationUser> teamLeaders = new List<ApplicationUser>();
            foreach (var project in projects)
            {
                var teamLeaderId = await _projectRepository.GetById(project.ProjectId);
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
        public async Task<List<ApplicationUser>> ManagementDetails()
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
        public async Task<bool> IsAdmin(string slackUserId)
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
        public UserAc UserDetailById(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            return GetUser(user);
        }

        /// <summary>
        /// Method is used to get the details of user by using their username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>details of user</returns>
        public async Task<UserAc> GetUserDetailByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return GetUser(user);
        }


        /// <summary>
        /// Fetches the list of Slack User Details
        /// </summary>
        /// <returns></returns>
        public async Task<List<SlackUserDetailAc>> GetSlackUserDetails()
        {
            _logger.LogInformation("User Repository: GetSlackUserDetails. Url: " + _appSettingUtil.Value.PromactErpUrl);
            HttpResponseMessage response = await _httpClientRepository.GetAsync(_appSettingUtil.Value.PromactErpUrl, _stringConstant.SlackUsersUrl);
            string responseResult = response.Content.ReadAsStringAsync().Result;
            _logger.LogInformation("User Repository: GetSlackUserDetails. ReponseResult: " + responseResult);
            // Transforming Json String to object type List of SlackUserDetailAc
            var data = JsonConvert.DeserializeObject<List<SlackUserDetailAc>>(responseResult);
            return data;
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
            HttpResponseMessage response = await _httpClientRepository.GetAsync(_appSettingUtil.Value.PromactErpUrl, _stringConstant.SlackUserByIdUrl + slackUserId);
            string responseResult = response.Content.ReadAsStringAsync().Result;
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