using Exceptionless;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Services;
using System;
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
        public async Task<IActionResult> TeamLeaderByUserId(string slackUserId)
        {
            try
            {
                var user = await _userRepository.TeamLeaderByUserSlackId(slackUserId);
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
        public async Task<IActionResult> UserIsAdmin(string slackUserId)
        {
            var result = await _userRepository.IsAdmin(slackUserId);
            return Ok(result);
        }
    }
}
