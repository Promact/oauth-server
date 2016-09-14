﻿using Microsoft.AspNetCore.Mvc;
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
        /**
        * @api {get} api/User/AllUsers 
        * @apiVersion 1.0.0
        * @apiName User
        * @apiGroup User
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Get List of Users"
        * }
        */
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


          /**
          * @api {get} api/User/GetUserById/:id 
          * @apiVersion 1.0.0
          * @apiName User
          * @apiGroup User
          * @apiParam {int} id User Id
          * @apiParamExample {json} Request-Example:
          *      
          *        {
          *             "id": "1"
          *             "description":"get the UserAc Object"
          *        }      
          * @apiSuccessExample {json} Success-Response:
          * HTTP/1.1 200 OK 
          * {
          *     "id":"1"
          *     "description":"get the UserAc Object"
          * }
          */

        
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

        /**
         * @api {post} api/User/RegisterUser 
         * @apiVersion 1.0.0
         * @apiName User
         * @apiGroup User
         * @apiParamExample {json} Request-Example:
         *      
         *        {
         *             "FirstName":"ProjectName",
         *             "LastName":"SlackChannelName",
         *             "IsActive":"True",
         *             "JoiningDate":"01-01-2016"
         *             "SlackUserName":"SlackUserName"
         *        }      
         * @apiSuccessExample {json} Success-Response:
         * HTTP/1.1 200 OK 
         * {
         *     "description":"Add User in Application User Table"
         * }
         */

        
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


        /**
     * @api {put} api/User/editProject 
     * @apiVersion 1.0.0
     * @apiName User
     * @apiGroup User
     * @apiParam {int} id  User Id
     * @apiParamExample {json} Request-Example:
     *      
     *        {
     *             "Id":"1",
     *             "FirstName":"ProjectName",
     *             "LastName":"SlackChannelName",
     *             "IsActive":"True",
     *             "JoiningDate":"01-01-2016"
     *             "SlackUserName":"SlackUserName"
     *        }      
     * @apiSuccessExample {json} Success-Response:
     * HTTP/1.1 200 OK 
     * {
     *     "description":"edit User in User Table"
     * }
     */
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



        /**
        * @api {post} api/User/ChangePasswordViewModel 
        * @apiVersion 1.0.0
        * @apiName User
        * @apiGroup User
        * @apiParam {string} OldPassword  User OldPassword
        * @apiParam {string} NewPassword  User NewPassword
        * @apiParam {string} ConfirmPassword  User ConfirmPassword
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             "OldPassword":"OldPassword",
        *             "NewPassword":"NewPassword",
        *             "ConfirmPassword":"ConfirmPassword"
        *            
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"Change Password"
        * }
        */
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
