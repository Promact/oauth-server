using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using Promact.Oauth.Server.Models;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Constants;
using Microsoft.Extensions.Logging;
using Promact.Oauth.Server.Exception_Handler;

namespace Promact.Oauth.Server.Repository.ProjectsRepository
{
    public class ProjectRepository : IProjectRepository
    {
        #region "Private Variable(s)"
        private readonly IDataRepository<Project> _projectDataRepository;
        private readonly IDataRepository<ProjectUser> _projectUserDataRepository;
        private readonly IDataRepository<ApplicationUser> _userDataRepository;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringConstant _stringConstant;
        private readonly IMapper _mapperContext;
        private readonly ILogger<ProjectRepository> _logger;
        #endregion

        #region "Constructor"
        public ProjectRepository(IDataRepository<Project> projectDataRepository, IDataRepository<ProjectUser> projectUserDataRepository, IDataRepository<ApplicationUser> userDataRepository, UserManager<ApplicationUser> userManager, 
            IMapper mapperContext,IStringConstant stringConstant, ILogger<ProjectRepository> logger)
        {
            _projectDataRepository = projectDataRepository;
            _projectUserDataRepository = projectUserDataRepository;
            _userDataRepository = userDataRepository;
            _mapperContext = mapperContext;
            _userManager = userManager;
            _stringConstant = stringConstant;
            _logger = logger;
        }
        #endregion

        /// <summary>
        /// Method to return list of projects 
        /// </summary>
        /// <returns>list of projects</returns>
        public async Task<IEnumerable<ProjectAc>> GetAllProjects()
        {
            var projects = await _projectDataRepository.GetAll().ToListAsync();
            var projectAcList = new List<ProjectAc>();

            foreach(var project in projects)
            {
                var userAc = new UserAc();
                if (!string.IsNullOrEmpty(project.TeamLeaderId))
                {
                    var user = _userDataRepository.FirstOrDefault(x => x.Id.Equals(project.TeamLeaderId));
                    userAc = _mapperContext.Map<ApplicationUser, UserAc>(user);
                }
                else
                {
                    userAc.FirstName = _stringConstant.TeamLeaderNotAssign;
                    userAc.LastName = _stringConstant.TeamLeaderNotAssign;
                    userAc.Email = _stringConstant.TeamLeaderNotAssign;
                }
                var CreatedBy = _userDataRepository.FirstOrDefault(x => x.Id == project.CreatedBy)?.FirstName;
                var UpdatedBy = _userDataRepository.FirstOrDefault(x => x.Id == project.UpdatedBy)?.FirstName;
                string UpdatedDate;
                if (project.UpdatedDateTime == null)
                { UpdatedDate = ""; }
                else
                { UpdatedDate = Convert.ToDateTime(project.UpdatedDateTime).ToString(_stringConstant.DateFormate); }
                var projectAc = _mapperContext.Map<Project, ProjectAc>(project);
                projectAc.TeamLeader = userAc;
                projectAc.CreatedBy = CreatedBy;
                projectAc.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.DateFormate);
                projectAc.UpdatedBy = UpdatedBy;
                projectAc.UpdatedDate = UpdatedDate;
                projectAcList.Add(projectAc);

            }
            return projectAcList;
        }

        /// <summary>
        /// Method to add new project in the database
        /// </summary>
        /// <param name="newProject">project that need to be added</param>
        /// <param name="createdBy">login user id</param>
        /// <returns>project id of newly created project</returns>
        public async Task<int> AddProject(ProjectAc newProject, string createdBy)
        {
            var projectAc = _mapperContext.Map<ProjectAc, Project>(newProject);
            projectAc.CreatedDateTime = DateTime.UtcNow;
            projectAc.CreatedBy = createdBy;
            projectAc.ApplicationUsers = null;
            _projectDataRepository.AddAsync(projectAc);
            await _projectDataRepository.SaveChangesAsync();
            return projectAc.Id;
        }

        /// <summary>
        /// Method to add user id and project id in userproject table
        /// </summary>
        /// <param name="newProjectUser"></param>ProjectId and UserId information that need to be added
        public async Task AddUserProject(ProjectUser newProjectUser)
        {
            _projectUserDataRepository.AddAsync(newProjectUser);
            await _projectDataRepository.SaveChangesAsync();

        }

        /// <summary>
        /// Method to return the project details of the given id 
        /// </summary>
        /// <param name="id"></param>Project id that need to be fetch the Project and list of users
        /// <returns></returns>Project and User/Users infromation 
        /// 
        public async Task<ProjectAc> GetById(int id)
        {
            List<UserAc> userAcList = new List<UserAc>();
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id==id);
            IEnumerable<ProjectUser> projectUserList =await _projectUserDataRepository.FetchAsync(y => y.ProjectId==project.Id);
            foreach (ProjectUser projectUsers in projectUserList)
            {
                var applicationUser =await _userDataRepository.FirstOrDefaultAsync(z => z.Id==projectUsers.UserId);
                userAcList.Add(new UserAc
                {
                    Id = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    Email = applicationUser.Email,
                    LastName = applicationUser.LastName
                });
            }
            var projectAc = _mapperContext.Map<Project, ProjectAc>(project);
            if (!string.IsNullOrEmpty(project.TeamLeaderId))
            {var teamLeader =await _userDataRepository.FirstOrDefaultAsync(x => x.Id.Equals(project.TeamLeaderId));
            projectAc.TeamLeader = new UserAc { FirstName = teamLeader.FirstName, LastName = teamLeader.LastName, Email = teamLeader.Email };}
            else{ projectAc.TeamLeader = null;}
            projectAc.ApplicationUsers = userAcList.OrderBy(y => y.FirstName).ToList();
            
            if (projectAc==null)
                throw new ProjectNotFound();
            else
                return projectAc;
        }

        /// <summary>
        /// Method to update project information 
        /// </summary>
        /// <param name="editProject"></param>Updated information in editProject Parmeter
        /// <param name="updatedBy"></param>Login User Id
        public async Task<int> EditProject(ProjectAc editProject, string updatedBy)
        {
            var projectId = editProject.Id;
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id==projectId);
            project.IsActive = editProject.IsActive;
            project.Name = editProject.Name;
            project.TeamLeaderId = editProject.TeamLeaderId;
            project.SlackChannelName = editProject.SlackChannelName;
            project.UpdatedDateTime = DateTime.UtcNow;
            project.UpdatedBy = updatedBy;
            _projectDataRepository.UpdateAsync(project);
            await _projectDataRepository.SaveChangesAsync();

            //Delete old users from project user table
            _projectUserDataRepository.Delete(x => x.ProjectId==projectId);
            await _projectUserDataRepository.SaveChangesAsync();

            foreach (var user in editProject.ApplicationUsers)
            {
                _projectUserDataRepository.AddAsync(new ProjectUser
                {
                    ProjectId = project.Id,
                    UpdatedDateTime = DateTime.UtcNow,
                    UpdatedBy = updatedBy,
                    CreatedBy = project.CreatedBy,
                    CreatedDateTime = project.CreatedDateTime,
                    UserId = user.Id,
                });
            }
            
            await _projectDataRepository.SaveChangesAsync();
            return editProject.Id;
        }

        /// <summary>
        /// Method to check Project and SlackChannelName is already exists or not 
        /// </summary>
        /// <param name="project"></param> pass the project parameter
        /// <returns>projectAc object</returns>
        public ProjectAc CheckDuplicate(ProjectAc projectAc)
        {
            var projectName=new Project();
            var slackChannelName=new Project();
            if (projectAc.Id==0)
            {
                projectName = _projectDataRepository.FirstOrDefault(x => x.Name == projectAc.Name );
                slackChannelName = _projectDataRepository.FirstOrDefault(x => x.SlackChannelName == projectAc.SlackChannelName);
            }
            else
            {
                projectName = _projectDataRepository.FirstOrDefault(x => x.Id != projectAc.Id && x.Name == projectAc.Name);
                slackChannelName = _projectDataRepository.FirstOrDefault(x => x.Id != projectAc.Id && x.SlackChannelName == projectAc.SlackChannelName);
            }
            if (projectName==null && slackChannelName==null)
            { return projectAc; }
            else if (projectName != null && slackChannelName==null)
            { projectAc.Name = null; return projectAc; }
            else if (projectName==null && slackChannelName != null)
            { projectAc.SlackChannelName = null; return projectAc; }
            else
            { projectAc.Name = null; projectAc.SlackChannelName = null; return projectAc; }
        }

        /// <summary>
        /// Method to return the project details of the given GroupName
        /// </summary>
        /// <param name="GroupName"></param>
        /// <returns>object of Project</returns>
        public async Task<ProjectAc> GetProjectByGroupName(string GroupName)
        {
                var project =await _projectDataRepository.FirstOrDefaultAsync(x => x.SlackChannelName==GroupName);
                var projectAc = new ProjectAc();
                if (project != null)
                {
                    projectAc.CreatedBy = project.CreatedBy;
                    projectAc.Id = project.Id;
                    projectAc.IsActive = project.IsActive;
                    projectAc.Name = project.Name;
                    projectAc.SlackChannelName = project.SlackChannelName;
                    projectAc.TeamLeaderId = project.TeamLeaderId;
                    project.UpdatedBy = project.UpdatedBy;
                }
                if (projectAc==null)
                    throw new ProjectNotFound();
                else
                    return projectAc;
          }

        /// <summary>
        /// Method to return list of users/employees of the given group name. - JJ
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="UserName"></param>
        /// <returns>list of object of UserAc</returns>
        public async Task<List<UserAc>> GetProjectUserByGroupName(string GroupName)
        {
            
                var project =await _projectDataRepository.FirstOrDefaultAsync(x => x.SlackChannelName==GroupName);
                var userAcList = new List<UserAc>();
                if (project != null)
                {
                    var projectUserList = await _projectUserDataRepository.FetchAsync(x => x.ProjectId==project.Id);
                    foreach (var projectUser in projectUserList)
                    {
                        var user =await _userDataRepository.FirstOrDefaultAsync(x => x.Id==projectUser.UserId);
                        var userAc = new UserAc();
                        userAc.Id = user.Id;
                        userAc.Email = user.Email;
                        userAc.FirstName = user.FirstName;
                        userAc.IsActive = user.IsActive;
                        userAc.LastName = user.LastName;
                        userAc.UserName = user.UserName;
                        userAc.SlackUserName = user.SlackUserName;
                        userAcList.Add(userAc);
                    }

                }
                if (userAcList==null)
                    throw new UserNotFound();
                else
                    return userAcList;
        }

        /// <summary>
        /// Method to return all project for specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectAc>> GetAllProjectForUser(string userId)
        {
            var projects =await _projectDataRepository.FetchAsync(x => x.TeamLeaderId == userId);
            _logger.LogInformation("Total Projects " + projects.Count().ToString()); 
            var projectUser =await _projectUserDataRepository.FetchAsync(x => x.UserId == userId);
            _logger.LogInformation("Total UserProjects " + projectUser.Count().ToString());
            List<ProjectAc> projectAcList = new List<ProjectAc>();
            ProjectAc projectAc = new ProjectAc();
            if (projects != null)
            {
                foreach (var project in projects)
                {
                    var teamLeader = await _userManager.FindByIdAsync(project.TeamLeaderId);
                    var projectMapper = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
                    projectAc.Name = project.Name;
                    projectAc.SlackChannelName = project.SlackChannelName;
                    projectAc.TeamLeader = projectMapper;
                    projectAc.IsActive = project.IsActive;
                    projectAcList.Add(projectAc);
                }
            }
            foreach (var project in projectUser)
            {
                projectAc = new ProjectAc();
                var projectDetails = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id == project.ProjectId);
                var teamLeader = await _userManager.FindByIdAsync(projectDetails.TeamLeaderId);
                var projectMapper = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
                projectAc.Name = projectDetails.Name;
                projectAc.SlackChannelName = projectDetails.SlackChannelName;
                projectAc.TeamLeader = projectMapper;
                projectAc.IsActive = projectDetails.IsActive;
                projectAcList.Add(projectAc);
            }
            _logger.LogInformation("Total recentProjects " + projectAcList.Count().ToString());
            return projectAcList;
        }

        /// <summary>
        /// Method to return list of users.
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetListOfEmployee(string slackUserId)
        {
            ApplicationUser applicationUser = await _userDataRepository.FirstOrDefaultAsync(x => x.UserName == name);
            var userRolesAcList = new List<UserRoleAc>();
            var usersRoleAc = new UserRoleAc();
            usersRoleAc.UserName = applicationUser.UserName;
            usersRoleAc.Role = _stringConstant.RoleTeamLeader;
            usersRoleAc.Name = applicationUser.FirstName + " " + applicationUser.LastName;
            userRolesAcList.Add(usersRoleAc);
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId == applicationUser.Id);
            var projectUserList = await _projectUserDataRepository.FetchAsync(x => x.ProjectId==project.Id);

            foreach (var projectUser in projectUserList)
            {
                var usersRolesAc = new UserRoleAc();
                var users = await _userDataRepository.FirstOrDefaultAsync(x => x.Id == projectUser.UserId);
                usersRolesAc.UserName = users.UserName;
                usersRolesAc.Name = users.FirstName + " " + users.LastName;
                usersRolesAc.Role = _stringConstant.RoleAdmin;
                userRolesAcList.Add(usersRolesAc);
            }
            return userRolesAcList;
        }

        /// <summary>
        /// Method to return user role
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetUserRole(string slackUserId)
        {
            ApplicationUser applicationUser = await _userDataRepository.FirstOrDefaultAsync(x => x.UserName == name);
            var role = await _userManager.GetRolesAsync(applicationUser);
            var userRole = role.First();
            var UserRoleAcList = new List<UserRoleAc>();
            if (userRole==_stringConstant.RoleAdmin)
            {
                var userRoleAdmin = new UserRoleAc();
                userRoleAdmin.UserName = applicationUser.UserName;
                userRoleAdmin.Name = applicationUser.FirstName + " " + applicationUser.LastName;
                userRoleAdmin.Role = userRole;
                UserRoleAcList.Add(userRoleAdmin);
                var userList =await _userDataRepository.GetAll().ToListAsync();
                foreach (var userDetails in userList)
                {
                    var roles = await _userManager.GetRolesAsync(userDetails);
                    if (roles.Count() != 0 && roles[0] == _stringConstant.RoleEmployee)
                    {
                        var userRoleAc = new UserRoleAc();
                        userRoleAc.UserName = userDetails.UserName;
                        userRoleAc.Name = userDetails.FirstName + " " + userDetails.LastName;
                        userRoleAc.Role = userRole;
                        UserRoleAcList.Add(userRoleAc);
                    }

                }
            }
            else
            {
                var project =await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId==applicationUser.Id);
                if (project==null)
                {
                    var usersRolesAc = new UserRoleAc();
                    usersRolesAc.UserName = applicationUser.UserName;
                    usersRolesAc.Role = _stringConstant.RoleEmployee;
                    usersRolesAc.Name = applicationUser.FirstName + " " + applicationUser.LastName;
                    UserRoleAcList.Add(usersRolesAc);
                }
                else
                {
                    var usersRoleAc = new UserRoleAc();
                    usersRoleAc.UserName = applicationUser.UserName;
                    usersRoleAc.Role = _stringConstant.RoleTeamLeader;
                    usersRoleAc.Name = applicationUser.FirstName + " " + applicationUser.LastName;
                    UserRoleAcList.Add(usersRoleAc);
                }
            }
            if (UserRoleAcList==null)
                throw new UserRoleNotFound();
            else
                return UserRoleAcList;
            
        }

        /// <summary>
        /// Method to return list of projects along with the users and teamleader in a project
        /// </summary>
        /// <returns>List of projects along with users</returns>
        public async Task<IList<ProjectAc>> GetProjectsWithUsers()
        {
            List<ProjectAc> projectAcList = new List<ProjectAc>();
            var projects = await _projectDataRepository.GetAll().ToListAsync();

            foreach(var project in projects)
            {
                ApplicationUser applicationUser = await _userDataRepository.FirstOrDefaultAsync(x => x.Id == project.TeamLeaderId);
                UserAc teamLeader = _mapperContext.Map<ApplicationUser, UserAc>(applicationUser);
                teamLeader.Role = _stringConstant.TeamLeader;

                List<ProjectUser> projectUsers = await _projectUserDataRepository.Fetch(x => x.ProjectId == project.Id).ToListAsync();
                ProjectAc projectAc = _mapperContext.Map<Project, ProjectAc>(project);
                projectAc.TeamLeader = teamLeader;
                projectAc.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.Format);
                foreach (var projectUser in projectUsers)
                {
                    ApplicationUser user = await _userDataRepository.FirstOrDefaultAsync(x => x.Id == projectUser.UserId);
                    UserAc userAc = _mapperContext.Map<ApplicationUser, UserAc>(user);
                    userAc.Role = _stringConstant.Employee;
                    projectAc.ApplicationUsers.Add(userAc);
                }
                projectAcList.Add(projectAc);
            }
            return projectAcList;
        }

        /// <summary>
        /// Method to return project details by using projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Project details along with users</returns>
        public async Task<ProjectAc> GetProjectDetails(int projectId)
        {
            Project project =await _projectDataRepository.FirstOrDefaultAsync(x => x.Id==projectId);
            ApplicationUser applicationUser =await _userDataRepository.FirstOrDefaultAsync(x => x.Id==project.TeamLeaderId);
            UserAc teamLeader = _mapperContext.Map<ApplicationUser, UserAc>(applicationUser);
            teamLeader.Role = _stringConstant.TeamLeader;
            IEnumerable<ProjectUser> projectUsers = await _projectUserDataRepository.FetchAsync(x => x.ProjectId==project.Id);
            ProjectAc projectAc = _mapperContext.Map<Project, ProjectAc>(project);
            projectAc.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.Format);
            projectAc.TeamLeader = teamLeader;
            List<UserAc> projectUserList = new List<UserAc>();
            foreach (var projectUser in projectUsers)
            {
                ApplicationUser user =await _userDataRepository.FirstOrDefaultAsync(x => x.Id==projectUser.UserId);
                UserAc userAc = _mapperContext.Map<ApplicationUser, UserAc>(user);
                userAc.Role = _stringConstant.Employee;
                projectAc.ApplicationUsers.Add(userAc);
            }

            return projectAc;
        }
    }
}
