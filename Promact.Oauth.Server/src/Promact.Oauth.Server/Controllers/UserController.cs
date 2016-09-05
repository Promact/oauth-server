using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System.Threading.Tasks;

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
        public async Task<IActionResult> AllUsers()
        {
            return Ok(await _userRepository.GetAllUsers());
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
            var user =await _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /**
         * @api {get} api/User/RegisterUser 
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
        public IActionResult RegisterUser([FromBody] UserAc newUser)
        {
            string createdBy = _userManager.GetUserId(User);
            try
            {
                if (ModelState.IsValid)
                {
                    _userRepository.AddUser(newUser, createdBy);
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /**
     * @api {get} api/User/editProject 
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
        public IActionResult UpdateUser([FromBody] UserAc editedUser)
        {
            string updatedBy = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                string id=_userRepository.UpdateUserDetails(editedUser, updatedBy);
                if (id != "")
                { return Ok(true); }
                else { return Ok(false); }
            }
            return Ok(false);
        }



        /**
        * @api {get} api/User/ChangePasswordViewModel 
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
        public IActionResult ChangePassword([FromBody] ChangePasswordViewModel passwordModel)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.CheckPasswordAsync(user, passwordModel.OldPassword).Result)
            {
                passwordModel.Email = user.Email;
                if (ModelState.IsValid)
                {
                    _userRepository.ChangePassword(passwordModel);
                    return Ok(true);
                    
                }
            }
            return Ok(false);
            
        }


        /// <summary>
        /// This method is used to check if a user already exists in the database with the given userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("findbyusername/{userName}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FindByUserName(string userName)
        {
            return Ok(_userRepository.FindByUserName(userName));
        }


       
        [HttpGet]
        [Route("fetchbyusername/{userName}")]
        public IActionResult FetchByUserName(string userName)
        {
            return Ok(_userRepository.GetUserDetail(userName));
        }
        /**
       * @api {get} api/User/ChangePasswordViewModel 
       * @apiVersion 1.0.0
       * @apiName User
       * @apiGroup User
       * @apiParam {string} email  User email
      * @apiParamExample {json} Request-Example:
       *      
       *        {
       *             "email":"email@email.com",
       *        }      
       * @apiSuccessExample {json} Success-Response:
       * HTTP/1.1 200 OK 
       * {
       *     "description":"check if a user already exists in the database with the given email"
       * }
       */
       
        [HttpGet]
        [Route("findbyemail/{email}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FindByEmail(string email)
        {
            return Ok(_userRepository.FindByEmail(email));
        }


       
        #endregion
    }
}
