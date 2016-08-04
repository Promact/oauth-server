using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Repository.ProjectsRepository
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Adds new project in the database
        /// </summary>
        /// <param name="newProject">project that need to be added</param>
        /// <returns>project id of newly created project</returns>
        int AddProject(ProjectAc newProject);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUserProject"></param>
        void AddUserProject(ProjectUser newUserProject);
        IEnumerable<ProjectAc> GetAllProjects();
        ProjectAc GetById(int id);
        void EditProject(ProjectAc editProject);
        //void UpdateUserDetails(ProjectAc editedProject);
    }
}
