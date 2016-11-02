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
using System.IO;
using Exceptionless.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Promact.Oauth.Server.Repository.ProjectsRepository
{
    public class ProjectRepository : IProjectRepository
    {
        #region "Private Variable(s)"
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDataRepository<Project> _projectDataRepository;
        private readonly IDataRepository<ProjectUser> _projectUserDataRepository;
        private readonly IDataRepository<ApplicationUser> _userDataRepository;

        private readonly UserManager<ApplicationUser> _userManager;
        private  IStringConstant _stringConstant;
        private readonly IMapper _mapperContext;
        private readonly dynamic array;
        //private Constant con=new Constant();
        //private readonly StringConstants _appSettings;
        private readonly ILogger<ProjectRepository> _logger;
        #endregion

        #region "Constructor"
        public ProjectRepository(IDataRepository<Project> projectDataRepository, IDataRepository<ProjectUser> projectUserDataRepository, IDataRepository<ApplicationUser> userDataRepository, UserManager<ApplicationUser> userManager, 
            IMapper mapperContext,IStringConstant stringConstant, IHostingEnvironment hostingEnvironment, ILogger<ProjectRepository> logger)//, IOptions<StringConstants> options)
        {
            _projectDataRepository = projectDataRepository;
            _projectUserDataRepository = projectUserDataRepository;
            _userDataRepository = userDataRepository;
            _mapperContext = mapperContext;
            _userManager = userManager;
            _stringConstant = stringConstant;
            _hostingEnvironment = hostingEnvironment;
            
            string path = _hostingEnvironment.ContentRootPath + "\\Constants\\StringConstant.json";
            using (StreamReader r = File.OpenText(path))
            {
                string json = r.ReadToEnd();
                array = JsonConvert.DeserializeObject(json);
                foreach (var item in array)
                {
                    _stringConstant.TeamLeaderNotAssign= item.TeamLeaderNotAssign;
                    _stringConstant.DateFormate = item.DateFormate;
                }

            }
        }
            //_appSettings = options.Value;
            _logger = logger;
        }
        #endregion

        /// <summary>
        /// Get All Projects list from the database
        /// </summary>
        /// <returns></returns>List of Projects
        public async Task<IEnumerable<ProjectAc>> GetAllProjects()
        {
            var projects = await _projectDataRepository.GetAll().ToListAsync();
            var projectAcs = new List<ProjectAc>();

            projects.ForEach(project =>
            {
                var teamLeader = new UserAc();
                if (project.TeamLeaderId != null)
                {
                    var teamLeaders = _userDataRepository.FirstOrDefault(x => x.Id == project.TeamLeaderId);
                    teamLeader.FirstName = teamLeaders.FirstName;
                    teamLeader.LastName = teamLeaders.LastName;
                    teamLeader.Email = teamLeaders.Email;
                }
                else
                {
                    teamLeader.FirstName = _stringConstant.TeamLeaderNotAssign;
                    teamLeader.LastName = _stringConstant.LastName;
                    teamLeader.Email = _stringConstant.Email;
                }
                var CreatedBy = _userDataRepository.FirstOrDefault(x => x.Id == project.CreatedBy)?.FirstName;
                var UpdatedBy = _userDataRepository.FirstOrDefault(x => x.Id == project.UpdatedBy)?.FirstName;
                string UpdatedDate;
                if (project.UpdatedDateTime == null)
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
        /// Adds new project in the database
        /// </summary>
        /// <param name="newProject">project that need to be added</param>
        /// <param name="createdBy">Login User Id</param>
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
        /// Adds UserId and ProjectId in UserProject table
        /// </summary>
        /// <param name="newProjectUser"></param>ProjectId and UserId information that need to be added
        public void AddUserProject(ProjectUser newProjectUser)
        {
            _projectUserDataRepository.Add(newProjectUser);
        }

        /// <summary>
        /// Get the single project and list of users related project Id from the database(project and ProjectUser Table)
        /// </summary>
        /// <param name="id"></param>Project id that need to be featch the Project and list of users
        /// <returns></returns>Project and User/Users infromation 
        /// 
        public async Task<ProjectAc> GetById(int id)
        {
            List<UserAc> applicationUserList = new List<UserAc>();
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id == id);
            List<ProjectUser> projectUserList = _projectUserDataRepository.Fetch(y => y.ProjectId == project.Id).ToList();
            foreach (ProjectUser projectUsers in projectUserList)
            {
                var applicationUser = _userDataRepository.FirstOrDefault(z => z.Id == projectUsers.UserId);
                applicationUserList.Add(new UserAc
                {
                    Id = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    Email = applicationUser.Email,
                    LastName = applicationUser.LastName
                });
            }

            var projectObject = _mapperContext.Map<Project, ProjectAc>(project);
            if (project.TeamLeaderId != null)
            {var teamLeader = _userDataRepository.FirstOrDefault(x => x.Id == project.TeamLeaderId);
             projectObject.TeamLeader = new UserAc { FirstName = teamLeader.FirstName, LastName = teamLeader.LastName, Email = teamLeader.Email };}
            else{projectObject.TeamLeader = null;}
            projectObject.ApplicationUsers = applicationUserList.OrderBy(y => y.FirstName).ToList();
            return projectObject;
        }

        /// <summary>
        /// Update Project information and User list information In Project table and Project User Table
        /// </summary>
        /// <param name="editProject"></param>Updated information in editProject Parmeter
        /// <param name="updatedBy"></param>Login User Id
        public async Task<int> EditProject(ProjectAc editProject, string updatedBy)
        {
            var projectId = editProject.Id;
            var projectInDb = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id == projectId);
            projectInDb.IsActive = editProject.IsActive;
            projectInDb.Name = editProject.Name;
            projectInDb.TeamLeaderId = editProject.TeamLeaderId;
            projectInDb.SlackChannelName = editProject.SlackChannelName;
            projectInDb.UpdatedDateTime = DateTime.UtcNow;
            projectInDb.UpdatedBy = updatedBy;
            _projectDataRepository.UpdateAsync(projectInDb);
            await _projectDataRepository.SaveChangesAsync();

            //Delete old users from project user table
            _projectUserDataRepository.Delete(x => x.ProjectId == projectId);
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
        /// Check Project and SlackChannelName is already exists or not 
        /// </summary>
        /// <param name="project"></param> pass the project parameter
        /// <returns>projectAc object</returns>
        public ProjectAc checkDuplicateFromEditProject(ProjectAc project)
        {
            var projectName = _projectDataRepository.FirstOrDefault(x => x.Id != project.Id && x.Name == project.Name);
            var sName = _projectDataRepository.FirstOrDefault(x => x.Id != project.Id && x.SlackChannelName == project.SlackChannelName);
            if (projectName == null && sName == null)
            { return project; }
            else if (projectName != null && sName == null)
            { project.Name = null; return project; }
            else if (projectName == null && sName != null)
            { project.SlackChannelName = null; return project; }
            else
            { project.Name = null; project.SlackChannelName = null; return project; }

        }

        /// <summary>
        /// Check Project and SlackChannelName is already exists or not 
        /// </summary>
        /// <param name="project"></param> pass the project parameter
        /// <returns>projectAc object</returns>
        public ProjectAc checkDuplicate(ProjectAc project)
        {
            var projectName = _projectDataRepository.FirstOrDefault(x => x.Name == project.Name);
            var sName = _projectDataRepository.FirstOrDefault(x => x.SlackChannelName == project.SlackChannelName);
            if (projectName == null && sName == null)
            { return project; }
            else if (projectName != null && sName == null)
            { project.Name = null; return project; }
            else if (projectName == null && sName != null)
            { project.SlackChannelName = null; return project; }
            else
            { project.Name = null; project.SlackChannelName = null; return project; }
        }

        /// <summary>
        /// Fetches the project details of the given GroupName
        /// </summary>
        /// <param name="GroupName"></param>
        /// <returns>object of Project</returns>
        public ProjectAc GetProjectByGroupName(string GroupName)
        {
            try
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
                return projectAc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// This method is used to fetch list of users/employees of the given group name. - JJ
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="UserName"></param>
        /// <returns>list of object of UserAc</returns>
        public List<UserAc> GetProjectUserByGroupName(string GroupName)
        {
            try
            {
                var project = _projectDataRepository.FirstOrDefault(x => x.SlackChannelName.Equals(GroupName));
                var userProjects = new List<UserAc>();
                if (project != null)
                {
                    var projectUserList = _projectUserDataRepository.Fetch(x => x.ProjectId == project.Id).ToList();
                    foreach (var projectUser in projectUserList)
                    {
                        var user = _userDataRepository.FirstOrDefault(x => x.Id == projectUser.UserId);
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
                return userProjects;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


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
                var projectDetails = _projectDataRepository.FirstOrDefault(x => x.Id == project.ProjectId);
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
        /// This Method get the list of Users.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetListOfEmployee(string name)
        {
            ApplicationUser user = _userDataRepository.FirstOrDefault(x => x.UserName == name);
            var userRoles = new List<UserRoleAc>();
            var usersRole = new UserRoleAc();
            usersRole.UserName = user.UserName;

            usersRole.Role = _stringConstant.RoleTeamLeader;
            usersRole.Name = user.FirstName + " " + user.LastName;
            userRoles.Add(usersRole);
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.TeamLeaderId.Equals(user.Id));
            var projectUserList = _projectUserDataRepository.Fetch(x => x.ProjectId == project.Id).ToList();

            foreach (var projectUser in projectUserList)
            {
                var usersRoles = new UserRoleAc();
                var users = _userDataRepository.FirstOrDefault(x => x.Id == projectUser.UserId);
                usersRoles.UserName = users.UserName;
                usersRoles.Name = users.FirstName + " " + users.LastName;
                usersRoles.Role = _stringConstant.RoleAdmin;
                userRoles.Add(usersRoles);
            }
            return userRoles;
        }
        /// <summary>
        /// This Method use to featch user role
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<UserRoleAc>> GetUserRole(string name)
        {
            ApplicationUser user = _userDataRepository.FirstOrDefault(x => x.UserName == name);
            var role = await _userManager.GetRolesAsync(user);
            var userRole = role.First();
            var userRoles = new List<UserRoleAc>();
            //userRole = _stringConstant.RoleTeamLeader;
            if (userRole == _stringConstant.RoleAdmin)
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
                //project = null;
                if (project == null)
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
                ApplicationUser teamLeader = _userDataRepository.FirstOrDefault(x => x.Id == project.TeamLeaderId);
                UserAc teamLead = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
                teamLead.Role = _stringConstant.TeamLeader;

                List<ProjectUser> projectUsers = _projectUserDataRepository.Fetch(x => x.ProjectId == project.Id).ToList();
                ProjectAc projectObject = _mapperContext.Map<Project, ProjectAc>(project);
                projectObject.TeamLeader = teamLead;
                projectObject.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.Format);
                foreach (var projectUser in projectUsers)
                {
                    ApplicationUser user = _userDataRepository.FirstOrDefault(x => x.Id == projectUser.UserId);
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
            Project project = _projectDataRepository.FirstOrDefault(x => x.Id == projectId);
            ApplicationUser teamLeader = _userDataRepository.FirstOrDefault(x => x.Id == project.TeamLeaderId);
            UserAc teamLead = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
            teamLead.Role = _stringConstant.TeamLeader;
            List<ProjectUser> projectUsers = await _projectUserDataRepository.Fetch(x => x.ProjectId == project.Id).ToListAsync();
            ProjectAc projectDetails = _mapperContext.Map<Project, ProjectAc>(project);
            projectDetails.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.Format);
            projectDetails.TeamLeader = teamLead;
            List<UserAc> projectUserList = new List<UserAc>();
            foreach (var projectUser in projectUsers)
            {
                ApplicationUser user = _userDataRepository.FirstOrDefault(x => x.Id == projectUser.UserId);
                UserAc proUser = _mapperContext.Map<ApplicationUser, UserAc>(user);
                proUser.Role = _stringConstant.Employee;
                projectDetails.ApplicationUsers.Add(proUser);
            }

            return projectDetails;
        }
    }
}
