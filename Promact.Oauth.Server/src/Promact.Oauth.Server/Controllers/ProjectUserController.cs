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
        public async Task<IActionResult> TeamLeaderByUserId(string userFirstName)
        {
            var user = await _userRepository.TeamLeaderByUserId(userFirstName);
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

        ///// <summary>
        ///// Method to get User details by user Id
        ///// </summary>
        ///// <param name="employeeId"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("userDetail/{employeeId}")]
        //public IActionResult UserDetailById(string employeeId)
        //{
        //    var user = _userRepository.UserDetailById(employeeId);
        //    return Ok(user);
        //}
    }
}
