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
        
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        /* Gets the list of all entries of the ApplicationUser Table */
        [HttpGet]
        [Route("users")]
        public IActionResult AllUsers()
        {
            return Ok(_userRepository.GetAllUsers());
        }


        /* Gets the details of a particular employee from ApplicationUser Table, by its id 
           parameter: integer Id*/
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



        /* Adds a new user to the ApplicationUser Table
         Parameter: UserAc Model */
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
            catch (Exception)
            {
                return BadRequest();
            }
        }


        /* Edits the details of an user to the ApplicationUser Table
         Parameter: UserAc Model */
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
            if(ModelState.IsValid)
            {
                _userRepository.ChangePassword(passwordModel);
                return Ok();
            }
            
            return BadRequest();
        }


        /* Finds if a user name exists in the database
          Parameter: string username*/
        [HttpGet]
        [Route("findbyusername/{userName}")]
        public IActionResult FindByUserName(string userName)
        {
            return Ok(_userRepository.FindByUserName(userName));
        }


        /* Finds if an email exists in the database
          Parameter: string email*/
        [HttpGet]
        [Route("findbyemail/{email}")]
        public IActionResult FindByEmail(string email)
        {
            return Ok(_userRepository.FindByEmail(email));
        }
    }
}
