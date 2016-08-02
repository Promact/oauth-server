using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Promact.Oauth.Server.Models;

namespace Promact.Oauth.Server.Repository.ProjectsRepository
{
    public class ProjectRepository : IProjectRepository
    {
        private IDataRepository<Project> projectDataRepository;
      
        public ProjectRepository(IDataRepository<Project> _projectDataRepository)
        {
            projectDataRepository = _projectDataRepository;
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return projectDataRepository.List().ToList();
        }
        public void AddProject(Project newProject)
        {
            var projectAc = new ProjectAc
            {
                Name = newProject.Name,
            SlackChannelName = newProject.SlackChannelName,
            IsActive = newProject.IsActive
            };
            projectDataRepository.Add(newProject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Project GetById(int id)
        {
            var projects = projectDataRepository.List().ToList();
            foreach (var project in projects)
            {
                if (project.Id.Equals(id))
                {
                    var requiredProject = new Project
                    {
                        Id = project.Id,
                        Name = project.Name,
                        SlackChannelName= project.SlackChannelName,
                        IsActive= project.IsActive
                    };
                    return requiredProject;
                }
            }
            return null;
        }

        public void EditProject(Project editProject)
        {
            projectDataRepository.Update(editProject);
        }
    }
}
