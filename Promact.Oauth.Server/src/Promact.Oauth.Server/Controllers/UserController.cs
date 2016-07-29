using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ManageViewModels;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }


        /// <summary>
        /// returns the list of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AllUsers()
        {
            return View(userRepository.GetAllUsers());
        }

        /// <summary>
        /// Register Users to the database
        /// </summary>
        /// <param name="user">ApplicationUser Object</param>
        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public IActionResult RegisterUser(ApplicationUser user)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        userRepository.AddUser(user);
        //    }

        //    return View(user);
        //}

        /// <summary>
        /// Edits the details of an employee
        /// </summary>
        /// <param name="editedUser"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUser(ApplicationUser editedUser)
        {
            if(ModelState.IsValid)
            {
                userRepository.UpdateUserDetails(editedUser);
                return View(editedUser);
            }
            return NotFound();
        }

    }
}
