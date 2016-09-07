using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Data_Repository;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Promact.Oauth.Server.Models.ManageViewModels;
using Promact.Oauth.Server.Services;
using AutoMapper;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.Constants;
using Microsoft.AspNetCore.Hosting;
using Promact.Oauth.Server.Data;

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
        #endregion

        #region "Constructor"

        public UserRepository(IDataRepository<ApplicationUser> applicationUserDataRepository, IHostingEnvironment hostingEnvironment, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IMapper mapperContext, IDataRepository<ProjectUser> projectUserRepository, IProjectRepository projectRepository, IDataRepository<Project> projectDataRepository )
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
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// This method is used to add new user
        /// </summary>
        /// <param name="applicationUser">UserAc Application class object</param>
        public async Task<string> AddUser(UserAc newUser, string createdBy)
        {
            try
            {
                LeaveCalculator LC = new LeaveCalculator();
                LC = CalculateAllowedLeaves(Convert.ToDateTime(newUser.JoiningDate));
                newUser.NumberOfCasualLeave = LC.CasualLeave;
                newUser.NumberOfSickLeave = LC.SickLeave;
                var user = _mapperContext.Map<UserAc, ApplicationUser>(newUser);
                user.UserName = user.Email;
                user.CreatedBy = createdBy;
                user.CreatedDateTime = DateTime.UtcNow;

                await _userManager.CreateAsync(user, "User@123");
                await _userManager.AddToRoleAsync(user, newUser.RoleName);
               //SendEmail(user);
                return user.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calculat casual leava and sick leave for the date of joining
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public LeaveCalculator CalculateAllowedLeaves(DateTime dateTime)
        {
            double casualAllowed = 0;
            double sickAllowed = 0;
            var day = dateTime.Day;
            var month = dateTime.Month;
            var year = dateTime.Year;
            double casualAllow = 14;
            double sickAllow = 7;
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
            LeaveCalculator calculate = new LeaveCalculator
            {
                CasualLeave = Convert.ToInt32(casualAllowed),
                SickLeave = Convert.ToInt32(sickAllowed)
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
        public async Task<IEnumerable<UserAc>> GetAllUsers()
        {
            var users = await _applicationUserDataRepository.GetAll().ToListAsync();
            var userList = new List<UserAc>();
            foreach (var user in users)
            {
                var listItem = _mapperContext.Map<ApplicationUser, UserAc>(user);

                userList.Add(listItem);
            }
            return userList;
        }


        /// <summary>
        /// This method is used to edit the details of an existing user
        /// </summary>
        /// <param name="editedUser">UserAc Application class object</param>
        public string UpdateUserDetails(UserAc editedUser, string updatedBy)
        {
            try
            {
                var slackUserName = _applicationUserDataRepository.FirstOrDefault(x => x.Id != editedUser.Id && x.SlackUserName == editedUser.SlackUserName);
                if (slackUserName == null)
                {
                    var user = _userManager.FindByIdAsync(editedUser.Id).Result;

                    user.FirstName = editedUser.FirstName;
                    user.LastName = editedUser.LastName;
                    user.Email = editedUser.Email;
                    user.IsActive = editedUser.IsActive;
                    user.UpdatedBy = updatedBy;
                    user.UpdatedDateTime = DateTime.UtcNow;
                    user.NumberOfCasualLeave = editedUser.NumberOfCasualLeave;
                    user.NumberOfSickLeave = editedUser.NumberOfSickLeave;
                    user.SlackUserName = editedUser.SlackUserName;
                    _userManager.UpdateAsync(user).Wait();
                    _applicationUserDataRepository.Save();
                    //IList<string> listofUserRole = _userManager.GetRolesAsync(user).Result;
                    //var removeFromRole = _userManager.RemoveFromRoleAsync(user, listofUserRole.FirstOrDefault()).Result;
                    //var addNewRole = _userManager.AddToRoleAsync(user, editedUser.RoleName).Result;
                    return user.Id;
                }
                else { return ""; }
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
                var user = await _applicationUserDataRepository.FirstOrDefaultAsync(u => u.Id == id);
                var requiredUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
                IList<string> identityUserRole = _userManager.GetRolesAsync(user).Result;
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
        public string ChangePassword(ChangePasswordViewModel passwordModel)
        {
            var user = _userManager.FindByEmailAsync(passwordModel.Email).Result;
            if (user != null)
            {
                _userManager.ChangePasswordAsync(user, passwordModel.OldPassword, passwordModel.NewPassword).Wait();
            }
            return passwordModel.NewPassword;
        }

        /// <summary>
        /// This method is used to check if a user already exists in the database with the given userName
        /// </summary>
        /// <param name="userName">string userName</param>
        /// <returns> boolean: true if the user name exists, false if does not exist</returns>
        public bool FindByUserName(string userName)
        {
            var user = _userManager.FindByNameAsync(userName).Result;
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
        public UserAc GetUserDetail(string UserName)
        {
            try
            {
                var user = _userManager.FindByNameAsync(UserName).Result;
                var userAc = new UserAc();
                if (user != null)
                {
                    userAc.Email = user.Email;
                    userAc.Id = user.Id;
                    userAc.FirstName = user.FirstName;
                    userAc.LastName = user.LastName;
                    userAc.UserName = user.UserName;
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
        public bool FindByEmail(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user == null)
            {
                return false;
            }
            return true;
        }


        public bool FindUserBySlackUserName(string slackUserName)
        {
            var user = _applicationUserDataRepository.FirstOrDefault(x => x.SlackUserName == slackUserName);
            if (user != null)
            {
                if (user.SlackUserName == slackUserName)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This method is used to send email to the currently added user
        /// </summary>
        /// <param name="user">Object of newly registered User</param>
        public void SendEmail(ApplicationUser user)
        {
            //string path = _hostingEnvironment.ContentRootPath + StringConstant.UserDetialTemplateFolderPath;
            //string finaleTemplate = "";
            //if (System.IO.File.Exists(path))
            //{
            //    finaleTemplate = System.IO.File.ReadAllText(path);
            //    finaleTemplate = finaleTemplate.Replace(StringConstant.UserEmail, user.Email).Replace(StringConstant.UserPassword, StringConstant.DefaultUserPassword).Replace(StringConstant.ResertPasswordUserName, user.FirstName);
            //    _emailSender.SendEmailAsync(user.Email, StringConstant.LoginCredentials, finaleTemplate);
            //}
        }


        /// <summary>
        /// Method to get user details by user first name
        /// </summary>
        /// <param name="firstname"></param>
        /// <returns>user details</returns>
        public ApplicationUser UserDetialByFirstName(string firstname)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.FirstName == firstname);
            var newUser = new ApplicationUser
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return newUser;
        }

        /// <summary>
        /// Method to get team leader's details by user firstname
        /// </summary>
        /// <param name="userFirstName"></param>
        /// <returns>list of team leader</returns>
        public async Task<List<ApplicationUser>> TeamLeaderByUserId(string userFirstName)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.FirstName == userFirstName);
            var projects = _projectUserRepository.Fetch(x => x.UserId == user.Id);
            List<ApplicationUser> teamLeaders = new List<ApplicationUser>();
            foreach (var project in projects)
            {
                var teamLeaderId = await _projectRepository.GetById(project.Id);
                var teamLeader = teamLeaderId.TeamLeaderId;
                user = await _userManager.FindByIdAsync(teamLeader);
                //user = _userManager.Users.FirstOrDefault(x => x.Id == teamLeader);
                var newUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email
                };
                teamLeaders.Add(newUser);
            }
            return teamLeaders;
        }

        /// <summary>
        /// Method to get management people details
        /// </summary>
        /// <returns>list of management</returns>
        public async Task<List<ApplicationUser>> ManagementByUserId()
        {
            var management = await _userManager.GetUsersInRoleAsync("Admin");
            List<ApplicationUser> managementUser = new List<ApplicationUser>();
            foreach (var user in management)
            {
                var newUser = new ApplicationUser
                {
                    FirstName = user.FirstName,
                    Email = user.Email
                };
                managementUser.Add(newUser);
            }
            return managementUser;
        }


        /// <summary>
        /// This method is used to Get User details by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>details of user</returns>
        public  UserAc UserDetailById(string userId)
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
        /// Method is used to return a user after assigning a role and mapping from ApplicationUser class to UserAc class
        /// </summary>
        /// <param name="user"></param>
        /// <returns>user</returns>
        private UserAc GetUser(ApplicationUser user)
        {
            if (user != null)
            {
                var Roles = _userManager.GetRolesAsync(user).Result.First();
                if (Roles.Equals(StringConstant.Admin))
                {
                    var newUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
                    newUser.Role = Roles;
                    return newUser;
                }
                if (Roles.Equals(StringConstant.Employee))
                {
                    var project = _projectDataRepository.FirstOrDefault(x => x.TeamLeaderId.Equals(user.Id));
                    if (project != null)
                    {
                        var newUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
                        newUser.Role = StringConstant.TeamLeader;
                        return newUser;
                    }
                    else
                    {
                        var newUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
                        newUser.Role = StringConstant.Employee;
                        return newUser;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}