using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Data;
using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Models;

using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly PromactOauthDbContext _appDbContext;
        private readonly IProjectRepository projectRepository;

        public ProjectController(PromactOauthDbContext appContext, IProjectRepository _projectRepository)
        {
            projectRepository = _projectRepository;
            _appDbContext = appContext;
        }

        // GET: api/values
        [HttpGet]
        [Route("projects")]
        public IEnumerable<Project> Get()
        {
            return projectRepository.GetAllProjects();
            //var projects = _appDbContext.Projects?.ToList();
            //var projectAcs = new List<ProjectAc>();
            //projects?.ForEach(x =>
            //{
            //    projectAcs.Add(new ProjectAc
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        SlackChannelName = x.SlackChannelName,
            //        IsActive = x.IsActive

            //    });
            //});
            //return projectAcs;
            //    new List<ProjectAc>
            //{
            //    new ProjectAc {
            //        Id=1,
            //        Name="Huddle",
            //        SlackChannelName="test",
            //        IsActive=true
            //    },
            //    new ProjectAc {
            //        Id=2,
            //        Name="Whiteboard",
            //        SlackChannelName="test1",
            //        IsActive=true

            //    }
            //};//projectAcs;
        }

        // GET api/values/5
        [HttpGet]
        [Route("getProjects/{id}")]
        public Project Get(int id)
        {
            return projectRepository.GetById(id);
            //return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("addProject")]
        public Project Post([FromBody]Project project)
        {
            //if (_appDbContext.Projects == null) _appDbContext.Projects = new DbSet<Models.Project>();
            //Today
            //_appDbContext.Projects.Add(new Models.Project
            //{
            //    Name = project.Name,
            //    SlackChannelName = project.SlackChannelName,
            //    IsActive=project.IsActive

            //});
            //_appDbContext.SaveChanges();
            //return RedirectToAction("Get");
            //End Today
            //project.CreatedBy = Microsoft.;
            int id=projectRepository.AddProject(project);
            foreach (ApplicationUser applicationUser in project.ApplicatioUsers)
            {
                ProjectUser projectUser = new ProjectUser();
                projectUser.ProjectId = id;
                projectUser.UserId = applicationUser.Id;
                projectRepository.AddUserProject(projectUser);
            }
            //ProjectRepository.AddUserProject(project,id);
            return project;
        }

        // PUT api/values/5
        [HttpPut]
        [Route("editProject")]
        public void Put(int id, [FromBody]Project project)
        {
            projectRepository.EditProject(project);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deleteProject/{id}")]
        public IActionResult Delete(int id)
        {
            _appDbContext.Projects.Remove(new Models.Project { Id=id});
            _appDbContext.SaveChanges();
            return RedirectToAction("Get");
        }
    }
}
