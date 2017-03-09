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
        ///This method to add user id and project id in userproject table
        /// </summary>
        /// <param name="newProjectUser">projectuser object</param>
        Task AddUserProjectAsync(ProjectUser newUserProject);

        /// <summary>
        /// This method getting the list of all projects
        /// </summary>
        /// <returns>list of projects</returns>
        Task<IEnumerable<ProjectAc>> GetAllProjectsAsync();

        /// <summary>
        /// Get the Project details by project id. 
        /// </summary>
        /// <param name="id">project id</param> 
        /// <returns></returns>Project and User/Users infromation 
        Task<ProjectAc> GetProjectByIdAsync(int id);

        /// <summary>
        /// This method to update project information 
        /// </summary>
        /// <param name="id">project id</param> 
        /// <param name="editProject">updated project object</param> 
        /// <param name="updatedBy">passed id of user who has update this project</param>
        /// <returns>project id</returns>
        Task<int> EditProjectAsync(int id,ProjectAc editProject,string updatedBy);

        /// <summary>
        /// this method to check Project is already exists or not 
        /// </summary>
        /// <param name="project">projectAc object</param> 
        /// <returns>projectAc object</returns>
        Task<ProjectAc> CheckDuplicateProjectAsync(ProjectAc project);

        /// <summary>
        /// This method to return all project for specific user
        /// </summary>
        /// <param name="userId">passed login user id</param>
        /// <returns>project information</returns>
        Task<IEnumerable<ProjectAc>> GetAllProjectForUserAsync(string userId);

        /// <summary>
        /// Method to return list of projects along with the users and teamleader in a project
        /// </summary>
        /// <returns>List of projects along with users</returns>
        Task<IList<ProjectAc>> GetProjectsWithUsersAsync();

        /// <summary>
        /// Method to return project details by using projectId - GA
        /// </summary>
        /// <param name="projectId">passed project Id</param>
        /// <returns>project details along with users</returns>
        Task<ProjectAc> GetProjectDetailsAsync(int projectId);

        /// <summary>
        /// Method to get list of project for an user
        /// </summary>
        /// <param name="userId">user's user Id</param>
        /// <returns>list of project</returns>
        Task<List<ProjectAc>> GetListOfProjectsEnrollmentOfUserByUserIdAsync(string userId);

        /// <summary>
        /// Method to get list of team member by project Id
        /// </summary>
        /// <param name="projectId">project Id</param>
        /// <returns>list of team members</returns>
        Task<List<UserAc>> GetListOfTeamMemberByProjectIdAsync(int projectId);


        /// <summary>
        /// Method to return active project details of the given projectId - JJ
        /// </summary>
        /// <param name="projectId">project Id</param>
        /// <returns>object of ProjectAc</returns>
        Task<ProjectAc> GetProjectByProjectIdAsync(int projectId);


        /// <summary>
        /// Method to return the list of project details - JJ
        /// </summary>
        /// <returns>list of object of ProjectAc</returns>
        Task<List<ProjectAc>> GetProjectListAsync();
    }
}
