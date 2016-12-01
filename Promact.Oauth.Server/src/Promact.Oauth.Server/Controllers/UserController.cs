using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Exceptions;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.Services;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    // [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        #region "Private Variable(s)"
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly IStringConstant _stringConstant;


        #endregion

        #region "Constructor"

        public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, ILogger<UserController> logger, IStringConstant stringConstant)
        {
            _userRepository = userRepository;
            _logger = logger;
            _userManager = userManager;
            _stringConstant = stringConstant;
        }

        #endregion

        #region public Methods
        /**
        * @api {get} api/User/AllUsers 
        * @apiVersion 1.0.0
        * @apiName User
        * @apiGroup User
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Get List of Users"
        * }
        */
        [HttpGet]
        [Route("users")]
        [Authorize(Roles = "Admin")]
        public IActionResult AllUsers()
        {
            try
            {
                return Ok(_userRepository.GetAllUsers());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /**
        * @api {get} api/User/GetEmployees 
        * @apiVersion 1.0.0
        * @apiName User
        * @apiGroup User
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Get List of Employees"
        * }
        */
        [HttpGet]
        [Route("getEmployees")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var user = await _userRepository.GetAllEmployees();
                return Ok(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /**
     * @api {get} api/User/GetRole 
     * @apiVersion 1.0.0
     * @apiName User
     * @apiGroup User
     * @apiSuccessExample {json} Success-Response:
     * HTTP/1.1 200 OK 
     * {
     *     [
     *      {
     *          Id: 452dsf34,
     *          Name: abc
     *      },
     *     ]
     * }
     */
        [HttpGet]
        [Route("getRole")]
        public IActionResult GetRole()
        {
            List<RolesAc> listofRoles = _userRepository.GetRoles();
            return Ok(listofRoles);
        }


        /**
        * @api {get} api/User/GetUserById/:id 
        * @apiVersion 1.0.0
        * @apiName User
        * @apiGroup User
        * @apiParam {int} id User Id
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             "id": "1"
        *             "description":"get the UserAc Object"
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "id":"1"
        *     "description":"get the UserAc Object"
        * }
        */
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /**
         * @api {post} api/User/RegisterUser 
         * @apiVersion 1.0.0
         * @apiName User
         * @apiGroup User
         * @apiParamExample {json} Request-Example:
         *      
         *        {
         *             "FirstName":"ProjectName",
         *             "LastName":"SlackChannelName",
         *             "IsActive":"True",
         *             "JoiningDate":"01-01-2016"
         *             "SlackUserId":"SlackUserId"
         *        }      
         * @apiSuccessExample {json} Success-Response:
         * HTTP/1.1 200 OK 
         * {
         *     "description":"Add User in Application User Table"
         * }
         */
        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterUser([FromBody] UserAc newUser)
        {
            try
            {
                string createdBy = _userManager.GetUserId(User);
                if (ModelState.IsValid)
                {
                    await _userRepository.AddUser(newUser, createdBy);
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (InvalidApiRequestException apiEx)
            {
                _logger.LogError("Forgot Password mail not send " + apiEx.Message + apiEx.ToString());
                if (apiEx.Errors.Length > 0)
                {
                    foreach (var error in apiEx.Errors)
                        _logger.LogError("Forgot Password mail not send " + error);
                }
                throw apiEx;
            }
            catch (ArgumentNullException argEx)
            {
                _logger.LogError("Add User unsuccessful " + argEx.Message + argEx.ToString());
                throw argEx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Add User unsuccessful " + ex.Message + ex.ToString());
                throw ex;
            }
        }


        /**
        * @api {put} api/User/editProject 
        * @apiVersion 1.0.0
        * @apiName User
        * @apiGroup User
        * @apiParam {int} id  User Id
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             "Id":"1",
        *             "FirstName":"ProjectName",
        *             "LastName":"SlackChannelName",
        *             "IsActive":"True",
        *             "JoiningDate":"01-01-2016"
        *             "SlackUserName":"SlackUserName"
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"edit User in User Table"
        * }
        */
        [HttpPut]
        [Route("edit")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> UpdateUser([FromBody] UserAc editedUser)
        {
            try
            {
                string updatedBy = _userManager.GetUserId(User);
                if (ModelState.IsValid)
                {
                    string id = await _userRepository.UpdateUserDetails(editedUser, updatedBy);
                    if (id != "")
                    { return Ok(true); }
                    //else { return Ok(false); }
                }
                return Ok(false);
            }
            catch (SlackUserNotFound ex)
            {
                _logger.LogInformation("User not Found");
                return NotFound();
            }
        }



        /**
        * @api {post} api/User/ChangePasswordViewModel 
        * @apiVersion 1.0.0
        * @apiName User
        * @apiGroup User
        * @apiParam {string} OldPassword  User OldPassword
        * @apiParam {string} NewPassword  User NewPassword
        * @apiParam {string} ConfirmPassword  User ConfirmPassword
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             "OldPassword":"OldPassword",
        *             "NewPassword":"NewPassword",
        *             "ConfirmPassword":"ConfirmPassword"
        *            
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Change Password"
        * }
        */
        [HttpPost]
        [Route("changepassword")]
        [AllowAnonymous]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel passwordModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                passwordModel.Email = user.Email;
                string response = await _userRepository.ChangePassword(passwordModel);
                return Ok(new { response });
            }
            catch (UserNotFound ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("checkOldPasswordIsValid/{oldPassword}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> CheckOldPasswordIsValid(string oldPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.CheckPasswordAsync(user, oldPassword))
                return Ok(true);
            else
                return Ok(false);
        }


        [HttpGet]
        [Route("findbyusername/{userName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindByUserName(string userName)
        {
            try
            {
                return Ok(await _userRepository.FindByUserName(userName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        [Route("fetchbyusername/{userName}")]
        public async Task<IActionResult> FetchByUserName(string userName)
        {
            try
            {
                return Ok(await _userRepository.GetUserDetail(userName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to check if a user already exists in the database with the given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("checkEmailIsExists/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckEmailIsExists(string email)
        {
            try
            {
                return Ok(await _userRepository.CheckEmailIsExists(email + _stringConstant.DomainAddress));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("checkUserIsExistsBySlackUserName/{slackUserName}")]
        public IActionResult CheckUserIsExistsBySlackUserName(string slackUserName)
        {
            try
            {
                ApplicationUser slackUser = _userRepository.FindUserBySlackUserName(slackUserName);
                bool result = slackUser != null ? true : false;

                return Ok(result);
            }
            catch (SlackUserNotFound ex)
            {
                _logger.LogInformation("User not Found");
                return NotFound();
            }
        }


        /**
          * @api {get} api/User/{userId}/detail
          * @apiVersion 1.0.0
          * @apiName GetUserDetails
          * @apiGroup User
          * @apiParam {string} id userId
          * @apiParamExample {json} Request-Example:
          *      
          *        {
          *             "id": "95151b57-42c5-48d5-84b6-6d20e2fb05cd"
          *        }      
          * @apiSuccessExample {json} Success-Response:
          * HTTP/1.1 200 OK 
          * {
          *     "description":"Object of type UserAc "
          *     
          *     {
          *         "Id": "95151b57-42c5-48d5-84b6-6d20e2fb05cd",
          *         "FirstName": "Admin",
          *         "LastName": "Promact",
          *         "IsActive": true,
          *         "Role": "Admin",
          *         "NumberOfCasualLeave": 0,
          *         "NumberOfSickLeave": 0,
          *         "JoiningDate": "0001-01-01T00:00:00",
          *         "SlackUserName": "roshni",
          *         "SlackUserId": "U70787887",
          *         "Email": "roshni@promactinfo.com",
          *         "UserName": "roshni@promactinfo.com",
          *         "UniqueName": "Admin-roshni@promactinfo.com",
          *     }
          * }
          */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("{userId}/detail")]
        public async Task<IActionResult> UserDetailByIdAsync(string userId)
        {
            try
            {
                var user = await _userRepository.UserDetailByIdAsync(userId);
                return Ok(user);
            }
            catch (UserNotFound)
            {
                _logger.LogInformation("User not Found");
                return NotFound();
            }
        }

        /**
          * @api {get} api/User/{userName}/details 
          * @apiVersion 1.0.0
          * @apiName User
          * @apiGroup User
          * @apiParam {string} Name userName
          * @apiParamExample {json} Request-Example:
          *      
          *        {
          *             "userName": "roshni@promactinfo.com"
          *        }      
          * @apiSuccessExample {json} Success-Response:
          * HTTP/1.1 200 OK 
          * {
          *     "description":"Object of type UserAc "
          *     
          *     {
          *         "Id": "95151b57-42c5-48d5-84b6-6d20e2fb05cd",
          *         "FirstName": "Admin",
          *         "LastName": "Promact",
          *         "IsActive": true,
          *         "Role": "Admin",
          *         "NumberOfCasualLeave": 0,
          *         "NumberOfSickLeave": 0,
          *         "JoiningDate": "0001-01-01T00:00:00",
          *         "SlackUserName": "roshni",
          *         "SlackUserId": "U70787887",
          *         "Email": "roshni@promactinfo.com",
          *         "UserName": "roshni@promactinfo.com",
          *         "UniqueName": "Admin-roshni@promactinfo.com",
          *     }
          * }
          */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("{userName}/details")]
        public async Task<IActionResult> GetByUserNameAsync(string userName)
        {
            try
            {
                return Ok(await _userRepository.GetUserDetailByUserNameAsync(userName));
            }
            catch (UserNotFound)
            {
                _logger.LogInformation("User not Found");
                return NotFound();
            }

        }

        /**
          * @api {get} api/User/ReSendMail/:id 
          * @apiVersion 1.0.0
          * @apiName ReSendMail
          * @apiParam {string} id
          * @apiParamExample {json} Request-Example:
          *      
          *        {
          *             "id": "adssdvvsdv55gdfgdsgbc"
          *        }      
          * @apiSuccessExample {json} Success-Response:
          * HTTP/1.1 200 OK 
          * {
          *     "description":"true"
          * }
          */
        [HttpGet]
        [Route("reSendMail/{id}")]
        public async Task<IActionResult> ReSendMail(string id)
        {
            try
            {
                _logger.LogInformation("Resend Mail");
                return Ok(await _userRepository.ReSendMail(id));
            }
            catch (InvalidApiRequestException apiEx)
            {
                _logger.LogError("Forgot Password mail not send " + apiEx.Message + apiEx.ToString());
                if (apiEx.Errors.Length > 0)
                {
                    foreach (var error in apiEx.Errors)
                        _logger.LogInformation("Forgot Password mail not send " + error);
                }
                throw apiEx;
            }
            catch (ArgumentNullException argEx)
            {
                _logger.LogError("Resend Mail unsuccessful " + argEx.Message + argEx.ToString());
                throw argEx;
            }
            catch (Exception ex)
            {
                _logger.LogError("Resend Mail unsuccessful " + ex.Message + ex.ToString());
                throw ex;
            }
        }


        /**
       * @api {get} api/user/:userid/role 
       * @apiVersion 1.0.0
       * @apiName GetUserRoleAsync
       * @apiGroup User
       * @apiParam {string} name UserName
       * @apiParamExample {json} Request-Example:
       * {
            "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367"    
       * }      
       * @apiSuccessExample {json} Success-Response:
       * HTTP/1.1 200 OK 
       * [
       *        {
       *            "UserName": "smith@promactinfo.com",
       *            "Name":"Smith",
       *            "Role":"Admin"
       *        }
       *]
       * @apiError UserRoleNotFound The role of the user not found.
       * @apiErrorExample {json} Error-Response:
       * HTTP/1.1 404 Not Found
       * {
       *  "error": "UserRoleNotFound"
       * }
       */

        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("{userid}/role")]
        public async Task<IActionResult> GetUserRoleAsync(string userId)
        {
            try
            {

                return Ok(await _userRepository.GetUserRoleAsync(userId));
            }
            catch (UserRoleNotFound)
            {
                return NotFound();
            }
        }
          

        /**
        * @api {get} api/user/:uerid/teammeber 
        * @apiVersion 1.0.0
        * @apiName GetTeamMembersAsync
        * @apiGroup User
        * @apiParam {string} name UserName
        * @apiParamExample {json} Request-Example:
        * {
        *   "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367"    
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
       *        },
       *    ]
       * @apiError UserRoleNotFound The role of the user not found.
       * @apiErrorExample {json} Error-Response:
       * HTTP/1.1 404 Not Found
       * {
       *  "error": "UserRoleNotFound"
       * }
       */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("{uerid}/teammebers")]
        public async Task<IActionResult> GetTeamMembersAsync(string userid)
        {
            try
            {
                return Ok(await _userRepository.GetTeamMembersAsync(userid));
            }
            catch (UserRoleNotFound)
            {
                return NotFound();
            }

        }

        /**
        * @api {get} api/user/slackChannel/:name 
        * @apiVersion 1.0.0
        * @apiName GetProjectUserByGroupName
        * @apiGroup User
        * @apiParam {string} groupName as a SlackChannelName
        * @apiParamExample {json} Request-Example:
        * {
        *   "groupName":"SlackChannelName",
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
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("slackChannel/{name}")]
        public async Task<IActionResult> GetProjectUserByGroupNameAsync(string name)
        {
            try
            {
                return Ok(await _userRepository.GetProjectUserByGroupNameAsync(name));
            }
            catch (UserNotFound)
            {
                return NotFound();
            }
        }
        /**
        * @api {get} api/User/{teamLeaderId}/project
        * @apiVersion 1.0.0
        * @apiName GetProject
        * @apiGroup User
        * @apiParam {string}  teamLeaderId
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             "id": "95151b57-42c5-48d5-84b6-6d20e2fb05cd"
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
        *           "NumberOfCasualLeave": 0,
        *           "NumberOfSickLeave": 0,
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
        *           "NumberOfCasualLeave": 0,
        *           "NumberOfSickLeave": 0,
        *           "JoiningDate": "2016-07-20T18:30:00",
        *           "SlackUserName": "gourav",
        *           "Email": "gourav@promactinfo.com",
        *           "UserName": "gourav@promactinfo.com",
        *           "UniqueName": "gourav-gourav@promactinfo.com",
        *        }
        *      ]
        * }
        */
        [ServiceFilter(typeof(CustomAttribute))]
        [HttpGet]
        [Route("{teamLeaderId}/project")]
        public async Task<IActionResult> GetProjectUsersByTeamLeaderIdAsync(string teamLeaderId)
        {
            if (teamLeaderId != null)
            {
                List<UserAc> projectUsers = await _userRepository.GetProjectUsersByTeamLeaderIdAsync(teamLeaderId);
                return Ok(projectUsers);
            }
            else
            {
                _logger.LogInformation("Teamleader Id does not exist ");
                return BadRequest();
            }
        }
        #endregion
    }
}
