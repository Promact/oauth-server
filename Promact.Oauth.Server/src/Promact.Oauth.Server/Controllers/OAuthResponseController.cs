using Exceptionless;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    [ServiceFilter(typeof(CustomAttribute))]
    [Route("api/[controller]")]
    public class OAuthResponseController : BaseController
    {
        private readonly IUserRepository _userRepository;
        public OAuthResponseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        /**
            * @api {get} api/OAuthResponse/user/slackUserId
            * @apiVersion 1.0.0
            * @apiName FetchUserDetialBySlackUserId
            * @apiGroup OAuthResponse
            * @apiParam {string} slackUserId
            * @apiParamExample {json} Request-Example:  
            *        {
            *            "slackUserId":"1jhdf87907"
            *        }     
            * @apiSuccessExample {json} Success-Response:
            * HTTP/1.1 200 OK 
            * {
            *  {
            *   "Id":"24387438",
            *   "Email":"abc@promactinfo.com",
            *   "FirstName ":"Abc",
            *   "LastName ":"Xyz",
            *   "SlackUserId ":"U76dfhgdfh"
            *   }
            * }
            * @apiError SlackUserNotFound The user with the given SlackUserId was not found.
            * @apiErrorExample {json} Error-Response:
            * HTTP/1.1 404 Not Found
            * {
            *   "error": "SlackUserNotFound"
            * }
        */
        [HttpGet]
        [Route("user/{slackUserId}")]
        public IActionResult FetchUserDetialBySlackUserId(string slackUserId)
        {
            try
            {
                ApplicationUser user = _userRepository.UserDetialByUserSlackId(slackUserId);
                return Ok(user);
            }
            catch (SlackUserNotFound ex)
            {
                ex.ToExceptionless().Submit();
                return NotFound();
            }
        }


        /**
        * @api {get} api/OAuthResponse/teamLeaders/slackUserId
        * @apiVersion 1.0.0
        * @apiName GetTeamLeadersByUserId
        * @apiGroup OAuthResponse
        * @apiParam {string} slackUserId
        * @apiParamExample {json} Request-Example:  
        *        {
        *            "slackUserId":"1jhdf87907"
        *        }     
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *   {
        *   "UserName ":"Abc",
        *   "Email":"abc@promactinfo.com",
        *   "SlackUserId ":"U76dfhgdfh"
        *   },
        *   {
        *   "UserName ":"Def",
        *   "Email":"def@promactinfo.com",
        *   "SlackUserId ":"U76dfg445h"
        *   },
        * }
        */
        [HttpGet]
        [Route("teamLeaders/{slackUserId}")]
        public async Task<IActionResult> GetTeamLeadersByUserId(string slackUserId)
        {
            List<ApplicationUser> userList = await _userRepository.TeamLeaderByUserSlackId(slackUserId);
            return Ok(userList);
        }


        /**
        * @api {get} api/OAuthResponse/management
        * @apiVersion 1.0.0
        * @apiName GetManagementDetails
        * @apiGroup OAuthResponse 
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *   {
        *   "FirstName ":"Abc",
        *   "Email":"abc@promactinfo.com",
        *   "SlackUserId ":"U76dfhgdfh"
        *   },
        *   {
        *   "FirstName ":"Def",
        *   "Email":"def@promactinfo.com",
        *   "SlackUserId ":"U76dfg445h"
        *   },
        * }
        */
        [HttpGet]
        [Route("management")]
        public async Task<IActionResult> GetManagementDetails()
        {
            List<ApplicationUser> userList = await _userRepository.ManagementDetails();
            return Ok(userList);
        }


        /**
            * @api {get} api/OAuthResponse/leave/slackUserId
            * @apiVersion 1.0.0
            * @apiName GetUserLeaveBySlackId
            * @apiGroup OAuthResponse
            * @apiParam {string} slackUserId
            * @apiParamExample {json} Request-Example:  
            *        {
            *            "slackUserId":"1jhdf87907"
            *        }     
            * @apiSuccessExample {json} Success-Response:
            * HTTP/1.1 200 OK 
            * {
            *  {
            *   "CasualLeave":"2",
            *   "SickLeave ":"7"        
            *   }
            * }
            * @apiError SlackUserNotFound The user with the given SlackUserId was not found.
            * @apiErrorExample {json} Error-Response:
            * HTTP/1.1 404 Not Found
            * {
            *   "error": "SlackUserNotFound"
            * }
        */
        [HttpGet]
        [Route("leave/{slackUserId}")]
        public IActionResult GetUserLeaveBySlackId(string slackUserId)
        {
            try
            {
                LeaveAllowed leaveAllowed = _userRepository.GetUserAllowedLeaveBySlackId(slackUserId);
                return Ok(leaveAllowed);
            }
            catch (SlackUserNotFound ex)
            {
                ex.ToExceptionless().Submit();
                return NotFound();
            }
        }


        /**
           * @api {get} api/OAuthResponse/slackUserId/isAdminUser
           * @apiVersion 1.0.0
           * @apiName UserIsAdmin
           * @apiGroup OAuthResponse
           * @apiParam {string} slackUserId
           * @apiParamExample {json} Request-Example:  
           *        {
           *            "slackUserId":"1jhdf87907"
           *        }     
           * @apiSuccessExample {json} Success-Response:
           * HTTP/1.1 200 OK 
           * {
           *    "true"
           * }
           * @apiError SlackUserNotFound The user with the given SlackUserId was not found.
           * @apiErrorExample {json} Error-Response:
           * HTTP/1.1 404 Not Found
           * {
           *   "error": "SlackUserNotFound"
           * }
       */
        [HttpGet]
        [Route("{slackUserId}/isAdminUser")]
        public async Task<IActionResult> UserIsAdmin(string slackUserId)
        {
            try
            {
                bool result = await _userRepository.IsAdmin(slackUserId);
                return Ok(result);
            }
            catch (SlackUserNotFound ex)
            {
                ex.ToExceptionless().Submit();
                return NotFound();
            }
        }

    }
}
