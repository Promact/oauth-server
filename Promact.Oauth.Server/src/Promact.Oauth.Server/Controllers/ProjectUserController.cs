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
        public IActionResult UserDetialByFirstName(string userFirstname)
        {
            var user = _userRepository.UserDetialByFirstName(userFirstname);
            return Ok(user);
        }

        /// <summary>
        /// Method is used to get list of teamLeader for an employee
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("teamLeaderDetails/{userId}")]
        public IActionResult TeamLeaderByUserId(string userId)
        {
            var user = _userRepository.TeamLeaderByUserId(userId);
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
