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
        private IDataRepository<ApplicationUser> _userDataRepository;
        public ProjectRepository(IDataRepository<Project> _projectDataRepository,IDataRepository<ProjectUser> projectUserDataRepository, IDataRepository<ApplicationUser> userDataRepository)
        {
            projectDataRepository = _projectDataRepository;
            _projectUserDataRepository = projectUserDataRepository;
            _userDataRepository = userDataRepository;
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

            List<ApplicationUser> applicationUserList = new List<ApplicationUser>(); 
            var projects = projectDataRepository.FirstOrDefault(x => x.Id == id);
            List<ProjectUser> projectUserList= _projectUserDataRepository.Fetch(y => y.ProjectId == projects.Id).ToList();
            foreach (ProjectUser pu in projectUserList)
            {
                var applicationUser = _userDataRepository.FirstOrDefault(z => z.Id == pu.UserId);
                applicationUserList.Add(applicationUser);
            }
            projects.ApplicatioUsers = applicationUserList;
            return projects;
        }

        public void EditProject(Project editProject)
        {
            projectDataRepository.Update(editProject);
        }
    }
}
