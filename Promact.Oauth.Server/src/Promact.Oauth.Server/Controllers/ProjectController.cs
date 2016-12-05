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
using Promact.Oauth.Server.ExceptionHandler;
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
         * @apiName GetProjectsAsync
         * @apiGroup Project
         * @apiParam {null} no parameter
         * @apiSuccessExample {json} Success-Response:
         * HTTP/1.1 200 OK 
         * [
          * {
          *   "Name":"Slack",
          *   "SlackChannelName":"SlackChannelName",
          *   "IsActive":"True",
          *   "TeamLeaderId":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
          *   "CreatedBy" : "Smith",
          *   "CreatedDate" : "10/02/2016",
          *   "UpdatedBy": "Smith",
          *   "UpdatedDate" : "10/02/2016"
          *   "TeamLeader": 
          *   {
          *         "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
          *         "FirstName" : "John",
          *         "Email" : "jone@promactinfo.com",
          *         "LastName" : "Doe",
          *         "SlackUserName" :"John",
          *         "IsActive" : "True",
          *         "JoiningDate" :"10-02-2016",
          *         "NumberOfCasualLeave":0,
          *         "NumberOfSickLeave":0,
          *         "UniqueName":null,
          *         "Role":null,
          *         "UserName": null
          *         
          *     } 
          * }  
          *]
          *    
         */
        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProjectsAsync()
        {
           
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var isRoleExists = await _userManager.IsInRoleAsync(user, "Employee");
                _logger.LogInformation("UserRole Employee  " + isRoleExists);
                if (isRoleExists)
                {
                    _logger.LogInformation("call project repository for User");
                    return Ok(await _projectRepository.GetAllProjectForUserAsync(user.Id));
                }
                else
                {
                    _logger.LogInformation("call project repository for projects");
                    return Ok(await _projectRepository.GetAllProjectsAsync());
                }
            }
            


        

        /**
        * @api {get} api/project/:id Request Project information
        * @apiVersion 1.0.0
        * @apiName GetProjectByIdAsync
        * @apiGroup Project
        * @apiParam {int} id  project Id
        * @apiParamExample {json} Request-Example:
        * {
        *   "id": 1
        * }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *   "Name":"Slack",
        *   "SlackChannelName":"SlackChannelName",
        *   "IsActive":"True",
        *   "TeamLeaderId":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *   "CreatedBy" : "Smith",
        *   "CreatedDate" : "10/02/2016",
        *   "UpdatedBy": "Smith",
        *   "UpdatedDate" : "10/02/2016"
        *   "TeamLeader":
        *     {
        *         "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *         "FirstName" : "John",
        *         "Email" : "jone@promactinfo.com",
        *         "LastName" : "Doe",
        *         "SlackUserName" :"John",
        *         "IsActive" : "True",
        *         "JoiningDate" :"10-02-2016",
        *         "NumberOfCasualLeave":0,
        *         "NumberOfSickLeave":0,
        *         "UniqueName":null,
        *         "Role":null,
        *         "UserName": null
        *     } 
        *   "ApplicationUsers" :[
        *     {
        *         "Id":"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *         "FirstName" : "Smith",
        *         "Email" : "Smith@promactinfo.com",
        *         "LastName" : "Doe",
        *         "SlackUserName" :"Smith",
        *         "IsActive" : "True",
        *         "JoiningDate" :"10-02-2016",
        *         "NumberOfCasualLeave":0,
        *         "NumberOfSickLeave":0,
        *         "UniqueName":null,
        *         "Role":null,
        *         "UserName": null,
        *         "RoleName": null
        *     },
        *     {
        *         "Id:"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *         "FirstName" : "White",
        *         "Email" : "White@promactinfo.com",
        *         "LastName" : "Doe",
        *         "SlackUserName" :"White",
        *         "IsActive" : "True",
        *         "JoiningDate" :"18-02-2016",
        *         "NumberOfCasualLeave":0,
        *         "NumberOfSickLeave":0,
        *         "UniqueName":null,
        *         "Role":null,
        *         "UserName": null,
        *         "RoleName": null
        *     }
        *   ]   
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
        [Route("{id:int}")]
        public async Task<IActionResult> GetProjectByIdAsync(int id)
        {
            try
            {
               return Ok(await _projectRepository.GetProjectByIdAsync(id));
            }
            catch (ProjectNotFound)
            {
                return NotFound();
            }
        }

        /**
          * @api {post} api/project 
          * @apiVersion 1.0.0
          * @apiName AddProjectAsync
          * @apiGroup Project
          * @apiParam {object} ProjectAc object 
          * @apiParamExample {json} Request-Example:
          * {
          *   "Name":"ProjectName",
          *   "TeamLeaderId":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
          *   "SlackChannelName":"SlackChannelName",
          *   "IsActive":"True",
          *   "TeamLeader": null,
          *   "ApplicationUsers" :[
          *     {
          *         "Id":"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
          *         "FirstName" : "Smith",
          *         "Email" : "Smith@promactinfo.com",
          *         "LastName" : "Doe",
          *         "SlackUserName" :"Smith",
          *         "IsActive" : "True",
          *         "JoiningDate" :"10-02-2016",
          *         "NumberOfCasualLeave":0,
          *         "NumberOfSickLeave":0,
          *         "UniqueName":null,
          *         "Role":null,
          *         "UserName": null,
          *         "RoleName": null
          *     },
          *     {
          *         "Id:"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
          *         "FirstName" : "White",
          *         "Email" : "White@promactinfo.com",
          *         "LastName" : "Doe",
          *         "SlackUserName" :"White",
          *         "IsActive" : "True",
          *         "JoiningDate" :"18-02-2016",
          *         "NumberOfCasualLeave":"0",
          *         "NumberOfSickLeave":"0",
          *         "UniqueName":null,
          *         "Role":null,
          *         "UserName": null,
          *         "RoleName": null
          *     }
          *   ]  
          * }      
          * @apiSuccessExample {json} Success-Response:
          * HTTP/1.1 200 OK 
          * {
          *   "Name":"ProjectName",
          *   "SlackChannelName":"SlackChannelName",
          *   "TeamLeaderId":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
          *   "IsActive":"True",
          *   "TeamLeaderId":"1",
          *   "TeamLeader": null,
          *   "ApplicationUsers" :[
          *     {
          *         "Id":"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
          *         "FirstName" : "Smith",
          *         "Email" : "Smith@promactinfo.com",
          *         "LastName" : "Doe",
          *         "SlackUserName" :"Smith",
          *         "IsActive" : "True",
          *         "JoiningDate" :"10-02-2016",
          *         "NumberOfCasualLeave":0,
          *         "NumberOfSickLeave":0,
          *         "UniqueName":null,
          *         "Role":null,
          *         "UserName": null,
          *         "RoleName": null
          *     },
          *     {
          *         "Id:"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
          *         "FirstName" : "White",
          *         "Email" : "White@promactinfo.com",
          *         "LastName" : "Doe",
          *         "SlackUserName" :"White",
          *         "IsActive" : "True",
          *         "JoiningDate" :"18-02-2016",
          *         "NumberOfCasualLeave":0,
          *         "NumberOfSickLeave":0,
          *         "UniqueName":null,
          *         "Role":null,
          *         "UserName": null,
          *         "RoleName": null
          *     }
          *   ]  
          * }  
          * @apiError BadRequest
          * @apiErrorExample {json} Error-Response:
          * HTTP/1.1 400 Bad Request
          * {
          *   "error": "Problems parsing JSON"
          * }    
          */
        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProjectAsync([FromBody]ProjectAc project)
        {
            
            if (ModelState.IsValid)
            {
                var createdBy = _userManager.GetUserId(User);
                ProjectAc projectAc = await _projectRepository.CheckDuplicateProjectAsync(project);

                if (!string.IsNullOrEmpty(projectAc.Name) && !string.IsNullOrEmpty(projectAc.SlackChannelName))
                {
                    int projectId = await _projectRepository.AddProjectAsync(project, createdBy);
                    foreach (var applicationUser in project.ApplicationUsers)
                    {
                        ProjectUser projectUser = new ProjectUser();
                        projectUser.ProjectId = projectId;
                        projectUser.UserId = applicationUser.Id;
                        projectUser.CreatedBy = createdBy;
                        projectUser.CreatedDateTime = DateTime.UtcNow;
                        await _projectRepository.AddUserProjectAsync(projectUser);
                    }
                    return Ok(project);
                }
                else
                { return Ok(project); }
            }
            else
            {
                return BadRequest();
            }
        }

        /**
        * @api {put} api/project 
        * @apiVersion 1.0.0
        * @apiName EditProjectAsync
        * @apiGroup Project
        * @apiParam {object} ProjectAc object 
        * @apiParamExample {json} Request-Example:
        * {
        *   "Id":"1",
        *   "Name":"ProjectName",
        *   "TeamLeaderId":"1",
        *   "SlackChannelName":"SlackChannelName",
        *   "IsActive":"True",
        *   "TeamLeader":null,
        *   "ApplicationUsers" : [
        *     {
        *         "Id":"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *         "FirstName" : "Smith",
        *         "Email" : "Smith@promactinfo.com",
        *         "LastName" : "Doe",
        *         "SlackUserName" :"Smith",
        *         "IsActive" : "True",
        *         "JoiningDate" :"10-02-2016",
        *         "NumberOfCasualLeave":0,
        *         "NumberOfSickLeave":0,
        *         "UniqueName":null,
        *         "Role":null,
        *         "UserName": null,
        *         "RoleName": null
        *     }
        *  ]
        * }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *   "Id":"1",
        *   "Name":"ProjectName",
        *   "TeamLeaderId":"1",
        *   "SlackChannelName":"SlackChannelName",
        *   "IsActive":"True",
        *   "TeamLeader": null,
        *   "ApplicationUsers" : [
        *     {
        *         "Id":"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *         "FirstName" : "Smith",
        *         "Email" : "Smith@promactinfo.com",
        *         "LastName" : "Doe",
        *         "SlackUserName" :"Smith",
        *         "IsActive" : "True",
        *         "JoiningDate" :"10-02-2016",
        *         "NumberOfCasualLeave":0,
        *         "NumberOfSickLeave":0,
        *         "UniqueName":null,
        *         "Role":null,
        *         "UserName": null,
        *         "RoleName": null
        *     }
        *  ]
        * }
        * @apiError BadRequest
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 404 ProjectNotFound Project Not Found
        * {
        *   "error": "ProjectNotFound"
        * }    
        */
        [Authorize]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> EditProjectAsync(int id,[FromBody]ProjectAc project)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    var updatedBy = _userManager.GetUserId(User);
                    ProjectAc projectAc = await _projectRepository.CheckDuplicateProjectAsync(project);
                    if (!string.IsNullOrEmpty(projectAc.Name) && !string.IsNullOrEmpty(projectAc.SlackChannelName))
                    {
                        await _projectRepository.EditProjectAsync(id, project, updatedBy);
                    }
                    else { return Ok(project); }
                }
                return Ok(project);
            }
            catch (ProjectNotFound)
            {
                return NotFound();
            }

        }

        /**
        * @api {get} api/project/:name 
        * @apiVersion 1.0.0
        * @apiName GetProjectByGroupNameAsync
        * @apiGroup Project
        * @apiParam {string} name project Name
        * @apiParamExample {json} Request-Example:
        * {
        *   "Name":"ProjectName"
        * }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *    
        *   "Name":"ProjectName",
        *   "SlackChannelName":"SlackChannelName",
        *   "IsActive":"True",
        *   "TeamLeaderId":"1",
        *   "ApplicationUsers":null
        * }
        * @apiError ProjectNotFound The id of the Project was not found.
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 404 Not Found
        * {
        *   "error": "ProjectNotFound"
        * }
        */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetProjectByGroupNameAsync(string name)
        {
            try
            {
                return Ok(await _projectRepository.GetProjectByGroupNameAsync(name));
            }
            catch (ProjectNotFound)
            {
                return NotFound();
            }
        }

        /**
      * @api {get} api/Project/allProjects 
      * @apiVersion 1.0.0
      * @apiName AllProjectsAsync
      * @apiGroup Project  
      * @apiSuccessExample {json} Success-Response:
      * HTTP/1.1 200 OK 
      * [
      *  {
      *   "Name":"ProjectName",
      *   "SlackChannelName":"SlackChannelName",
      *   "IsActive":"True",
      *   "TeamLeaderId":"1",
      *   "ApplicationUsers":null
      *  }
      * ]
      */
        [HttpGet]
        [Route("list")]
        public async Task<IEnumerable<ProjectAc>> AllProjectsAsync()
        {
            return await _projectRepository.GetProjectsWithUsersAsync();
        }

        /**
        * @api {get} api/Project/projectDetails/:projectId 
        * @apiVersion 1.0.0
        * @apiName ProjectDetailsAsync
        * @apiGroup Project
        * @apiParam {int} id  projectId
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             "id": "1"
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        *  {
        *   "Name":"ProjectName",
        *   "SlackChannelName":"SlackChannelName",
        *   "IsActive":"True",
        *   "TeamLeaderId":"1",
        *   "ApplicationUsers":null
        *  }
        * @apiError ProjectNotFound The id of the Project was not found.
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 404 Not Found
        * {
        *   "error": "ProjectNotFound"
        * }
        */
        [HttpGet]
        [Route("{projectId:int}/detail")]
        public async Task<IActionResult> ProjectDetailsAsync(int projectId)
        {
            try
            {
                return Ok(await _projectRepository.GetProjectDetailsAsync(projectId));
            }
            catch (ProjectNotFound)
            {
                return NotFound();
            }

        }
        #endregion
    }
}