using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using Exceptionless;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    // [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        #region "Private Variable(s)"

        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region "Constructor"

        public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        #endregion

        #region public Methods

        /// <summary>
        /// This method is used for getting the list of all users
        /// </summary>
        /// <returns>User list</returns>
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
                ex.ToExceptionless().Submit();
                throw;
            }
        }

        [HttpGet]
        [Route("getRole")]
        public IActionResult GetRole()
        {
            List<RolesAc> listofRoles = _userRepository.GetRoles();
            return Ok(listofRoles);
        }


        /// <summary>
        /// This method is used to get particular user's details by his/her id
        /// </summary>
        /// <param name="id">String id</param>
        /// <returns>UserAc Application class user</returns>
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
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }



        /// <summary>
        /// This method is used to add new user
        /// </summary>
        /// <param name="newUser">UserAc application class user</param>
        /// <returns></returns>
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
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }


        /// <summary>
        /// This method is used to edit the details of an existing user
        /// </summary>
        /// <param name="editedUser">UserAc application class user</param>
        /// <returns></returns>
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
                    else { return Ok(false); }
                }
                return Ok(false);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }



        /// <summary>
        /// This method is used to change the password of a particular user
        /// </summary>
        /// <param name="passwordModel">ChangePassWordModel object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("changepassword")]
        [AllowAnonymous]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel passwordModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (await _userManager.CheckPasswordAsync(user, passwordModel.OldPassword))
                {
                    passwordModel.Email = user.Email;
                    if (ModelState.IsValid)
                    {
                        await _userRepository.ChangePassword(passwordModel);
                        return Ok(true);
                    }
                }
                return Ok(false);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }


        /// <summary>
        /// This method is used to check if a user already exists in the database with the given userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
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
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }


        /// <summary>
        /// This method is used to check if a user already exists in the database with the given userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
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
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to check if a user already exists in the database with the given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("findbyemail/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindByEmail(string email)
        {
            try
            {
                return Ok(await _userRepository.FindByEmail(email));
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }


        [HttpGet]
        [Route("findUserBySlackUserName/{slackUserName}")]
        public IActionResult FindUserBySlackUserName(string slackUserName)
        {
            try
            {
                return Ok(_userRepository.FindUserBySlackUserName(slackUserName));
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }        
        #endregion
    }
}
