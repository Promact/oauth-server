using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    public class ProjectUserController:BaseController
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
        [Route("userDetails/{userFirstname}")]
        public IActionResult UserDetialByFirstName(string userFirstName)
        {
            var user = _userRepository.UserDetialByFirstName(userFirstName);
            return Ok(user);
        }

        /// <summary>
        /// Method is used to get list of teamLeader for an employee first name
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("teamLeaderDetails/{userFirstName}")]
        public IActionResult TeamLeaderByUserId(string userFirstName)
        {
            var user = _userRepository.TeamLeaderByUserId(userFirstName);
            return Ok(user);
        }

        /// <summary>
        /// Method is used to get list of management people
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("managementDetails")]
        public async Task<IActionResult> ManagementByUserId(string userId)
        {
            var user = await _userRepository.ManagementByUserId();
            return Ok(user);
        }
    }
}
