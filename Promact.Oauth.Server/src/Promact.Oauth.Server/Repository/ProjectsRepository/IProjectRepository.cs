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
        int AddProject(Project newProject);
        void AddUserProject(ProjectUser newUserProject);
        IEnumerable<Project> GetAllProjects();
        Project GetById(int id);
        void EditProject(Project editProject);
        //void UpdateUserDetails(ProjectAc editedProject);
    }
}
