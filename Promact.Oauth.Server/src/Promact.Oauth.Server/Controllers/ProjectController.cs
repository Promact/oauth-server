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
using Promact.Oauth.Server.Exception_Handler;
using Exceptionless;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Promact.Oauth.Server.Controllers
{

    //[Authorize]
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
         * @api {get} api/Project 
         * @apiVersion 1.0.0
         * @apiName Project
         * @apiGroup Project
         * @apiSuccessExample {json} Success-Response:
         * HTTP/1.1 200 OK 
         * {
         *     "description":"return list of projects"
         * }
         */
        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<ProjectAc>> Projects()
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
      * @api {get} api/Project/:id 
      * @apiVersion 1.0.0
      * @apiName Project
      * @apiGroup Project
      * @apiParam {int} id  project Id
      * @apiParamExample {json} Request-Example:
      *      
      *        {
      *             "id": "1"
      *             "description":"return the project details of the given id"
      *        }      
      * @apiSuccessExample {json} Success-Response:
      * HTTP/1.1 200 OK 
      * {
      *     "id":"1"
      *     "description":"return the project details of the given id"
      * }
      */
        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
               return Ok(await _projectRepository.GetById(id));
            }
            catch (ProjectNotFound)
            {
                return NotFound();
            }
        }

        /**
      * @api {post} api/Project 
      * @apiVersion 1.0.0
      * @apiName Project
      * @apiGroup Project
      * @apiParam {string} Name  Project Name
      * @apiParam {string} SlackChannelName  Project SlackChannelName
      * @apiParam {bool} IsActive  Project IsActive
      * @apiParam {int} TeamLeaderId  Project TeamLeaderId
      * @apiParam {UserAc} ApplicationUsers  Project ApplicationUsers
      * @apiParamExample {json} Request-Example:
      *      
      *        {
      *             "Name":"ProjectName",
      *             "SlackChannelName":"SlackChannelName",
      *             "IsActive":"True",
      *             "TeamLeaderId":"1",
      *             "ApplicationUsers":"List of Users"
      *        }      
      * @apiSuccessExample {json} Success-Response:
      * HTTP/1.1 200 OK 
      * {
      *     "description":"Add new project"
      * }
      */
        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> addProject([FromBody]ProjectAc project)
        {
            var createdBy = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                ProjectAc checkDuplicateProject = _projectRepository.checkDuplicate(project);
                if (checkDuplicateProject.Name != null && checkDuplicateProject.SlackChannelName != null)
                {
                    int id = await _projectRepository.AddProject(project, createdBy);
                    foreach (var applicationUser in project.ApplicationUsers)
                    {
                        ProjectUser projectUser = new ProjectUser();
                        projectUser.ProjectId = id;
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
        * @api {put} api/Project 
        * @apiVersion 1.0.0
        * @apiName Project
        * @apiGroup Project
        * @apiParam {int} Id  Project Id
        * @apiParam {string} Name  Project Name
        * @apiParam {string} SlackChannelName  Project SlackChannelName
        * @apiParam {bool} IsActive  Project IsActive
        * @apiParam {int} TeamLeaderId  Project TeamLeaderId
        * @apiParam {UserAc} ApplicationUsers  Project ApplicationUsers
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             "Id":"1",
        *             "Name":"ProjectName",
        *             "SlackChannelName":"SlackChannelName",
        *             "IsActive":"True",
        *             "TeamLeaderId":"1",
        *             "ApplicationUsers":"List of Users"
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Update project information"
        * }
        */
        [Authorize]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> editProject([FromBody]ProjectAc project)
        {
            var updatedBy = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                ProjectAc checkDuplicateProject = _projectRepository.checkDuplicateFromEditProject(project);
                if (checkDuplicateProject.Name != null && checkDuplicateProject.SlackChannelName != null)
                {
                    await _projectRepository.EditProject(project, updatedBy);
                }
                else { return Ok(project); }
            }
            return Ok(project);
            
        }

        /**
        * @api {get} api/Project/fetchProject 
        * @apiVersion 1.0.0
        * @apiName Project
        * @apiGroup Project
        * @apiParam {string} name project Name
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             
        *             "Name":"ProjectName"
        *            
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Method to return list of users/employees of the given group name"
        * }
        */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("fetchProject/{name}")]
        public IActionResult Fetch(string name)
        {
            try
            {
                return Ok(_projectRepository.GetProjectByGroupName(name));
            }
            catch (ProjectNotFound)
            {
                return NotFound();
            }
        }

        /**
        * @api {get} api/Project/GetUserRole 
        * @apiVersion 1.0.0
        * @apiName Project
        * @apiGroup Project
        * @apiParam {string} name UserName
        * @apiParamExample {json} Request-Example:
        *      
        *        {
                    "Name":"UserName"    
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Method to return user role"
        * }
        */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("featchUserRole/{name}")]
        public async Task<IActionResult> GetUserRole(string name)
        {
            try
            {
                
                return Ok(await _projectRepository.GetUserRole(name));
            }
            catch (UserRoleNotFound)
            {
                return NotFound();
            }

        }
        /**
        * @api {get} api/Project/GetListOfEmployee 
        * @apiVersion 1.0.0
        * @apiName Project
        * @apiGroup Project
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Method return list of users"
        * }
        */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("featchListOfUser/{name}")]
        public async Task<List<UserRoleAc>> GetListOfEmployee(string name)
        {
            return await _projectRepository.GetListOfEmployee(name);

        }

        /**
      * @api {get} api/Project/fetchProject 
      * @apiVersion 1.0.0
      * @apiName Project
      * @apiGroup Project
      * @apiParam {string} groupName as a SlackChannelName
      * @apiParamExample {json} Request-Example:
      *      
      *        {
      *             
      *               "groupName":"SlackChannelName",
      *            
      *        }      
      * @apiSuccessExample {json} Success-Response:
      * HTTP/1.1 200 OK 
      * {
      *     "description":"method return list of users"
      * }
      */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("fetchProjectUsers/{groupName}")]
        public async Task<IActionResult> FetchUsers(string groupName)
        {
            try
            {
                return Ok(await _projectRepository.GetProjectUserByGroupName(groupName));
            }
            catch (UserNotFound)
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
        [Route("allProjects")]
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
        [Route("projectDetails/{projectId}")]
        public async Task<ProjectAc> ProjectDetails(int projectId)
        {
            return await _projectRepository.GetProjectDetails(projectId);
        }
        #endregion
    }
}