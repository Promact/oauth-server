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
        #region "Private Variable(s)"
        private readonly PromactOauthDbContext _appDbContext;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        #endregion

        #region "Constructor"
        public ProjectController(PromactOauthDbContext appContext, IProjectRepository projectRepository, UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _appDbContext = appContext;
            _userManager = userManager;
            _userRepository = userRepository;
        }
        #endregion

        #region public Methods
        /**
     * @api {get} api/Project/projects 
     * @apiVersion 1.0.0
     * @apiName Project
     * @apiGroup Project
     * @apiSuccessExample {json} Success-Response:
     * HTTP/1.1 200 OK 
     * {
     *     "description":"Get List of Projects"
     * }
     */
        [HttpGet]
        [Route("getAllProjects")]
        public async Task<IEnumerable<ProjectAc>> getAllProjects()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userRole = await _userManager.IsInRoleAsync(user, "Employee");
                if (userRole==true)
                {
                    return await _projectRepository.GetAllProjectForUser(user.Id);
                }
                else
                {
                    return await _projectRepository.GetAllProjects();
                }
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }
        /**
      * @api {get} api/Project/getProjects/:id 
      * @apiVersion 1.0.0
      * @apiName Project
      * @apiGroup Project
      * @apiParam {int} id  project Id
      * @apiParamExample {json} Request-Example:
      *      
      *        {
      *             "id": "1"
      *             "description":"get the ProjectAc Object"
      *        }      
      * @apiSuccessExample {json} Success-Response:
      * HTTP/1.1 200 OK 
      * {
      *     "id":"1"
      *     "description":"get the ProjectAc Object"
      * }
      */

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

        /**
      * @api {post} api/Project/addProject 
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
      *     "description":"Add Project in ProjectTable"
      * }
      */
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

        /**
        * @api {put} api/Project/editProject 
        * @apiVersion 1.0.0
        * @apiName Project
        * @apiGroup Project
        *  @apiParam {int} Id  Project Id
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
        *     "description":"edit Project in ProjectTable"
        * }
        */
        [HttpPut]
        [Route("editProject")]
        public async Task<IActionResult> editProject(int id, [FromBody]ProjectAc project)
        {
            try
            {
                var updatedBy = _userManager.GetUserId(User);

                if (ModelState.IsValid)
                {
                    ProjectAc projectAc = _projectRepository.checkDuplicateFromEditProject(project);
                    if (projectAc.Name != null && projectAc.SlackChannelName != null)
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
        *     "description":"Object of ProjectAc"
        * }
        */
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

        /**
        * @api {get} api/Project/GetUserRole 
        * @apiVersion 1.0.0
        * @apiName Project
        * @apiGroup Project
        * @apiParam {string} name UserName
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             
        *             "Name":"UserName"
        *            
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Object of UserRoleAc"
        * }
        */
        [HttpGet]
        [Route("featchUserRole/{name}")]
        public async Task<List<UserRoleAc>> GetUserRole(string name)
        {
            return await _projectRepository.GetUserRole(name);

        }


        /**
        * @api {get} api/Project/GetListOfEmployee 
        * @apiVersion 1.0.0
        * @apiName Project
        * @apiGroup Project
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Get List of Users"
        * }
        */
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
      *     "description":"List of Object of UserAc"
      * }
      */
        [HttpGet]
        [Route("fetchProjectUsers/{groupName}")]
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

        /**
      * @api {get} api/Project/allProjects 
      * @apiVersion 1.0.0
      * @apiName Project
      * @apiGroup Project  
      * @apiSuccessExample {json} Success-Response:
      * HTTP/1.1 200 OK 
      * {
      *     "description":"List of Object of ProjectAc"
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
      *     "description":"Object of type ProjectAc "
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