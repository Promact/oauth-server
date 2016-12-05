using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Exceptionless;
using Promact.Oauth.Server.Exception_Handler;

namespace Promact.Oauth.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class ConsumerAppController : Controller
    {
        #region "Private Variable(s)"
        private readonly IConsumerAppRepository _consumerAppRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region "Constructor"
        public ConsumerAppController(IConsumerAppRepository iConsumerAppRepository, UserManager<ApplicationUser> userManager)
        {
            _consumerAppRepository = iConsumerAppRepository;
            _userManager = userManager;
        }

        #endregion

        #region Public Methods


        /**
        * @api {post} api/consumerapp 
        * @apiVersion 1.0.0
        * @apiName ConsumerApp
        * @apiGroup ConsumerApp
        * @apiParam {object} consumerAppsAc  object
        * @apiParamExample {json} Request-Example:
        *      
        *        {
        *             "Name":"ProjectName",
        *             "Description":"True",
        *             "CallbackUrl":"1",
        *        }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "description":"return true if succesfully consumer app has been added else return false."
        * }
        */
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddConsumerApp([FromBody]ConsumerAppsAc consumerAppsAc)
        {
            try
            {
                consumerAppsAc.CreatedBy = _userManager.GetUserId(User);
                return Ok(await _consumerAppRepository.AddConsumerAppsAsync(consumerAppsAc));
            }
            catch (ConsumerAppNameIsAlreadyExists)
            {
                return BadRequest();
            }
        }


        /**
        * @api {get} api/consumerapp 
        * @apiVersion 1.0.0
        * @apiName ConsumerApp
        * @apiGroup ConsumerApp
        * @apiParam {null} no parameter
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        * 
        *   "description":"retun consumer app list"
        * }
        */
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetConsumerApps()
        {
            List<ConsumerApps> listOfApps = await _consumerAppRepository.GetListOfConsumerAppsAsync();
            return Ok(listOfApps);
        }


        /**
       * @api {get} api/consumerapp/id 
       * @apiVersion 1.0.0
       * @apiName ConsumerApp
       * @apiGroup ConsumerApp
       * @apiParam {int} id 
       * @apiSuccessExample {json} Success-Response:
       * {
       *   "id":"1",
       * }
       * HTTP/1.1 200 OK 
       * {
       *   "description":"retun consumer app object"
       * }
       */
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetConsumerAppById(int id)
        {
            try
            {
                ConsumerApps consumerApps = await _consumerAppRepository.GetConsumerAppByIdAsync(id);
                return Ok(consumerApps);
            }
            catch (ConsumerAppNotFound)
            {
                return NotFound();
            }
        }

        /**
        * @api {put} api/consumerapp 
        * @apiVersion 1.0.0
        * @apiName ConsumerApp
        * @apiGroup ConsumerApp
        * @apiParam {object} consumerAppsAc object
        * @apiParamExample {json} Request-Example:  
          *        {
          *             "Name":"ProjectName",
          *             "Description":"True",
          *             "CallbackUrl":"1",
          *        } 
        * HTTP/1.1 200 OK 
        * {
        *
        *   "description":"return true if succesfully consumer app has been updated else return false."
        * }
        */
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateConsumerApp([FromBody]ConsumerAppsAc consumerAppsAc)
        {
            try
            {
                ConsumerApps consumerApp = await _consumerAppRepository.GetConsumerAppByIdAsync(consumerAppsAc.Id);
                consumerApp.Name = consumerAppsAc.Name;
                consumerApp.CallbackUrl = consumerAppsAc.CallbackUrl;
                consumerApp.Description = consumerAppsAc.Description;
                consumerApp.UpdatedDateTime = DateTime.Now;
                return Ok(await _consumerAppRepository.UpdateConsumerAppsAsync(consumerApp));
            }
            catch (ConsumerAppNameIsAlreadyExists)
            {
                return BadRequest();
            }
        }

        #endregion
    }
}