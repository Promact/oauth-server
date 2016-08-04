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
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        // Gets the list of all entries of the ApplicationUser Table
        // Get api/user
        [HttpGet]
        [Route("users")]
        public IActionResult AllUsers()
        {
            return Ok(_userRepository.GetAllUsers());
        }


        // Gets the details of a particular employee from ApplicationUser Table, by its id
        // Get api/user/3
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



        // Adds a new user to the ApplicationUser Table
        // Post api/user/add
        [HttpPost]
        [Route("add")]
        public IActionResult RegisterUser([FromBody] UserAc newUser)
        {
            try
            {
                if (ModelState.IsValid && newUser.Email.Contains("@promactinfo.com"))
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


        // Edits the details of an user to the ApplicationUser Table
        // Put api/user/edit/3
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

    }
}
