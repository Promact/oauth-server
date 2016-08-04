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
        private IDataRepository<Project> _projectDataRepository;
        private IDataRepository<ProjectUser> _projectUserDataRepository;
        private IDataRepository<ApplicationUser> _userDataRepository;
        public ProjectRepository(IDataRepository<Project> projectDataRepository, IDataRepository<ProjectUser> projectUserDataRepository, IDataRepository<ApplicationUser> userDataRepository)
        {
            _projectDataRepository = projectDataRepository;
            _projectUserDataRepository = projectUserDataRepository;
            _userDataRepository = userDataRepository;
        }

        public IEnumerable<ProjectAc> GetAllProjects()
        {
            var projects = _projectDataRepository.List().ToList();
            var projectAcs = new List<ProjectAc>();
            projects.ForEach(project =>
            {
                projectAcs.Add(new ProjectAc
                {
                    Id = project.Id,
                    Name = project.Name,
                    IsActive = project.IsActive,
                    SlackChannelName = project.SlackChannelName,
                    TeamLeaderId = project.TeamLeaderId
                });
            });
            return projectAcs;
        }

        /// <summary>
        /// Adds new project in the database
        /// </summary>
        /// <param name="newProject">project that need to be added</param>
        /// <returns>project id of newly created project</returns>
        public int AddProject(ProjectAc newProject)
        {

            try
            {
                Project project = new Project();
                project.IsActive = newProject.IsActive;
                project.Name = newProject.Name;
                project.TeamLeaderId = newProject.TeamLeaderId;
                project.SlackChannelName = newProject.SlackChannelName;
                project.CreatedDateTime = DateTime.Now;
                _projectDataRepository.Add(project);
                return project.Id;
            }
            catch (Exception ex)
            {

                throw;
            }
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
        public ProjectAc GetById(int id)
        {

            List<UserAc> applicationUserList = new List<UserAc>();
            var project = _projectDataRepository.FirstOrDefault(x => x.Id == id);
            List<ProjectUser> projectUserList = _projectUserDataRepository.Fetch(y => y.ProjectId == project.Id).ToList();
            foreach (ProjectUser pu in projectUserList)
            {
                var applicationUser = _userDataRepository.FirstOrDefault(z => z.Id == pu.UserId);
                applicationUserList.Add(new UserAc
                {
                    Id = applicationUser.Id,
                    FirstName = applicationUser.FirstName
                });
            }
            var projectAc = new ProjectAc();
            projectAc.Id = project.Id;
            projectAc.SlackChannelName = project.SlackChannelName;
            projectAc.IsActive = project.IsActive;
            projectAc.Name = project.Name;
            projectAc.TeamLeader = new UserAc();
            projectAc.TeamLeaderId = project.TeamLeaderId;
            projectAc.TeamLeader.FirstName = _userDataRepository.FirstOrDefault(x => x.Id == project.TeamLeaderId)?.FirstName;
            projectAc.ApplicatioUsers = applicationUserList;
            return projectAc;
        }

        public void EditProject(ProjectAc editProject)
        {
            var projectId = editProject.Id;
            var projectInDb = _projectDataRepository.FirstOrDefault(x => x.Id == projectId);
            projectInDb.IsActive = editProject.IsActive;
            projectInDb.Name = editProject.Name;
            projectInDb.TeamLeaderId = editProject.TeamLeaderId;
            projectInDb.SlackChannelName = editProject.SlackChannelName;
            projectInDb.UpdatedDateTime = DateTime.UtcNow;
            _projectDataRepository.Update(projectInDb);

            //Delete old users from project user table
            _projectUserDataRepository.Delete(x => x.ProjectId == projectId);
            _projectUserDataRepository.Save();

            editProject.ApplicatioUsers.ForEach(x =>
            {
                _projectUserDataRepository.Add(new ProjectUser
                {
                    ProjectId = projectInDb.Id,
                    UpdatedDateTime = DateTime.UtcNow,
                    UserId=x.Id
                });
            });
        }
    }
}
