using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ProjectsRepository;
using Promact.Oauth.Server.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.Constants;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        #region "Private Variable(s)"
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringConstant _stringConstant;
        public const string ReadProject = "ReadProject";
        #endregion

        #region "Constructor"
        public ProjectController(IProjectRepository projectRepository, UserManager<ApplicationUser> userManager, IStringConstant stringConstant)
        {
            _projectRepository = projectRepository;
            _userManager = userManager;
            _stringConstant = stringConstant;
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
            var isRoleExists = await _userManager.IsInRoleAsync(user, _stringConstant.Employee);
            if (isRoleExists)
            {
                return Ok(await _projectRepository.GetAllProjectForUserAsync(user.Id));
            }
            else
            {
                return Ok(await _projectRepository.GetAllProjectsAsync());
            }
        }

        /**
        * @api {get} api/project/:id 
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
        * @api {put} api/project/:id 
        * @apiVersion 1.0.0
        * @apiName EditProjectAsync
        * @apiGroup Project
        * @apiParam {id} project Id.
        * @apiParam {object} ProjectAc object 
        * @apiParamExample {json} Request-Example:
        * "Id":"1",
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
        public async Task<IActionResult> EditProjectAsync(int id, [FromBody]ProjectAc project)
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
        * @api {get} api/project/:slackChannelName 
        * @apiVersion 1.0.0
        * @apiName GetProjectBySlackChannelNameAsync
        * @apiGroup Project
        * @apiParam {string} SlackChannelName Slack Channel Name
        * @apiParamExample {json} Request-Example:
        * {
        *   "slackChannelName":"SlackChannelName"
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
        */
        [Authorize(Policy = ReadProject)]
        [HttpGet]
        [Route("{slackChannelName}")]
        public async Task<IActionResult> GetProjectBySlackChannelNameAsync(string slackChannelName)
        {
            return Ok(await _projectRepository.GetProjectBySlackChannelNameAsync(slackChannelName));
        }

        /**
      * @api {get} api/project/list 
      * @apiVersion 1.0.0
      * @apiName GetProjectList
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
        [Authorize(Policy = ReadProject)]
        [HttpGet]
        [Route("list")]
        public async Task<IEnumerable<ProjectAc>> AllProjectsAsync()
        {
            return await _projectRepository.GetProjectsWithUsersAsync();
        }

        /**
        * @api {get} api/project/:projectId/detail
        * @apiVersion 1.0.0
        * @apiName GetProjectDetail
        * @apiGroup Project
        * @apiParam {int} id  projectId
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             "projectId": "1"
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
        [Authorize(Policy = ReadProject)]
        [HttpGet]
        [Route("{projectId:int}/detail")]
        public async Task<IActionResult> ProjectDetailsAsync(int projectId)
        {
            return Ok(await _projectRepository.GetProjectDetailsAsync(projectId));
        }

        /**
        * @api {get} api/project/detail/:userId
        * @apiVersion 1.0.0
        * @apiName GetListOfProjectsEnrollmentOfUserByUserIdAsync
        * @apiGroup Project
        * @apiParam {string} userId  userId
        * @apiParamExample {json} Request-Example:
        *        {
        *             "userId": "skgnskdgfsdssdvsdj"
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        *  [{
        *   "Id":"1"
        *   "Name":"ProjectName",
        *   "SlackChannelName":"SlackChannelName",
        *   "IsActive":"True",
        *   "TeamLeaderId":"1",
        *   "ApplicationUsers":null
        *  }]
        */
        [Authorize(Policy = ReadProject)]
        [HttpGet]
        [Route("detail/{userId}")]
        public async Task<IActionResult> GetListOfProjectsEnrollmentOfUserByUserIdAsync(string userId)
        {
            return Ok(await _projectRepository.GetListOfProjectsEnrollmentOfUserByUserIdAsync(userId));
        }


        /**
        * @api {get} api/project/user/projectId
        * @apiVersion 1.0.0
        * @apiName GetListOfTeamMemberByProjectIdAsync
        * @apiGroup Project
        * @apiParam {int} projectId  projectId
        * @apiParamExample {json} Request-Example:
        *        {
        *             "projectId": "1"
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        *  [{
        *         "Id":"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *         "FirstName" : "Smith",
        *         "Email" : "Smith@promactinfo.com",
        *         "LastName" : "Doe",
        *         "IsActive" : "True",
        *         "JoiningDate" :"10-02-2016",
        *         "NumberOfCasualLeave":0,
        *         "NumberOfSickLeave":0,
        *         "UniqueName":null,
        *         "Role":null,
        *         "UserName": null,
        *         "RoleName": null
        *     }]
        */
        [Authorize(Policy = ReadProject)]
        [HttpGet]
        [Route("user/{projectId:int}")]
        public async Task<IActionResult> GetListOfTeamMemberByProjectIdAsync(int projectId)
        {
            return Ok(await _projectRepository.GetListOfTeamMemberByProjectIdAsync(projectId));
        }


        /**
        * @api {put} api/project/projectDetail
        * @apiVersion 1.0.0
        * @apiName EditProjectAsync
        * @apiGroup Project
        * @apiParam {id} project Id.
        * @apiParam {object} ProjectAc object 
        * @apiParamExample {json} Request-Example:
        * "Id":"1",
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
        *   "IsActive":"True",
        *   "TeamLeader": null,
        *   "ApplicationUsers" : [
        *     {
        *         "Id":"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *         "FirstName" : "Smith",
        *         "Email" : "Smith@promactinfo.com",
        *         "LastName" : "Doe",
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
        */
        [Authorize(Policy = ReadProject)]
        [HttpGet]
        [Route("projectDetail/{projectId}")]
        public async Task<IActionResult> GetProjectByProjectIdAsync(int projectId)
        {
            return Ok(await _projectRepository.GetProjectByProjectIdAsync(projectId));
        }

        
        /**
        * @api {get} api/project/:slackChannelName 
        * @apiVersion 1.0.0
        * @apiName GetProjectBySlackChannelNameAsync
        * @apiGroup Project
        * @apiParam {string} SlackChannelName Slack Channel Name
        * @apiParamExample {json} Request-Example:
        * {
        *   "projectName":"projectName"
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
        */
        [Authorize(Policy = ReadProject)]
        [HttpGet]
        [Route("{all}")]
        public async Task<IActionResult> GetProjectByProjectNameAsync()
        {
            return Ok(await _projectRepository.GetProjectListAsync());
        }


        #endregion
    }
}