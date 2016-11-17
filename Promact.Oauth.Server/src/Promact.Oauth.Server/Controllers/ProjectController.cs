using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Models;
using Microsoft.AspNetCore.Identity;
using System;
using Promact.Oauth.Server.Repository;
using System.Threading.Tasks;
using Promact.Oauth.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Promact.Oauth.Server.Exception_Handler;
using Microsoft.Extensions.Logging;




// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        #region "Private Variable(s)"
        private readonly PromactOauthDbContext _appDbContext;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ProjectController> _logger;
        #endregion

        #region "Constructor"
        public ProjectController(PromactOauthDbContext appContext, IProjectRepository projectRepository,
            UserManager<ApplicationUser> userManager, IUserRepository userRepository, ILogger<ProjectController> logger)
        {
            _projectRepository = projectRepository;
            _appDbContext = appContext;
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = logger;
        }
        #endregion

        #region public Methods

        /**
         * @api {get} api/project 
         * @apiVersion 1.0.0
         * @apiName GetProjects
         * @apiGroup Project
         * @apiParam {null} no parameter
         * @apiSuccessExample {json} Success-Response:
         * HTTP/1.1 200 OK 
         * {
         *     "description":"return list of projects."
         * }
         */
        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<ProjectAc>> GetProjects()
        {
            
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userRole = await _userManager.IsInRoleAsync(user, "Employee");
                _logger.LogInformation("UserRole Employee  "+userRole);
                if (userRole)
                {
                    _logger.LogInformation("call project repository for User");
                    return await _projectRepository.GetAllProjectForUser(user.Id);
                }
                else
                {
                    _logger.LogInformation("call project repository for projects");
                    return await _projectRepository.GetAllProjects();
                }


        }

        /**
        * @api {get} api/project/:id Request Project information
        * @apiVersion 1.0.0
        * @apiName GetProjectById
        * @apiGroup Project
        * @apiParam {int} id  project Id
        * @apiParamExample {json} Request-Example:
        * {
        *   "id": "1"
        * }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"return the project details of the given id"
        * }
        * @apiError ProjectNotFound The id of the Project was not found.
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 404 Not Found
        * {
        *   "error": "ProjectNotFound"
        * }
        */

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
               return Ok(await _projectRepository.GetProjectById(id));
            }
            catch (ProjectNotFound)
            {
                return NotFound();
            }
        }

        /**
      * @api {post} api/project 
      * @apiVersion 1.0.0
      * @apiName AddProject
      * @apiGroup Project
      * @apiParam {object} ProjectAc object 
      * @apiParamExample {json} Request-Example:
      * {
      *   "Name":"ProjectName",
      *   "SlackChannelName":"SlackChannelName",
      *   "IsActive":"True",
      *   "TeamLeaderId":"1",
      *   "ApplicationUsers":"List of Users"
      * }      
      * @apiSuccessExample {json} Success-Response:
      * HTTP/1.1 200 OK 
      * {
      *     "description":"Add new project"
      * }
      */
        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProject([FromBody]ProjectAc project)
        {
            var createdBy = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                ProjectAc projectAc =await _projectRepository.CheckDuplicateProject(project);
                
                if (!string.IsNullOrEmpty(projectAc.Name) && !string.IsNullOrEmpty(projectAc.SlackChannelName))
                {
                    int projectId = await _projectRepository.AddProject(project, createdBy);
                    foreach (var applicationUser in project.ApplicationUsers)
                    {
                        ProjectUser projectUser = new ProjectUser();
                        projectUser.ProjectId = projectId;
                        projectUser.UserId = applicationUser.Id;
                        projectUser.CreatedBy = createdBy;
                        projectUser.CreatedDateTime = DateTime.UtcNow;
                        await _projectRepository.AddUserProject(projectUser);
                    }
                    return Ok(project);
                }
                else
                { return Ok(project); }
            }
            return Ok(false);
        }

        /**
        * @api {put} api/project 
        * @apiVersion 1.0.0
        * @apiName EditProject
        * @apiGroup Project
        * @apiParam {object} ProjectAc object 
        * @apiParamExample {json} Request-Example:
        * {
        *   "Id":"1",
        *   "Name":"ProjectName",
        *   "SlackChannelName":"SlackChannelName",
        *   "IsActive":"True",
        *   "TeamLeaderId":"1",
        *   "ApplicationUsers":"List of Users"
        * }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Update project information"
        * }
        */
        [Authorize]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> EditProject([FromBody]ProjectAc project)
        {
            var updatedBy = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                ProjectAc projectAc = await _projectRepository.CheckDuplicateProject(project);
                if (!string.IsNullOrEmpty(projectAc.Name) && !string.IsNullOrEmpty(projectAc.SlackChannelName))
                {
                    await _projectRepository.EditProject(project, updatedBy);
                }
                else { return Ok(project); }
            }
            return Ok(project);

        }

        /**
        * @api {get} api/project/slackChannel/:name 
        * @apiVersion 1.0.0
        * @apiName Project
        * @apiGroup Project
        * @apiParam {string} name project Name
        * @apiParamExample {json} Request-Example:
        * {
        *   "Name":"ProjectName"
        * }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Method to return project details of the given group name"
        * }
        */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("slackChannel/{name}")]
        public async Task<IActionResult> GetProjectByGroupName(string name)
        {
            try
            {
                return Ok(await _projectRepository.GetProjectByGroupName(name));
            }
            catch (ProjectNotFound)
            {
                return NotFound();
            }
        }
        
        /**
      * @api {get} api/Project/allProjects 
      * @apiVersion 1.0.0
      * @apiName Project
      * @apiGroup Project  
      * @apiSuccessExample {json} Success-Response:
      * HTTP/1.1 200 OK 
      * {
      *     "description":"Method return list of Project"
      * }
      */
        [HttpGet]
        [Route("list")]
        public async Task<IEnumerable<ProjectAc>> AllProjects()
        {
            return await _projectRepository.GetProjectsWithUsers();
        }


        /**
      * @api {get} api/Project/projectDetails/:projectId 
      * @apiVersion 1.0.0
      * @apiName Project
      * @apiGroup Project
      * @apiParam {int} id  projectId
      * @apiParamExample {json} Request-Example:
      *      
      *        {
      *             "id": "1"
      *        }      
      * @apiSuccessExample {json} Success-Response:
      * HTTP/1.1 200 OK 
      * {
      *     "description":"return list of projects"
      * }
      */
        [HttpGet]
        [Route("{projectId}/detail")]
        public async Task<ProjectAc> ProjectDetails(int projectId)
        {
            return await _projectRepository.GetProjectDetails(projectId);
        }
        #endregion
    }
}