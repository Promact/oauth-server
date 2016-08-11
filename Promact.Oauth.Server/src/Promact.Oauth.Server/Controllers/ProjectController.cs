using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using Promact.Oauth.Server.Repository;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Promact.Oauth.Server.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly PromactOauthDbContext _appDbContext;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        public ProjectController(PromactOauthDbContext appContext, IProjectRepository projectRepository, UserManager<ApplicationUser> userManager ,IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _appDbContext = appContext;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        // GET: api/values
        [HttpGet]
        [Route("projects")]
        public IEnumerable<ProjectAc> Get()
        {
            return _projectRepository.GetAllProjects();
        }

        

        // GET api/values/5
        [HttpGet]
        [Route("getProjects/{id}")]
        public ProjectAc Get(int id)
        {
            return _projectRepository.GetById(id);
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
                ProjectAc p =_projectRepository.checkDuplicate(project);
                if (p.Name != null && p.SlackChannelName != null)
                {
                    int id = _projectRepository.AddProject(project, createdBy);
                    foreach (var applicationUser in project.ApplicationUsers)
                    {
                        ProjectUser projectUser = new ProjectUser();
                        projectUser.ProjectId = id;
                        projectUser.UserId = applicationUser.Id;
                        projectUser.CreatedBy = createdBy;
                        projectUser.CreatedDateTime = DateTime.UtcNow;
                        _projectRepository.AddUserProject(projectUser);
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
            //project.TeamLeader = _userRepository.GetById(project.TeamLeaderId);
            var updatedBy = _userManager.GetUserId(User);
            try
            {
                if (ModelState.IsValid)
                {
                    ProjectAc p = _projectRepository.checkDuplicateFromEditProject(project);
                    if (p.Name != null && p.SlackChannelName != null)
                    {
                        _projectRepository.EditProject(project, updatedBy);
                    }
                    else { return Ok(project); }
                }
            }
            catch(Exception ex)
            { }
            
           // else { return BadRequest(); }
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