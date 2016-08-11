using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Promact.Oauth.Server.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly PromactOauthDbContext _appDbContext;
        private readonly IProjectRepository projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProjectController(PromactOauthDbContext appContext, IProjectRepository _projectRepository, UserManager<ApplicationUser> userManager)
        {
            projectRepository = _projectRepository;
            _appDbContext = appContext;
            _userManager = userManager;
        }

        // GET: api/values
        [HttpGet]
        [Route("projects")]
        public IEnumerable<ProjectAc> Get()
        {
            return projectRepository.GetAllProjects();
        }

        

        // GET api/values/5
        [HttpGet]
        [Route("getProjects/{id}")]
        public ProjectAc Get(int id)
        {
            return projectRepository.GetById(id);
            //return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("addProject")]
        public IActionResult Post([FromBody]ProjectAc project)
        {
            var createdBy = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                ProjectAc p =projectRepository.checkDuplicate(project);
                if (p.Name != null && p.SlackChannelName != null)
                {
                    int id = projectRepository.AddProject(project, createdBy);
                    foreach (var applicationUser in project.ApplicationUsers)
                    {
                        ProjectUser projectUser = new ProjectUser();
                        projectUser.ProjectId = id;
                        projectUser.UserId = applicationUser.Id;
                        projectUser.CreatedBy = createdBy;
                        projectUser.CreatedDateTime = DateTime.UtcNow;
                        projectRepository.AddUserProject(projectUser);
                    }
                    return Ok(project);
                }
                else
                    //project = null;
                { return Ok(project); }
            }
            return Ok(false);
            
        }

        // PUT api/values/5
        [HttpPut]
        [Route("editProject")]
        public IActionResult Put(int id, [FromBody]ProjectAc project)
        {
            var updatedBy = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                ProjectAc p = projectRepository.checkDuplicateFromEditProject(project);
                if (p.Name != null && p.SlackChannelName != null)
                {
                    projectRepository.EditProject(project, updatedBy);
                }
                else { return Ok(project); }
            }
            else { return BadRequest(); }
            return Ok(project);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deleteProject/{id}")]
        public IActionResult Delete(int id)
        {
            _appDbContext.Projects.Remove(new Project { Id=id});
            _appDbContext.SaveChanges();
            return Ok(new { }); 
        }
    }
}