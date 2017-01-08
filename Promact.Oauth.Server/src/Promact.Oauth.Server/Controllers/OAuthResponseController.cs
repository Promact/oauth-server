using Exceptionless;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Services;
using System;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    [ServiceFilter(typeof(CustomAttribute))]
    [Route("api/[controller]")]
    public class OAuthResponseController : BaseController
    {
        #region Private Variable
        private readonly IUserRepository _userRepository;
        #endregion

        #region Constructor
        public OAuthResponseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Private Methods
        /**
        * @api {get} api/OAuthResponse/user/{slackUserId}
        * @apiVersion 1.0.0
        * @apiName UserDetialBySlackUserIdAsync
        * @apiGroup OAuthResponse
        * @apiParam {string} Name  slackUserId
        * @apiParamExample {json} Request-Example:
        *        {
        *             slackUserId : ADF4HY54H
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        *   {
        *       "Id":"34d1af3d-062f-4bcd-b6f9-b8fd5165e367",
        *       "FirstName" : "ABC",
        *       "Email" : "abc@promactinfo.com",
        *       "LastName" : "DEF",
        *       "SlackUserId" : "ADF4HY54H",
        *   }
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 400 Bad Request 
        * {
        *     "error" : "SlackUserNotFound"
        * }
        */
        [HttpGet]
        [Route("user/{slackUserId}")]
        public async Task<IActionResult> UserDetialBySlackUserIdAsync(string slackUserId)
        {
            try
            {
                return Ok(await _userRepository.UserDetialByUserSlackIdAsync(slackUserId));
            }
            catch (SlackUserNotFound ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest(ex.StackTrace);
            }
        }

        /**
        * @api {get} api/OAuthResponse/teamLeaders/{slackUserId}
        * @apiVersion 1.0.0
        * @apiName TeamLeaderByUsersSlackIdAsync
        * @apiGroup OAuthResponse
        * @apiParam {string} Name  slackUserId
        * @apiParamExample {json} Request-Example:
        *        {
        *             slackUserId : ADF4HY54H
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        *   {
        *       [
        *           {
        *               "Email" : "abc@promactinfo.com",
        *               "UserName" : "abc@promactinfo.com",
        *               "SlackUserId" : "ADF4HY54H",
        *           }
        *       ]
        *   }
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 400 Bad Request 
        * {
        *     "error" : "SlackUserNotFound"
        * }
        */
        [HttpGet]
        [Route("teamLeaders/{slackUserId}")]
        public async Task<IActionResult> TeamLeaderByUsersSlackIdAsync(string slackUserId)
        {
            try
            {
                return Ok(await _userRepository.TeamLeaderByUserSlackIdAsync(slackUserId));
            }
            catch (SlackUserNotFound ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest(ex.StackTrace);
            }
        }

        /**
        * @api {get} api/OAuthResponse/managements
        * @apiVersion 1.0.0
        * @apiName ManagementDetailsAsync
        * @apiGroup OAuthResponse    
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        *   {
        *       [
        *           {
        *               "Email" : "abc@promactinfo.com",
        *               "UserName" : "abc@promactinfo.com",
        *               "SlackUserId" : "ADF4HY54H",
        *           }
        *       ]
        *   }
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 400 Bad Request 
        * {
        *     "error" : "FailedToFetchDataException"
        * }
        */
        [HttpGet]
        [Route("managements")]
        public async Task<IActionResult> ManagementDetailsAsync()
        {
            try
            {
                return Ok(await _userRepository.ManagementDetailsAsync());
            }
            catch (FailedToFetchDataException ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest(ex.StackTrace);
            }
        }

        /**
        * @api {get} api/OAuthResponse/leaveAllowed/{slackUserId}
        * @apiVersion 1.0.0
        * @apiName GetUserCasualLeaveBySlackIdAsync
        * @apiGroup OAuthResponse    
        * @apiParam {string} Name  slackUserId
        * @apiParamExample {json} Request-Example:
        *        {
        *             slackUserId : ADF4HY54H
        *        } 
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        *   {
        *       "CasualLeave" : "14",
        *       "SickLeave" : "7"
        *   }
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 400 Bad Request 
        * {
        *     "error" : "SlackUserNotFound"
        * }
        */
        [HttpGet]
        [Route("leaveAllowed/{slackUserId}")]
        public async Task<IActionResult> GetUserCasualLeaveBySlackIdAsync(string slackUserId)
        {
            try
            {
                return Ok(await _userRepository.GetUserAllowedLeaveBySlackIdAsync(slackUserId));
            }
            catch (SlackUserNotFound ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest(ex.StackTrace);
            }
        }

        /**
        * @api {get} api/OAuthResponse/userIsAdmin/{slackUserId}
        * @apiVersion 1.0.0
        * @apiName UserIsAdminAsync
        * @apiGroup OAuthResponse    
        * @apiParam {string} Name  slackUserId
        * @apiParamExample {json} Request-Example:
        *        {
        *             slackUserId : ADF4HY54H
        *        } 
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        *   {
        *       "true"
        *   }
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 400 Bad Request 
        * {
        *     "error" : "SlackUserNotFound"
        * }
        */
        [HttpGet]
        [Route("userIsAdmin/{slackUserId}")]
        public async Task<IActionResult> UserIsAdminAsync(string slackUserId)
        {
            try
            {
                return Ok(await _userRepository.IsAdminAsync(slackUserId));
            }
            catch(SlackUserNotFound ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest(ex.StackTrace);
            }
        }
        #endregion
    }
}
