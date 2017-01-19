using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System.Threading.Tasks;
using System.Collections.Generic;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.Services;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        #region "Private Variable(s)"
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringConstant _stringConstant;
        #endregion

        #region "Constructor"
        public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, IStringConstant stringConstant)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _stringConstant = stringConstant;
        }

        #endregion

        #region "Public Methods"

        /**
        * @api {get} api/users 
        * @apiVersion 1.0.0
        * @apiName GetAllUsersAsync
        * @apiGroup User
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * [
        *   {
        *       "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *       "FirstName" : "John",
        *       "Email" : "jone@promactinfo.com",
        *       "LastName" : "Doe",
        *       "SlackUserName" :"John",
        *       "IsActive" : "True",
        *       "JoiningDate" :"10-02-2016",
        *       "NumberOfCasualLeave":0,
        *       "NumberOfSickLeave":0,
        *       "UniqueName":null,
        *       "Role":null,
        *       "UserName": null
        *   }
        * ]
        */
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return Ok(await _userRepository.GetAllUsersAsync());
        }

        /**
        * @api {get} api/users/orderby/name 
        * @apiVersion 1.0.0
        * @apiName GetEmployeesAsync
        * @apiGroup User
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * [
        *   {
        *       "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *       "FirstName" : "John",
        *       "Email" : "jone@promactinfo.com",
        *       "LastName" : "Doe",
        *       "SlackUserName" :"John",
        *       "IsActive" : "True",
        *       "JoiningDate" :"10-02-2016",
        *       "NumberOfCasualLeave":0,
        *       "NumberOfSickLeave":0,
        *       "UniqueName":null,
        *       "Role":null,
        *       "UserName": null
        *   }
        * ]
        */
        [HttpGet]
        [Route("orderby/name")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            return Ok(await _userRepository.GetAllEmployeesAsync());
        }

        /**
         * @api {get} api/users/roles 
         * @apiVersion 1.0.0
         * @apiName GetRoleAsync
         * @apiGroup User
         * @apiSuccessExample {json} Success-Response:
         * HTTP/1.1 200 OK 
         * {
         *     [
         *      {
         *          Id: 452dsf34,
         *          Name: Employee
         *      },
         *     ]
         * }
         */
        [HttpGet]
        [Route("roles")]
        [Authorize]
        public async Task<IActionResult> GetRoleAsync()
        {
            return Ok(await _userRepository.GetRolesAsync());
        }

        /**
        * @api {get} api/users/:id 
        * @apiVersion 1.0.0
        * @apiName GetUserByIdAsync
        * @apiGroup User
        * @apiParam {string} id
        * @apiParamExample {json} Request-Example:
        * {
        *   "id": "34d1af3d-062f-4bcd-b6f9-b8fd5165e367"
        * }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *   "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *   "FirstName" : "John",
        *   "Email" : "jone@promactinfo.com",
        *   "LastName" : "Doe",
        *   "SlackUserName" :"John",
        *   "IsActive" : "True",
        *   "JoiningDate" :"10-02-2016",
        *   "NumberOfCasualLeave":0,
        *   "NumberOfSickLeave":0,
        *   "UniqueName":null,
        *   "Role":null,
        *   "UserName": null
        * }
        * @apiError UserNotFound user id not found.
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 404 Not Found
        * {
        *   "error": "UserNotFound"
        * }
        */
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            try
            {
                return Ok(await _userRepository.GetByIdAsync(id));
            }
            catch (UserNotFound)
            {
                return NotFound();
            }
        }


        /**
        * @api {post} api/users 
        * @apiVersion 1.0.0
        * @apiName RegisterUserAsync
        * @apiParam {object} newUser  object
        * @apiParamExample {json} Request-Example:
        * {
        *    "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *    "FirstName" : "John",
        *    "Email" : "jone@promactinfo.com",
        *    "LastName" : "Doe",
        *    "SlackUserName" :"John",
        *    "IsActive" : "True",
        *    "JoiningDate" :"10-02-2016",
        *    "NumberOfCasualLeave":0,
        *    "NumberOfSickLeave":0,
        *    "UniqueName":null,
        *    "Role":null,
        *    "UserName": null
        * }
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     true
        * }
        * @apiError BadRequest
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 400 Bad Request
        * {
        *   "error": "Problems parsing JSON object"
        * }
        */
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserAc newUser)
        {
            if (ModelState.IsValid)
            {
                string createdBy = _userManager.GetUserId(User);
                await _userRepository.AddUserAsync(newUser, createdBy);
                return Ok(true);
            }
            return BadRequest();
        }


        /**
        * @api {put} api/users/:id 
        * @apiVersion 1.0.0
        * @apiName UpdateUserAsync
        * @apiGroup User
        * @apiParam {string} id
        * @apiParam {object} editedUser  object
        * @apiParamExample {json} Request-Example:
        *  "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *   {
        *    "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *    "FirstName" : "John",
        *    "Email" : "jone@promactinfo.com",
        *    "LastName" : "Doe",
        *    "SlackUserName" :"John",
        *    "IsActive" : "True",
        *    "JoiningDate" :"10-02-2016",
        *    "NumberOfCasualLeave":0,
        *    "NumberOfSickLeave":0,
        *    "UniqueName":null,
        *    "Role":null,
        *    "UserName": null
        *   }
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     true
        * }
        * @apiError BadRequest
        * @apiError SlackUserNotFound the slack user was not found.
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 400 Bad Request
        * {
        *   "error": "Problems parsing JSON object"
        * }
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 404 Not Found
        * {
        *   "error": "SlackUserNotFound"
        * }
        */
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] UserAc editedUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string updatedBy = _userManager.GetUserId(User);
                    editedUser.Id = id;
                    await _userRepository.UpdateUserDetailsAsync(editedUser, updatedBy);
                    return Ok(true);
                }
                return BadRequest();
            }
            catch (SlackUserNotFound)
            {
                return NotFound();
            }
        }


        /**
        * @api {post} api/users/password 
        * @apiVersion 1.0.0
        * @apiName ChangePasswordAsync
        * @apiGroup User
        * @apiParam {object} passwordModel object
        * @apiParamExample {json} Request-Example:
        * {
        *     "OldPassword":"OldPassword",
        *     "NewPassword":"NewPassword",
        *     "ConfirmPassword":"ConfirmPassword"
        *            
        * }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     Message : If password is changed successfully, return empty otherwise error message
        * }
        * @apiError UserNotFound the slack user was not found.
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 404 Not Found
        * {
        *   "error": "UserNotFound"
        * }
        */
        [HttpPost]
        [Route("password")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordViewModel passwordModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                passwordModel.Email = user.Email;
                return Ok(await _userRepository.ChangePasswordAsync(passwordModel));
            }
            catch (UserNotFound)
            {
                return NotFound();
            }
        }

        /**
       * @api {get} api/users/:password/available 
       * @apiVersion 1.0.0
       * @apiName CheckPasswordAsync
       * @apiGroup User
       * @apiParam {string} password  User OldPassword
       * @apiParamExample {json} Request-Example:
       * {
       *     "password":"OldPassword123"
       * }      
       * @apiSuccessExample {json} Success-Response:
       * HTTP/1.1 200 OK 
       * {
       *     true
       * }
       */
        [HttpGet]
        [Route("{password}/available")]
        [Authorize]
        public async Task<ActionResult> CheckPasswordAsync(string password)
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(await _userManager.CheckPasswordAsync(user, password));
        }

        /**
       * @api {get} api/users/available/email/:email
       * @apiVersion 1.0.0
       * @apiName CheckEmailIsExistsAsync
       * @apiGroup User
       * @apiParam {string} email  user email
       * @apiParamExample {json} Request-Example:
       * {
       *     "email":"jone@promactinfo.com"
       * }      
       * @apiSuccessExample {json} Success-Response:
       * HTTP/1.1 200 OK 
       * {
       *   true
       * }
       */
        [HttpGet]
        [Route("available/email/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckEmailIsExistsAsync(string email)
        {
            return Ok(await _userRepository.CheckEmailIsExistsAsync(email + _stringConstant.DomainAddress));
        }

        /**
         * @api {get} api/users/available/:slackUserName 
         * @apiVersion 1.0.0
         * @apiName CheckUserIsExistsBySlackUserNameAsync
         * @apiGroup User
         * @apiParam {string} slackUserName  user slack name
         * @apiParamExample {json} Request-Example:
         * {
         *     "slackUserName" :"John"
         * }      
         * @apiSuccessExample {json} Success-Response:
         * HTTP/1.1 200 OK 
         * {
         *   "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
         *   "FirstName" : "John",
         *   "Email" : "jone@promactinfo.com",
         *   "LastName" : "Doe",
         *   "SlackUserName" :"John",
         *   "IsActive" : "True",
         *   "JoiningDate" :"10-02-2016",
         *   "NumberOfCasualLeave":0,
         *   "NumberOfSickLeave":0,
         *   "UniqueName":null,
         *   "Role":null,
         *   "UserName": null
         * }
         * @apiError SlackUserNotFound the slack user was not found.
         * @apiErrorExample {json} Error-Response:
         * HTTP/1.1 404 Not Found
         * {
         *   "error": "SlackUserNotFound"
         * }
         */
        [HttpGet]
        [Route("available/{slackUserName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckUserIsExistsBySlackUserNameAsync(string slackUserName)
        {
            try
            {
                return Ok(await _userRepository.FindUserBySlackUserNameAsync(slackUserName));
            }
            catch (SlackUserNotFound)
            {
                return NotFound();
            }
        }

        /**
          * @api {get} api/users/:userId/detail 
          * @apiVersion 1.0.0
          * @apiName UserDetailByIdAsync
          * @apiGroup User
          * @apiParam {string} userId
          * @apiParamExample {json} Request-Example:    
          *        {
          *             "userId": "95151b57-42c5-48d5-84b6-6d20e2fb05cd"
          *        }      
          * @apiSuccessExample {json} Success-Response:
          * HTTP/1.1 200 OK 
          * {
          *     "description":"Object of type UserAc "   
          *     {
          *         "Id": "95151b57-42c5-48d5-84b6-6d20e2fb05cd",
          *         "FirstName": "Admin",
          *         "LastName": "Promact",
          *         "IsActive": true,
          *         "Role": "Admin",
          *         "NumberOfCasualLeave": 14,
          *         "NumberOfSickLeave": 7,
          *         "JoiningDate": "0001-01-01T00:00:00",
          *         "SlackUserName": "roshni",
          *         "SlackUserId": "U70787887",
          *         "Email": "roshni@promactinfo.com",
          *         "UserName": "roshni@promactinfo.com",
          *         "UniqueName": "Admin-roshni@promactinfo.com",
          *     }
          * }
          */
        [Authorize("ReadUser")]
        [HttpGet]
        [Route("{userId}/detail")]
        public async Task<IActionResult> UserDetailByIdAsync(string userId)
        {
            return Ok(await _userRepository.UserDetailByIdAsync(userId));
        }


        /**
          * @api {get} api/users/email/:id/send 
          * @apiVersion 1.0.0
          * @apiName ReSendMailAsync
          * @apiParam {string} id
          * @apiParamExample {json} Request-Example:   
          *        {
          *             "id": "adssdvvsdv55gdfgdsgbc"
          *        }      
          * @apiSuccessExample {json} Success-Response:
          * HTTP/1.1 200 OK 
          * {
          *     "true"
          * }
          */
        [HttpGet]
        [Route("email/{id}/send")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReSendMailAsync(string id)
        {
            await _userRepository.ReSendMailAsync(id);
            return Ok(true);
        }


        /**
       * @api {get} api/users/:userId/role 
       * @apiVersion 1.0.0
       * @apiName GetUserRoleAsync
       * @apiGroup User
       * @apiParam {string} name userId
       * @apiParamExample {json} Request-Example:
       * {
       *     "userId":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367"    
       * }      
       * @apiSuccessExample {json} Success-Response:
       * HTTP/1.1 200 OK 
       * [
       *        {
       *            "UserId": "34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
       *            "UserName": "smith@promactinfo.com",
       *            "Name":"Smith",
       *            "Role":"Admin"
       *        }
       *]
       */
        [Authorize("ReadUser")]
        [HttpGet]
        [Route("{userId}/role")]
        public async Task<IActionResult> GetUserRoleAsync(string userId)
        {
            return Ok(await _userRepository.GetUserRoleAsync(userId));
        }


        /**
        * @api {get} api/users/:userId/teammembers 
        * @apiVersion 1.0.0
        * @apiName GetTeamMembersAsync
        * @apiGroup User
        * @apiParam {string} userId
        * @apiParamExample {json} Request-Example:
        * {
        *   "userId":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367"    
        * }
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * [
        *        {
        *            "UserId": "34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *            "UserName": "smith@promactinfo.com",
        *            "Name":"Smith",
        *            "Role":"Admin"
        *        },
        *        {
        *            "UserId": "avd1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *            "UserName": "john@promactinfo.com",
        *            "Name":"John",
        *            "Role":"Employee"
        *        }
        *    ]
        */
        [Authorize("ReadUser")]
        [HttpGet]
        [Route("{userId}/teammembers")]
        public async Task<IActionResult> GetTeamMembersAsync(string userId)
        {
            return Ok(await _userRepository.GetTeamMembersAsync(userId));
        }

        /**
        * @api {get} api/users/slackChannel/:name 
        * @apiVersion 1.0.0
        * @apiName GetProjectUserByGroupNameAsync
        * @apiGroup User
        * @apiParam {string} name as a SlackChannelName
        * @apiParamExample {json} Request-Example:
        * {
        *   "name":"SlackChannelName"
        * }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * [
        *     {
        *         "Id:"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *         "FirstName" : "Smith",
        *         "Email" : "Smith@promactinfo.com",
        *         "LastName" : "Doe",
        *         "SlackUserName" :"Smith",
        *         "IsActive" : "True",
        *         "JoiningDate" :"10-02-2016",
        *         "Password" : null
        *     },
        *     {
        *         "Id":"abcd1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *         "FirstName" : "White",
        *         "Email" : "White@promactinfo.com",
        *         "LastName" : "Doe",
        *         "SlackUserName" :"White",
        *         "IsActive" : "True",
        *         "JoiningDate" :"18-02-2016",
        *         "Password" : null
        *     }
        *   ]   
        * @apiError UserNotFound The User not found.
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 404 Not Found
        * {
        *  "error": "UserNotFound"
        * }
        */
        [HttpGet]
        [Route("slackChannel/{name}")]
        public async Task<IActionResult> GetProjectUserByGroupNameAsync(string name)
        {
            try
            {
                return Ok(await _userRepository.GetProjectUserBySlackChannelNameAsync(name));
            }
            catch (UserNotFound)
            {
                return NotFound();
            }
        }

        /**
        * @api {get} api/users/:teamLeaderId/project
        * @apiVersion 1.0.0
        * @apiName GetProjectUsersByTeamLeaderIdAsync
        * @apiGroup User
        * @apiParam {string}  teamLeaderId
        * @apiParamExample {json} Request-Example:     
        *        {
        *             "teamLeaderId": "95151b57-42c5-48d5-84b6-6d20e2fb05cd"
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"list of projects with users for that specific teamleader"
        *     
        *     [
        *       {
        *           "Id": "95151b57-42c5-48d5-84b6-6d20e2fb05cd",
        *           "FirstName": "Admin",
        *           "LastName": "Promact",
        *           "IsActive": true,
        *           "Role": "TeamLeader",
        *           "NumberOfCasualLeave": 14,
        *           "NumberOfSickLeave": 7,
        *           "JoiningDate": "0001-01-01T00:00:00",
        *           "SlackUserName": "roshni",
        *           "SlackUserId": "U70787887",
        *           "Email": "roshni@promactinfo.com",
        *           "UserName": "roshni@promactinfo.com",
        *           "UniqueName": "Admin-roshni@promactinfo.com",
        *        },
        *        {
        *           "Id": "bbd66866-8e35-430a-9f66-8cb550e72f9e",
        *           "FirstName": "gourav",
        *           "LastName": "gourav",
        *           "IsActive": true,
        *           "Role": "Employee",
        *           "NumberOfCasualLeave": 14,
        *           "NumberOfSickLeave": 7,
        *           "JoiningDate": "2016-07-20T18:30:00",
        *           "SlackUserName": "gourav",
        *           "Email": "gourav@promactinfo.com",
        *           "UserName": "gourav@promactinfo.com",
        *           "UniqueName": "gourav-gourav@promactinfo.com",
        *        }
        *      ]
        * }
        */
        [Authorize("ReadUser")]
        [HttpGet]
        [Route("{teamLeaderId}/project")]
        public async Task<IActionResult> GetProjectUsersByTeamLeaderIdAsync(string teamLeaderId)
        {
            List<UserAc> projectUsers = await _userRepository.GetProjectUsersByTeamLeaderIdAsync(teamLeaderId);
            return Ok(projectUsers);
        }

        #endregion
    }
}
