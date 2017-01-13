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
using Promact.Oauth.Server.ExceptionHandler;

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
            IMapper mapperContext, IStringConstant stringConstant, ILogger<ProjectRepository> logger)
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
        /// This method getting the list of all projects
        /// </summary>
        /// <returns>list of projects</returns>
        public async Task<IEnumerable<ProjectAc>> GetAllProjectsAsync()
        {
            var projects = await _projectDataRepository.GetAll().ToListAsync();
            var projectAcList = new List<ProjectAc>();
            foreach (var project in projects)
            {
                var userAc = new UserAc();
                if (!string.IsNullOrEmpty(project.TeamLeaderId))
                {
                    var user = await _userDataRepository.FirstAsync(x => x.Id.Equals(project.TeamLeaderId));
                    userAc = _mapperContext.Map<ApplicationUser, UserAc>(user);
                }
                var projectAc = _mapperContext.Map<Project, ProjectAc>(project);
                projectAc.TeamLeader = userAc;
                projectAc.CreatedBy = (await _userDataRepository.FirstAsync(x => x.Id == project.CreatedBy)).FirstName;
                projectAc.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.DateFormate);
                projectAc.UpdatedBy = (await _userDataRepository.FirstOrDefaultAsync(x => x.Id == project.UpdatedBy))?.FirstName;
                projectAc.UpdatedDate = project.UpdatedDateTime == null ? "" : Convert.ToDateTime(project.UpdatedDateTime).ToString(_stringConstant.DateFormate);
                projectAcList.Add(projectAc);
            }
            return projectAcList;
        }

        /// <summary>
        /// This method is used to add new project
        /// </summary>
        /// <param name="newProject">project that need to be added</param>
        /// <param name="createdBy">Passed id of user who has create this project</param>
        /// <returns>project id of newly created project</returns>
        public async Task<int> AddProjectAsync(ProjectAc newProject, string createdBy)
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
        ///This method to add user id and project id in userproject table
        /// </summary>
        /// <param name="newProjectUser">projectuser object</param>
        public async Task AddUserProjectAsync(ProjectUser newProjectUser)
        {
            _projectUserDataRepository.AddAsync(newProjectUser);
            await _projectDataRepository.SaveChangesAsync();

        }

        /// <summary>
        /// This method to return the project details of the given id 
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns>Project infromation </returns>
        public async Task<ProjectAc> GetProjectByIdAsync(int id)
        {
            List<UserAc> userAcList = new List<UserAc>();
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (project != null)
            {
                var userIdList = (await _projectUserDataRepository.Fetch(y => y.ProjectId == project.Id).ToListAsync()).Select(x=>x.UserId);
                var userList = await _userDataRepository.Fetch(y => userIdList.Contains(y.Id)).ToListAsync();
                userAcList = _mapperContext.Map<List<ApplicationUser>,List<UserAc>>(userList);
                var projectAc = _mapperContext.Map<Project, ProjectAc>(project);
                if (!string.IsNullOrEmpty(project.TeamLeaderId))
                {
                    var teamLeader = await _userDataRepository.FirstOrDefaultAsync(x => x.Id.Equals(project.TeamLeaderId));
                    projectAc.TeamLeader = new UserAc { FirstName = teamLeader.FirstName, LastName = teamLeader.LastName, Email = teamLeader.Email };
                }
                else { projectAc.TeamLeader = null; }
                projectAc.ApplicationUsers = userAcList.OrderBy(y => y.FirstName).ToList();
                return projectAc;
            }
            else
            {
                throw new ProjectNotFound();
            }

        }

        /// <summary>
        /// This method to update project information 
        /// </summary>
        /// <param name="editProject">Updated project object</param> 
        /// <param name="updatedBy">Passed id of user who has update this project</param>
        /// <returns>project id</returns>
        public async Task<int> EditProjectAsync(int id, ProjectAc editProject, string updatedBy)
        {
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (project != null)
            {
                project.IsActive = editProject.IsActive;
                project.Name = editProject.Name;
                project.TeamLeaderId = editProject.TeamLeaderId;
                project.SlackChannelName = editProject.SlackChannelName;
                project.UpdatedDateTime = DateTime.UtcNow;
                project.UpdatedBy = updatedBy;
                _projectDataRepository.UpdateAsync(project);
                await _projectDataRepository.SaveChangesAsync();

                //Delete old users from project user table
                _projectUserDataRepository.Delete(x => x.ProjectId == id);
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
            else
            {
                throw new ProjectNotFound();
            }
        }

        /// <summary>
        /// this Method to check Project and SlackChannelName is already exists or not 
        /// </summary>
        /// <param name="project">projectAc object</param> 
        /// <returns>projectAc object</returns>
        public async Task<ProjectAc> CheckDuplicateProjectAsync(ProjectAc projectAc)
        {
            string projectName;
            string slackChannelName;
            if (projectAc.Id == 0) // for new project
            {
                projectName = (await _projectDataRepository.FirstOrDefaultAsync(x => x.Name == projectAc.Name))?.Name;
                slackChannelName = (await _projectDataRepository.FirstOrDefaultAsync(x => x.SlackChannelName == projectAc.SlackChannelName))?.SlackChannelName;
            }
            else // for edit project
            {
                projectName = (await _projectDataRepository.FirstOrDefaultAsync(x => x.Id != projectAc.Id && x.Name == projectAc.Name))?.Name;
                slackChannelName = (await _projectDataRepository.FirstOrDefaultAsync(x => x.Id != projectAc.Id && x.SlackChannelName == projectAc.SlackChannelName))?.SlackChannelName;

            }
            if (string.IsNullOrEmpty(projectName) && string.IsNullOrEmpty(slackChannelName))
            { return projectAc; }
            else if (!string.IsNullOrEmpty(projectName) && string.IsNullOrEmpty(slackChannelName))
            { projectAc.Name = null; return projectAc; }
            else if (string.IsNullOrEmpty(projectName) && !string.IsNullOrEmpty(slackChannelName))
            { projectAc.SlackChannelName = null; return projectAc; }
            else
            { projectAc.Name = null; projectAc.SlackChannelName = null; return projectAc; }
        }

        /// <summary>
        /// Method to return the project details of the given GroupName
        /// </summary>
        /// <param name="groupName">passed group name</param>
        /// <returns>object of Project</returns>
        public async Task<ProjectAc> GetProjectByGroupNameAsync(string groupName)
        {
            var project = await _projectDataRepository.FirstOrDefaultAsync(x => x.SlackChannelName == groupName);
            var projectAc = new ProjectAc();
            if (project != null)
            {
                projectAc = _mapperContext.Map<Project, ProjectAc>(project);
            }
            if (projectAc == null)
                throw new ProjectNotFound();
            else
                return projectAc;
        }

        /// <summary>
        /// This method to return all project for specific user
        /// </summary>
        /// <param name="userId">pass login user id</param>
        /// <returns>project information</returns>
        public async Task<IEnumerable<ProjectAc>> GetAllProjectForUserAsync(string userId)
        {
            List<ProjectAc> projectAcList = new List<ProjectAc>();
            ProjectAc projectAc = new ProjectAc();
            var projects = (await _projectDataRepository.FetchAsync(x => x.TeamLeaderId == userId)).ToList();
            if (projects != null)
            {
                foreach (var project in projects)
                {
                    var teamLeader = await _userManager.FindByIdAsync(project.TeamLeaderId);
                    var projectMapper = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
                    projectAc = _mapperContext.Map<Project, ProjectAc>(project);
                    projectAc.TeamLeader = projectMapper;
                    projectAcList.Add(projectAc);
                }
            }
            var projectUser = (await _projectUserDataRepository.FetchAsync(x => x.UserId == userId)).ToList();
            foreach (var project in projectUser)
            {
                projectAc = new ProjectAc();
                var projectDetails = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id == project.ProjectId);
                var teamLeader = await _userManager.FindByIdAsync(projectDetails.TeamLeaderId);
                var projectMapper = _mapperContext.Map<ApplicationUser, UserAc>(teamLeader);
                projectAc = _mapperContext.Map<Project, ProjectAc>(projectDetails);
                projectAc.TeamLeader = projectMapper;
                projectAcList.Add(projectAc);
            }
            return projectAcList;
        }

        /// <summary>
        /// Method to return list of projects along with the users and teamleader in a project
        /// </summary>
        /// <returns>List of projects along with users</returns>
        public async Task<IList<ProjectAc>> GetProjectsWithUsersAsync()
        {
            List<ProjectAc> projectAcList = new List<ProjectAc>();
            var projects = await _projectDataRepository.GetAll().ToListAsync();

            foreach (var project in projects)
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
        public async Task<ProjectAc> GetProjectDetailsAsync(int projectId)
        {
            Project project = await _projectDataRepository.FirstOrDefaultAsync(x => x.Id == projectId);
            if (project != null)
            {
                ApplicationUser applicationUser = await _userDataRepository.FirstOrDefaultAsync(x => x.Id == project.TeamLeaderId);
                UserAc teamLeader = _mapperContext.Map<ApplicationUser, UserAc>(applicationUser);
                teamLeader.Role = _stringConstant.TeamLeader;
                IEnumerable<ProjectUser> projectUsers = await _projectUserDataRepository.FetchAsync(x => x.ProjectId == project.Id);
                ProjectAc projectAc = _mapperContext.Map<Project, ProjectAc>(project);
                projectAc.CreatedDate = project.CreatedDateTime.ToString(_stringConstant.Format);
                projectAc.TeamLeader = teamLeader;
                List<UserAc> projectUserList = new List<UserAc>();
                foreach (var projectUser in projectUsers)
                {
                    ApplicationUser user = await _userDataRepository.FirstOrDefaultAsync(x => x.Id == projectUser.UserId);
                    UserAc userAc = _mapperContext.Map<ApplicationUser, UserAc>(user);
                    userAc.Role = _stringConstant.Employee;
                    projectAc.ApplicationUsers.Add(userAc);
                }

                return projectAc;
            }
            else
            {
                throw new ProjectNotFound();
            }
        }
    }
}
