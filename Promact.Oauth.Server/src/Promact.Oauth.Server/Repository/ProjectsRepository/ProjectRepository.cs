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
        private IDataRepository<ProjectUser> _projectUserDataRepository;
      
        public ProjectRepository(IDataRepository<Project> _projectDataRepository,IDataRepository<ProjectUser> projectUserDataRepository)
        {
            projectDataRepository = _projectDataRepository;
            _projectUserDataRepository = projectUserDataRepository;
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return projectDataRepository.List().ToList();
        }
        public int AddProject(Project newProject)
        {
            
            try
            {
                Project project = new Project();
                project.IsActive = newProject.IsActive;
                project.Name = newProject.Name;
                project.TeamLeaderId = newProject.TeamLeaderId;
                project.SlackChannelName = newProject.SlackChannelName;
                project.CreatedDateTime = DateTime.Now;

                projectDataRepository.Add(project);
                
                return project.Id;
            }
            catch (Exception ex)
            {

                throw;
            }

            //_projectUserDataRepository.Add(newProject.ApplicatioUsers);
        }
        public void AddUserProject(ProjectUser newProjectUser)
        {
            _projectUserDataRepository.Add(newProjectUser);
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
