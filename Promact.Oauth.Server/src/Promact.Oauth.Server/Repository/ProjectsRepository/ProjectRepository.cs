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
        /// <returns></returns>list of projects
        public async Task<IEnumerable<ProjectAc>> GetAllProjects()
        {
            var projects = await _projectDataRepository.GetAll().ToListAsync();
            var projectAcs = new List<ProjectAc>();

            projects.ForEach(project =>
            {
                var teamLeader = new UserAc();
                if (project.TeamLeaderId != null)
                {
                    var teamLeaders = _userDataRepository.FirstOrDefault(x => x.Id.Equals(project.TeamLeaderId));
                    teamLeader.FirstName = teamLeaders.FirstName;
                    teamLeader.LastName = teamLeaders.LastName;
                    teamLeader.Email = teamLeaders.Email;
                }
                else
                {

                    teamLeader.FirstName = _stringConstant.TeamLeaderNotAssign;
                    teamLeader.LastName = _stringConstant.TeamLeaderNotAssign;
                    teamLeader.Email = _stringConstant.TeamLeaderNotAssign;
                }
                var CreatedBy = _userDataRepository.FirstOrDefault(x => x.Id.Equals(project.CreatedBy))?.FirstName;
                var UpdatedBy = _userDataRepository.FirstOrDefault(x => x.Id.Equals(project.UpdatedBy))?.FirstName;
                string UpdatedDate;
                if (project.UpdatedDateTime.Equals(null))
                { UpdatedDate = ""; }
                else
                { UpdatedDate = Convert.ToDateTime(project.UpdatedDateTime).ToString(_stringConstant.DateFormate); }
                var projectObject = _mapperContext.Map<Project, ProjectAc>(project);
                projectObject.TeamLeader = teamLeader;
                projectObject.CreatedBy = CreatedBy;
                projectObject.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.DateFormate);
                projectObject.UpdatedBy = UpdatedBy;
                projectObject.UpdatedDate = UpdatedDate;
                projectAcs.Add(projectObject);

            });
            return projectAcs;
        }

        /// <summary>
        /// Method to add new project in the database
        /// </summary>
        /// <param name="newProject">project that need to be added</param>
        /// <param name="createdBy">login user id</param>
        /// <returns>project id of newly created project</returns>
        public async Task<int> AddProject(ProjectAc newProject, string createdBy)
        {
            var projectObject = _mapperContext.Map<ProjectAc, Project>(newProject);
            projectObject.CreatedDateTime = DateTime.UtcNow;
            projectObject.CreatedBy = createdBy;
            projectObject.ApplicationUsers = null;
            _projectDataRepository.AddAsync(projectObject);
            await _projectDataRepository.SaveChangesAsync();
            return projectObject.Id;
        }

        /// <summary>
        /// Method to add user id and project id in userproject table
        /// </summary>
        /// <param name="newProjectUser"></param>ProjectId and UserId information that need to be added
        public void AddUserProject(ProjectUser newProjectUser)
        {
            _projectUserDataRepository.Add(newProjectUser);
        }

        /// <summary>
        /// Method to return the project details of the given id 
        /// </summary>
        /// <param name="id"></param>Project id that need to be fetch the Project and list of users
        /// <returns></returns>Project and User/Users infromation 
        /// 
        public async Task<ProjectAc> GetById(int id)
        {
            List<UserAc> applicationUserList = new List<UserAc>();
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id.Equals(id));
            IEnumerable<ProjectUser> projectUserList =await _projectUserDataRepository.FetchAsync(y => y.ProjectId.Equals(project.Id));
            foreach (ProjectUser projectUsers in projectUserList)
            {
                var applicationUser = _userDataRepository.FirstOrDefault(z => z.Id.Equals(projectUsers.UserId));
                applicationUserList.Add(new UserAc
                {
                    Id = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    Email = applicationUser.Email,
                    LastName = applicationUser.LastName
                });
            }
            var projectAc = _mapperContext.Map<Project, ProjectAc>(project);
            if (project.TeamLeaderId != null)
            {var teamLeader = _userDataRepository.FirstOrDefault(x => x.Id.Equals(project.TeamLeaderId));
            projectAc.TeamLeader = new UserAc { FirstName = teamLeader.FirstName, LastName = teamLeader.LastName, Email = teamLeader.Email };}
            else{ projectAc.TeamLeader = null;}
            projectAc.ApplicationUsers = applicationUserList.OrderBy(y => y.FirstName).ToList();
            
            if (projectAc.Equals(null))
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
            var projectInDb = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id.Equals(projectId));
            projectInDb.IsActive = editProject.IsActive;
            projectInDb.Name = editProject.Name;
            projectInDb.TeamLeaderId = editProject.TeamLeaderId;
            projectInDb.SlackChannelName = editProject.SlackChannelName;
            projectInDb.UpdatedDateTime = DateTime.UtcNow;
            projectInDb.UpdatedBy = updatedBy;
            _projectDataRepository.UpdateAsync(projectInDb);
            await _projectDataRepository.SaveChangesAsync();

            //Delete old users from project user table
            _projectUserDataRepository.Delete(x => x.ProjectId.Equals(projectId));
            await _projectUserDataRepository.SaveChangesAsync();

            editProject.ApplicationUsers.ForEach(x =>
            {
                _projectUserDataRepository.Add(new ProjectUser
                {
                    ProjectId = projectInDb.Id,
                    UpdatedDateTime = DateTime.UtcNow,
                    UpdatedBy = updatedBy,
                    CreatedBy = projectInDb.CreatedBy,
                    CreatedDateTime = projectInDb.CreatedDateTime,
                    UserId = x.Id
                });
            });
            return editProject.Id;
        }

        /// <summary>
        /// Method to check project and slackChannelName is already exists or not 
        /// </summary>
        /// <param name="project"></param> pass the project parameter
        /// <returns>projectAc object</returns>
        public ProjectAc checkDuplicateFromEditProject(ProjectAc project)
        {
            var projectName = _projectDataRepository.FirstOrDefault(x => x.Id != project.Id && x.Name.Equals(project.Name));
            var slackChannelName = _projectDataRepository.FirstOrDefault(x => x.Id != project.Id && x.SlackChannelName.Equals(project.SlackChannelName));
            if (projectName.Equals(null) && slackChannelName.Equals(null))
            { return project; }
            else if (projectName != null && slackChannelName.Equals(null))
            { project.Name = null; return project; }
            else if (projectName.Equals(null) && slackChannelName.Equals(null))
            { project.SlackChannelName = null; return project; }
            else
            { project.Name = null; project.SlackChannelName = null; return project; }

        }

        /// <summary>
        /// Method to check Project and SlackChannelName is already exists or not 
        /// </summary>
        /// <param name="project"></param> pass the project parameter
        /// <returns>projectAc object</returns>
        public ProjectAc checkDuplicate(ProjectAc project)
        {
            var projectName = _projectDataRepository.FirstOrDefault(x => x.Name.Equals(project.Name));
            var slackChannelName = _projectDataRepository.FirstOrDefault(x => x.SlackChannelName.Equals(project.SlackChannelName));
            if (projectName.Equals(null) && slackChannelName.Equals(null))
            { return project; }
            else if (projectName != null && slackChannelName.Equals(null))
            { project.Name = null; return project; }
            else if (projectName.Equals(null) && slackChannelName != null)
            { project.SlackChannelName = null; return project; }
            else
            { project.Name = null; project.SlackChannelName = null; return project; }
        }

        /// <summary>
        /// Method to return the project details of the given GroupName
        /// </summary>
        /// <param name="GroupName"></param>
        /// <returns>object of Project</returns>
        public ProjectAc GetProjectByGroupName(string GroupName)
        {
                var project = _projectDataRepository.FirstOrDefault(x => x.SlackChannelName.Equals(GroupName));
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
                if (projectAc.Equals(null))
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
            
                var project =await _projectDataRepository.FirstOrDefaultAsync(x => x.SlackChannelName.Equals(GroupName));
                var userProjects = new List<UserAc>();
                if (project != null)
                {
                    var projectUserList = await _projectUserDataRepository.FetchAsync(x => x.ProjectId.Equals(project.Id));
                    foreach (var projectUser in projectUserList)
                    {
                        var user = _userDataRepository.FirstOrDefault(x => x.Id.Equals(projectUser.UserId));
                        var userAc = new UserAc();
                        userAc.Id = user.Id;
                        userAc.Email = user.Email;
                        userAc.FirstName = user.FirstName;
                        userAc.IsActive = user.IsActive;
                        userAc.LastName = user.LastName;
                        userAc.UserName = user.UserName;
                        userAc.SlackUserName = user.SlackUserName;
                        userProjects.Add(userAc);
                    }

                }
                if (userProjects.Equals(null))
                    throw new UserNotFound();
                else
                    return userProjects;
        }

        /// <summary>
        /// Method to return all project for specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectAc>> GetAllProjectForUser(string userId)
        {
            var projects = _projectDataRepository.Fetch(x => x.TeamLeaderId == userId);
            _logger.LogInformation("Total Projects " + projects.Count().ToString()); 
            var projectUser = _projectUserDataRepository.Fetch(x => x.UserId == userId);
            _logger.LogInformation("Total UserProjects " + projectUser.Count().ToString());
            List<ProjectAc> recentProjects = new List<ProjectAc>();
            ProjectAc recentProject = new ProjectAc();
            if (projects != null)
            {
                foreach (var project in projects)
                {
                    var teamLeader = await _userManager.FindByIdAsync(project.TeamLeaderId);
                    var projectMapper = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
                    recentProject.Name = project.Name;
                    recentProject.SlackChannelName = project.SlackChannelName;
                    recentProject.TeamLeader = projectMapper;
                    recentProject.IsActive = project.IsActive;
                    recentProjects.Add(recentProject);
                }
            }
            foreach (var project in projectUser)
            {
                recentProject = new ProjectAc();
                var projectDetails = _projectDataRepository.FirstOrDefault(x => x.Id.Equals(project.ProjectId));
                var teamLeader = await _userManager.FindByIdAsync(projectDetails.TeamLeaderId);
                var projectMapper = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
                recentProject.Name = projectDetails.Name;
                recentProject.SlackChannelName = projectDetails.SlackChannelName;
                recentProject.TeamLeader = projectMapper;
                recentProject.IsActive = projectDetails.IsActive;
                recentProjects.Add(recentProject);
            }
            _logger.LogInformation("Total recentProjects " + recentProjects.Count().ToString());
            return recentProjects;
        }
        /// <summary>
        /// Method to return list of users.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetListOfEmployee(string name)
        {
            ApplicationUser user = _userDataRepository.FirstOrDefault(x => x.UserName.Equals(name));
            var userRoles = new List<UserRoleAc>();
            var usersRole = new UserRoleAc();
            usersRole.UserName = user.UserName;
            usersRole.Role = _stringConstant.RoleTeamLeader;
            usersRole.Name = user.FirstName + " " + user.LastName;
            userRoles.Add(usersRole);
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId.Equals(user.Id));
            var projectUserList =await _projectUserDataRepository.FetchAsync(x => x.ProjectId.Equals(project.Id));

            foreach (var projectUser in projectUserList)
            {
                var usersRoles = new UserRoleAc();
                var users = _userDataRepository.FirstOrDefault(x => x.Id.Equals(projectUser.UserId));
                usersRoles.UserName = users.UserName;
                usersRoles.Name = users.FirstName + " " + users.LastName;
                usersRoles.Role = _stringConstant.RoleAdmin;
                userRoles.Add(usersRoles);
            }
            return userRoles;
        }
        /// <summary>
        /// Method to return user role
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetUserRole(string name)
        {
            ApplicationUser user = _userDataRepository.FirstOrDefault(x => x.UserName.Equals(name));
            var role = await _userManager.GetRolesAsync(user);
            var userRole = role.First();
            var userRoles = new List<UserRoleAc>();
            if (userRole.Equals(_stringConstant.RoleAdmin))
            {
                var userRoleAdmin = new UserRoleAc();
                userRoleAdmin.UserName = user.UserName;
                userRoleAdmin.Name = user.FirstName + " " + user.LastName;
                userRoleAdmin.Role = userRole;
                userRoles.Add(userRoleAdmin);
                var userList = _userDataRepository.GetAll().ToList();
                foreach (var userDetails in userList)
                {
                    var roles = await _userManager.GetRolesAsync(userDetails);
                    if (roles.Count() != 0 && roles[0] == _stringConstant.RoleEmployee)
                    {
                        var userRoleAc = new UserRoleAc();
                        userRoleAc.UserName = userDetails.UserName;
                        userRoleAc.Name = userDetails.FirstName + " " + userDetails.LastName;
                        userRoleAc.Role = userRole;
                        userRoles.Add(userRoleAc);
                    }

                }
            }
            else
            {
                var project = _projectDataRepository.FirstOrDefault(x => x.TeamLeaderId.Equals(user.Id));
                if (project.Equals(null))
                {
                    var usersRole = new UserRoleAc();
                    usersRole.UserName = user.UserName;
                    usersRole.Role = _stringConstant.RoleEmployee;
                    usersRole.Name = user.FirstName + " " + user.LastName;
                    userRoles.Add(usersRole);
                }
                else
                {
                    var usersRole = new UserRoleAc();
                    usersRole.UserName = user.UserName;
                    usersRole.Role = _stringConstant.RoleTeamLeader;
                    usersRole.Name = user.FirstName + " " + user.LastName;
                    userRoles.Add(usersRole);
                }
            }
            if (userRoles.Equals(null))
                throw new UserRoleNotFound();
            else
                return userRoles;
            
        }

        /// <summary>
        /// Method to return list of projects along with the users and teamleader in a project
        /// </summary>
        /// <returns>List of projects along with users</returns>
        public async Task<IList<ProjectAc>> GetProjectsWithUsers()
        {
            List<ProjectAc> projectAcs = new List<ProjectAc>();
            var projects = await _projectDataRepository.GetAll().ToListAsync();

            projects.ForEach(project =>
            {
                ApplicationUser teamLeader = _userDataRepository.FirstOrDefault(x => x.Id.Equals(project.TeamLeaderId));
                UserAc teamLead = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
                teamLead.Role = _stringConstant.TeamLeader;

                List<ProjectUser> projectUsers = _projectUserDataRepository.Fetch(x => x.ProjectId.Equals(project.Id)).ToList();
                ProjectAc projectObject = _mapperContext.Map<Project, ProjectAc>(project);
                projectObject.TeamLeader = teamLead;
                projectObject.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.Format);
                foreach (var projectUser in projectUsers)
                {
                    ApplicationUser user = _userDataRepository.FirstOrDefault(x => x.Id.Equals(projectUser.UserId));
                    UserAc proUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
                    proUser.Role = _stringConstant.Employee;
                    projectObject.ApplicationUsers.Add(proUser);
                }
                projectAcs.Add(projectObject);
            });
            return projectAcs;
        }

        /// <summary>
        /// Method to return project details by using projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Project details along with users</returns>
        public async Task<ProjectAc> GetProjectDetails(int projectId)
        {
            Project project = _projectDataRepository.FirstOrDefault(x => x.Id.Equals(projectId));
            ApplicationUser teamLeader = _userDataRepository.FirstOrDefault(x => x.Id.Equals(project.TeamLeaderId));
            UserAc teamLead = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
            teamLead.Role = _stringConstant.TeamLeader;
            IEnumerable<ProjectUser> projectUsers = await _projectUserDataRepository.FetchAsync(x => x.ProjectId.Equals(project.Id));
            ProjectAc projectDetails = _mapperContext.Map<Project, ProjectAc>(project);
            projectDetails.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.Format);
            projectDetails.TeamLeader = teamLead;
            List<UserAc> projectUserList = new List<UserAc>();
            foreach (var projectUser in projectUsers)
            {
                ApplicationUser user = _userDataRepository.FirstOrDefault(x => x.Id.Equals(projectUser.UserId));
                UserAc proUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
                proUser.Role = _stringConstant.Employee;
                projectDetails.ApplicationUsers.Add(proUser);
            }

            return projectDetails;
        }
    }
}
