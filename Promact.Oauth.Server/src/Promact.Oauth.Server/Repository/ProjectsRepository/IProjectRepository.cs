using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System.Collections.Generic;


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
        int AddProject(ProjectAc newProject,string createdBy);

        /// <summary>
        /// Adds UserId and ProjectId in UserProject table
        /// </summary>
        /// <param name="newProjectUser"></param>ProjectId and UserId information that need to be added
        void AddUserProject(ProjectUser newUserProject);

        /// <summary>
        /// Get All Projects list from the database
        /// </summary>
        /// <returns></returns>List of Projects
        IEnumerable<ProjectAc> GetAllProjects();

        /// <summary>
        /// Get the single project and list of users related project Id from the database(project and ProjectUser Table)
        /// </summary>
        /// <param name="id"></param>Project id that need to be featch the Project and list of users
        /// <returns></returns>Project and User/Users infromation 
        ProjectAc GetById(int id);

        /// <summary>
        /// Update Project information and User list information In Project table and Project User Table
        /// </summary>
        /// <param name="editProject"></param>Updated information in editProject Parmeter
        void EditProject(ProjectAc editProject,string updatedBy);
        
    }
}
