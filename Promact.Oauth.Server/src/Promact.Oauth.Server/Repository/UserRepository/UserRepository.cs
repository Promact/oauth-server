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

namespace Promact.Oauth.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        #region "Private Variable(s)"

        private readonly IDataRepository<ApplicationUser> _applicationUserDataRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapperContext;
        private readonly IDataRepository<ProjectUser> _projectUserRepository;
        private readonly IProjectRepository _projectRepository;

        #endregion

        #region "Constructor"

        public UserRepository(IDataRepository<ApplicationUser> applicationUserDataRepository, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IMapper mapperContext,IDataRepository<ProjectUser> projectUserRepository, IProjectRepository projectRepository)
        {
            _applicationUserDataRepository = applicationUserDataRepository;
            _userManager = userManager;
            _emailSender = emailSender;
            _mapperContext = mapperContext;
            _projectUserRepository = projectUserRepository;
            _projectRepository = projectRepository;
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// This method is used to add new user
        /// </summary>
        /// <param name="applicationUser">UserAc Application class object</param>
        public string AddUser(UserAc newUser, string createdBy)
        {
            var user = _mapperContext.Map<UserAc, ApplicationUser>(newUser);
            user.UserName = user.Email;
            user.CreatedBy = createdBy;
            user.CreatedDateTime = DateTime.UtcNow;

            _userManager.CreateAsync(user, "User@123").Wait();
            _userManager.AddToRoleAsync(user, "Employee").Wait();
            //SendEmail(user);
            return user.Id;
        }


        /// <summary>
        /// This method is used for getting the list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        public IEnumerable<UserAc> GetAllUsers()
        {
            var users = _applicationUserDataRepository.List().ToList();
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
            var user = _userManager.FindByIdAsync(editedUser.Id).Result;

            user.FirstName = editedUser.FirstName;
            user.LastName = editedUser.LastName;
            user.Email = editedUser.Email;
            user.IsActive = editedUser.IsActive;
            user.UpdatedBy = updatedBy;
            user.UpdatedDateTime = DateTime.UtcNow;

            _userManager.UpdateAsync(user).Wait();
            _applicationUserDataRepository.Save();

            return user.Id;
        }



        /// <summary>
        /// This method is used to get particular user's details by his/her id
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>UserAc Application class object</returns>
        public UserAc GetById(string id)
        {
            try
            {
                var user = _applicationUserDataRepository.FirstOrDefault(u => u.Id == id);
                var requiredUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
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


        /// <summary>
        /// This method is used to send email to the currently added user
        /// </summary>
        /// <param name="user">Object of newly registered User</param>
        public void SendEmail(ApplicationUser user)
        {
            //Create a new message for the email with the required content
            var message = "Welcome to Promact Infotech Private Limited \n"
                            + "Email: " + user.Email
                            + "\n Password: User@123"
                            + "\n Link: ";
            _emailSender.SendEmailAsync(user.Email, "Login Credentials", message);
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
                user = _userManager.Users.FirstOrDefault(x => x.Id == teamLeader);
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

        #endregion
    }
}