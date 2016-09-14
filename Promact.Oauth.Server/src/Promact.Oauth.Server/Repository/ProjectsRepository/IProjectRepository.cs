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
        Task<int> AddProject(ProjectAc newProject,string createdBy);

        /// <summary>
        /// Adds UserId and ProjectId in UserProject table
        /// </summary>
        /// <param name="newProjectUser"></param>ProjectId and UserId information that need to be added
        void AddUserProject(ProjectUser newUserProject);

        /// <summary>
        /// Get All Projects list from the database
        /// </summary>
        /// <returns></returns>List of Projects
        Task<IEnumerable<ProjectAc>> GetAllProjects();

        /// <summary>
        /// Get the single project and list of users related project Id from the database(project and ProjectUser Table)
        /// </summary>
        /// <param name="id"></param>Project id that need to be featch the Project and list of users
        /// <returns></returns>Project and User/Users infromation 
        Task<ProjectAc> GetById(int id);

        /// <summary>
        /// Update Project information and User list information In Project table and Project User Table
        /// </summary>
        /// <param name="editProject"></param>Updated information in editProject Parmeter
        Task<int> EditProject(ProjectAc editProject,string updatedBy);

        /// <summary>
        /// Check Project and SlackChannelName is already exists or not 
        /// </summary>
        /// <param name="project"></param> pass the project parameter
        /// <returns>projectAc object</returns>
        ProjectAc checkDuplicate(ProjectAc project);

        /// <summary>
        /// Check Project and SlackChannelName is already exists or not 
        /// </summary>
        /// <param name="project"></param> pass the project parameter
        /// <returns>projectAc object</returns>
        ProjectAc checkDuplicateFromEditProject(ProjectAc project);

        /// <summary>
        /// Fetches the project details of the given GroupName
        /// </summary>
        /// <param name="GroupName"></param>
        /// <returns>object of ProjectAc</returns>
        ProjectAc GetProjectByGroupName(string GroupName);

        /// <summary>
        /// This method is used to fetch list of users/employees of the given group name. - JJ
        /// </summary>
        /// <param name="GroupName"></param>
            /// <returns>object of UserAc</returns>
        List<UserAc> GetProjectUserByGroupName(string GroupName);


        /// <summary>
        /// Method to get list of project in which current user is envolved
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProjectAc>> GetAllProjectForUser(string userId);

        Task<List<UserRoleAc>> GetUserRole(string name);

        /// <summary>
        /// Method to return list of projects along with the users and teamleader in a project
        /// </summary>
        /// <returns>List of projects along with users</returns>
        Task<IList<ProjectAc>> GetProjectsWithUsers();

        /// <summary>
        /// Method to return project details by using projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Project details along with users</returns>
        Task<ProjectAc> GetProjectDetails(int projectId);

        Task<List<UserRoleAc>> GetUserRole(string name);

    }
}
