using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Models.ApplicationClasses;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private UserManager<ApplicationUser> _userManager;

        public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }


        /* Calls the repository method for fetching the list of all entries of the ApplicationUser Table */
        [HttpGet]
        [Route("users")]
        public IActionResult AllUsers()
        {
            return Ok(_userRepository.GetAllUsers());
        }


        /* Calls the repository method for fetching the details of a particular employee from ApplicationUser Table, by its id 
           Parameter: integer Id*/
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUserById(string id)
        {
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }



        /* Calls the repository method for adding a new user to the ApplicationUser Table
         Parameter: Application class object UserAc */
        [HttpPost]
        [Route("add")]
        public IActionResult RegisterUser([FromBody] UserAc newUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userRepository.AddUser(newUser);
                    return Ok(newUser);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }


        /* Calls the repository method for updating the details of an user to the ApplicationUser Table
         Parameter: Application class object UserAc */
        [HttpPut]
        [Route("edit")]
        public IActionResult UpdateUser([FromBody] UserAc editedUser)
        {
            if (ModelState.IsValid)
            {
                _userRepository.UpdateUserDetails(editedUser);
                return Ok(editedUser);
            }
            return BadRequest();
        }



        /* Calls the repository method for changing the password
           Parameter: PasswordViewModel */
        [HttpPost]
        [Route("changepassword")]
        [AllowAnonymous]
        public IActionResult ChangePassword([FromBody] ChangePasswordViewModel passwordModel)
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (_userManager.CheckPasswordAsync(user, passwordModel.OldPassword).Result)
            {
                passwordModel.Email = user.Email;
                if (ModelState.IsValid)
                {
                    _userRepository.ChangePassword(passwordModel);
                    return Ok(passwordModel);
                }
            }
            return BadRequest();
        }


        /* Calls the repository method to find if a user already exists with the specified username, in the database
          Parameter: string username*/
        [HttpGet]
        [Route("findbyusername/{userName}")]
        public IActionResult FindByUserName(string userName)
        {
            return Ok(_userRepository.FindByUserName(userName));
        }


        /* Calls the repository method to find if a user already exists with the specified email, in the database
          Parameter: string email*/
        [HttpGet]
        [Route("findbyemail/{email}")]
        public IActionResult FindByEmail(string email)
        {
            return Ok(_userRepository.FindByEmail(email));
        }
    }
}
