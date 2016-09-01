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
using System.Threading.Tasks;
using Exceptionless;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Promact.Oauth.Server.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly PromactOauthDbContext _appDbContext;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        public ProjectController(PromactOauthDbContext appContext, IProjectRepository projectRepository, UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _appDbContext = appContext;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        // GET: api/values
        [HttpGet]
        [Route("projects")]
        public async Task<IEnumerable<ProjectAc>> projects()
        {
            try
            {
                return await _projectRepository.GetAllProjects();
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }



        // GET api/values/5
        [HttpGet]
        [Route("getProjects/{id}")]
        public async Task<ProjectAc> getProjects(int id)
        {
            try
            {
                return await _projectRepository.GetById(id);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        // POST api/values
        [HttpPost]
        [Route("addProject")]
        public async Task<IActionResult> addProject([FromBody]ProjectAc project)
        {
            try
            {
                var createdBy = _userManager.GetUserId(User);
                if (ModelState.IsValid)
                {
                    ProjectAc p = _projectRepository.checkDuplicate(project);
                    if (p.Name != null && p.SlackChannelName != null)
                    {
                        int id = await _projectRepository.AddProject(project, createdBy);
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
                    { return Ok(project); }
                }
                return Ok(false);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        // PUT api/values/5
        [HttpPut]
        [Route("editProject")]
        public async Task<IActionResult> editProject(int id, [FromBody]ProjectAc project)
        {
            try
            {
                var updatedBy = _userManager.GetUserId(User);

                if (ModelState.IsValid)
                {
                    ProjectAc p = _projectRepository.checkDuplicateFromEditProject(project);
                    if (p.Name != null && p.SlackChannelName != null)
                    {
                        await _projectRepository.EditProject(project, updatedBy);
                    }
                    else { return Ok(project); }
                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        // GET api/values/name
        [HttpGet]
        [Route("fetchProject/{name}")]
        public ProjectAc Fetch(string name)
        {
            try
            {
                return _projectRepository.GetProjectByGroupName(name);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        // GET api/values/name
        [HttpGet]
        [Route("fetchProjectUsers/{name}")]
        public List<UserAc> FetchUsers(string groupName)
        {
            try
            {
                return _projectRepository.GetProjectUserByGroupName(groupName);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }


        [HttpGet]
        [Route("projectUsersById/{teamLeaderId}")]
        public List<UserAc> GetProjectUsersByTeamLeaderId(string teamLeaderId)
        {
            List<UserAc> projectUsers = _projectRepository.GetProjectUsersByTeamLeaderId(teamLeaderId);
            return projectUsers;
        }


        [HttpGet]
        [Route("projectUsersById/{teamLeaderId}")]
        public List<UserAc> GetProjectUsersByTeamLeaderId(string teamLeaderId)
        {
            List<UserAc> projectUsers = _projectRepository.GetProjectUsersByTeamLeaderId(teamLeaderId);
            return projectUsers;
        }
    }
}