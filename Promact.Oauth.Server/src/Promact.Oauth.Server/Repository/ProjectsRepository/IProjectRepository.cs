using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.ProjectsRepository
{
    public interface IProjectRepository
    {
       
        /// <summary>
        /// Adds new project in the database
        /// </summary>
        /// <param name="newProject">project that need to be added</param>
        /// <param name="createdBy">Login User Id</param>
        /// <returns>project id of newly created project</returns>
        Task<int> AddProjectAsync(ProjectAc newProject,string createdBy);

        /// <summary>
        /// Adds UserId and ProjectId in UserProject table
        /// </summary>
        /// <param name="newProjectUser"></param>ProjectId and UserId information that need to be added
        Task AddUserProjectAsync(ProjectUser newUserProject);

        /// <summary>
        /// Getting the list of all projects
        /// </summary>
        /// <returns></returns>List of Projects
        Task<IEnumerable<ProjectAc>> GetAllProjectsAsync();

        /// <summary>
        /// Get the Project details by project id. 
        /// </summary>
        /// <param name="id"></param>Project id that need to be featch the Project and list of users
        /// <returns></returns>Project and User/Users infromation 
        Task<ProjectAc> GetProjectByIdAsync(int id);

        /// <summary>
        /// Should be update Project ,Team leader and Team member information. 
        /// </summary>
        /// <param name="editProject"></param>Updated information in editProject Parmeter
        Task<int> EditProjectAsync(int id,ProjectAc editProject,string updatedBy);

        /// <summary>
        /// Check Project and SlackChannelName is already exists or not 
        /// </summary>
        /// <param name="project"></param> pass the project parameter
        /// <returns>projectAc object</returns>
        Task<ProjectAc> CheckDuplicateProjectAsync(ProjectAc project);

        /// <summary>
        /// Fetches the project details of the given GroupName
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns>object of ProjectAc</returns>
        Task<ProjectAc> GetProjectByGroupNameAsync(string groupName);
        
        /// <summary>
        /// Method to get list of project in which current user is envolved
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProjectAc>> GetAllProjectForUserAsync(string userId);

        /// <summary>
        /// Method to return list of projects along with the users and teamleader in a project
        /// </summary>
        /// <returns>List of projects along with users</returns>
        Task<IList<ProjectAc>> GetProjectsWithUsersAsync();

        /// <summary>
        /// Method to return project details by using projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Project details along with users</returns>
        Task<ProjectAc> GetProjectDetailsAsync(int projectId);

    }
}
