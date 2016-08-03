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
using Promact.Oauth.Server.Models.ApplicationClass;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(IUserRepository _userRepository, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            userRepository = _userRepository;
            userManager = _userManager;
            signInManager = _signInManager;
        }


        /// <summary>
        /// returns the list of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("users")]
        public IEnumerable<UserModel> AllUsers()
        {
            return userRepository.GetAllUsers();
        }


        /// <summary>
        /// Edits the details of an employee
        /// </summary>
        /// <param name="editedUser"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("edit")]
        public UserModel UpdateUser([FromBody] UserModel editedUser)
        {
            if(ModelState.IsValid)
            {
                userRepository.UpdateUserDetails(editedUser);
                return editedUser;
            }
            return null;
        }


        /// <summary>
        /// Register Users to the database
        /// </summary>
        /// <param name="user">ApplicationUser Object</param>
        [HttpPost]
        [Route("add")]
        public UserModel RegisterUser([FromBody] UserModel newUser)
        {
            userRepository.AddUser(newUser);
            return newUser;
        }


        /// <summary>
        /// Fetches the details of the user by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public UserModel GetUserById(string id)
        {
            return userRepository.GetById(id);
        }
    }
}
