using Exceptionless;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Repository.OAuthRepository;
using Promact.Oauth.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    [ServiceFilter(typeof(CustomAttribute))]
    [Route("api/[controller]")]
    public class ProjectUserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        public ProjectUserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Method to get User details by user first name
        /// </summary>
        /// <param name="userFirstname"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("userDetails/{userSlackName}")]
        public IActionResult UserDetialByFirstName(string userSlackName)
        {
            try
            {
                var user = _userRepository.UserDetialByUserSlackName(userSlackName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        /// <summary>
        /// Method is used to get list of teamLeader for an employee first name
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("teamLeaderDetails/{userSlackName}")]
        public async Task<IActionResult> TeamLeaderByUserId(string userSlackName)
        {
            try
            {
                var user = await _userRepository.TeamLeaderByUserSlackName(userSlackName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        /// <summary>
        /// Method is used to get list of management people
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("managementDetails")]
        public async Task<IActionResult> ManagementDetails()
        {
            try
            {
                var user = await _userRepository.ManagementDetails();
                return Ok(user);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }


        /// <summary>
        /// Method to get the number of casual leave allowed to a user by slack user name
        /// </summary>
        /// <param name="slackUserName"></param>
        /// <returns>number of casual leave</returns>
        [HttpGet]
        [Route("casual/leave/{slackUserName}")]
        public IActionResult GetUserCasualLeaveBySlackName(string slackUserName)
        {
            try
            {
                var casualLeave = _userRepository.GetUserAllowedLeaveBySlackName(slackUserName);
                return Ok(casualLeave);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        [HttpGet]
        [Route("userIsAdmin/{userName}")]
        public async Task<IActionResult> UserIsAdmin(string userName)
        {
            var result = await _userRepository.IsAdmin(userName);
            return Ok(result);
        }
    }
}
