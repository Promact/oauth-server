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
        * @api {post} api/consumerApp/AddConsumerApp 
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
        *     "description":"retun true when added succesfully.If any problem in addconsumer mehtod or not added consumer apps so return false."
        * }
        */
        [HttpPost]
        [Route("addConsumer")]
        public async Task<IActionResult> AddConsumerApp([FromBody]ConsumerAppsAc consumerAppsAc)
        {
            try
            {
                consumerAppsAc.CreatedBy = _userManager.GetUserId(User);
                if (await _consumerAppRepository.AddConsumerApps(consumerAppsAc) != 0)
                    return Ok(true);
                else
                    return Ok(false);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }


        /**
        * @api {get} api/consumerApp/GetConsumerApps 
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
        [Route("getConsumerApps")]
        public async Task<IActionResult> GetConsumerApps()
        {
            try
            {
                List<ConsumerApps> listOfApps = await _consumerAppRepository.GetListOfApps();
                return Ok(listOfApps);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }


        /**
       * @api {get} api/consumerApp/GetConsumerById 
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
        [Route("getConsumerById/{id}")]
        public async Task<IActionResult> GetConsumerById(int id)
        {
            try
            {
                var consumerApps = await _consumerAppRepository.GetConsumerAppsById(id);
                return Ok(consumerApps);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                throw ex;
            }
        }

        /**
        * @api {post} api/consumerApp/UpdateConsumerApps 
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
        *   "description":"retun true if succesfully consumer updated If any problem in updte consumer mehtod or not update consumer apps so return false."
        * }
        */
        [HttpPost]
        [Route("updateConsumer")]
        public async Task<IActionResult> UpdateConsumerApps([FromBody]ConsumerAppsAc consumerAppsAc)
        {
            try
            {
                var oldAppsDetails = await _consumerAppRepository.GetConsumerAppsById(consumerAppsAc.Id);
                if (oldAppsDetails != null)
                {
                    oldAppsDetails.Name = consumerAppsAc.Name;
                    oldAppsDetails.CallbackUrl = consumerAppsAc.CallbackUrl;
                    oldAppsDetails.Description = consumerAppsAc.Description;
                    oldAppsDetails.UpdatedDateTime = DateTime.Now;
                    if (await _consumerAppRepository.UpdateConsumerApps(oldAppsDetails) != 0)
                        return Ok(true);
                    else
                        return Ok(false);
                }
                return Ok(oldAppsDetails);
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