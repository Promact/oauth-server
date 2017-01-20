using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Services;
using System;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    [Authorize(Policy = ReadUser)]
    [Route("api/[controller]")]
    public class ProjectUserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        public const string ReadUser = "ReadUser";
        public ProjectUserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Method to get User details by slack user Id
        /// </summary>
        /// <param name="userFirstname"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("userDetails/{slackUserId}")]
        public IActionResult UserDetialBySlackUserId(string slackUserId)
        {
            try
            {
                var user = _userRepository.UserDetialByUserSlackId(slackUserId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        /// <summary>
        /// Method is used to get list of teamLeader for an employee slack user Id
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("teamLeaderDetails/{slackUserId}")]
        public async Task<IActionResult> TeamLeaderByUserIdAsync(string slackUserId)
        {
            try
            {
                var user = await _userRepository.TeamLeaderByUserSlackIdAsync(slackUserId);
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
        public async Task<IActionResult> ManagementDetailsAsync()
        {
            try
            {
                var user = await _userRepository.ManagementDetailsAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }


        /// <summary>
        /// Method to get the number of casual leave allowed to a user by slack user Id
        /// </summary>
        /// <param name="slackUserId"></param>
        /// <returns>number of casual leave</returns>
        [HttpGet]
        [Route("casual/leave/{slackUserId}")]
        public IActionResult GetUserCasualLeaveBySlackId(string slackUserId)
        {
            try
            {
                var casualLeave = _userRepository.GetUserAllowedLeaveBySlackId(slackUserId);
                return Ok(casualLeave);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        [HttpGet]
        [Route("userIsAdmin/{slackUserId}")]
        public async Task<IActionResult> UserIsAdminAsync(string slackUserId)
        {
            var result = await _userRepository.IsAdminAsync(slackUserId);
            return Ok(result);
        }
    }
}
