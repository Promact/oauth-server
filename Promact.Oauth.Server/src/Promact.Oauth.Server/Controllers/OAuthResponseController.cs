using Exceptionless;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.ExceptionHandler;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Services;
using System;
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

        /// <summary>
        /// Method to get User details by slack user Id
        /// </summary>
        /// <param name="userFirstname"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/{slackUserId}")]
        public IActionResult FetchUserDetialBySlackUserId(string slackUserId)
        {
            ApplicationUser user = _userRepository.UserDetialByUserSlackId(slackUserId);
            return Ok(user);
        }


        /// <summary>
        /// Method is used to get list of teamLeader for an employee slack user Id
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("teamLeaders/{slackUserId}")]
        public async Task<IActionResult> GetTeamLeaderByUserId(string slackUserId)
        {
            List<ApplicationUser> userList = await _userRepository.TeamLeaderByUserSlackId(slackUserId);
            return Ok(userList);
        }


        /// <summary>
        /// Method is used to get list of management people
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("management")]
        public async Task<IActionResult> GetManagementDetails()
        {
            List<ApplicationUser> userList = await _userRepository.ManagementDetails();
            return Ok(userList);
        }


        /// <summary>
        /// Method to get the number of casual leave allowed to a user by slack user Id
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>number of casual leave</returns>
        [HttpGet]
        [Route("leave/{slackUserId}")]
        public IActionResult GetUserLeaveBySlackId(string slackUserId)
        {
            LeaveAllowed leaveAllowed = _userRepository.GetUserAllowedLeaveBySlackId(slackUserId);
            return Ok(leaveAllowed);
        }


        [HttpGet]
        [Route("userIsAdmin/{slackUserId}")]
        public async Task<IActionResult> UserIsAdmin(string slackUserId)
        {
            try
            {
                bool result = await _userRepository.IsAdmin(slackUserId);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

    }
}
