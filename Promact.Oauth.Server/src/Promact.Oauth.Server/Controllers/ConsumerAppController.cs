using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Microsoft.AspNetCore.Identity;

namespace Promact.Oauth.Server.Controllers
{
    [Route("api/[controller]")]
    public class ConsumerAppController : Controller
    {
        private readonly IConsumerAppReposiotry _iConsumerAppRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConsumerAppController(IConsumerAppReposiotry iConsumerAppRepository, UserManager<ApplicationUser> userManager)
        {
            _iConsumerAppRepository = iConsumerAppRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// This method used for add new consumer app. -An
        /// </summary>
        /// <param name="consumerAppsAc"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addConsumer")]
        public IActionResult AddConsumerApp([FromBody]ConsumerAppsAc consumerAppsAc)
        {
            try
            {
                Apps newApps = new Apps();
                newApps.CallbackUrl = consumerAppsAc.CallbackUrl;
                newApps.CreatedDateTime = DateTime.Now;
                newApps.Description = consumerAppsAc.Description;
                newApps.Name = consumerAppsAc.Name;
                var createdBy = _userManager.GetUserId(User);
                newApps.CreatedBy = createdBy;
                _iConsumerAppRepository.AddedConsumerApps(newApps);
                return Ok(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// This method used for get cinsumer apps list. -An
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getConsumerApps")]
        public IActionResult GetConsumerApps()
        {
            try
            {
                List<Apps> listOfApps = _iConsumerAppRepository.GetListOfApps();
                return Ok(listOfApps);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// This method used for get consumer app object by id. -An
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getConsumerById/{id}")]
        public IActionResult GetConsumerById(int id)
        {
            try
            {
                var consumerApps = _iConsumerAppRepository.GetAppsObjectById(id);
                return Ok(consumerApps);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method used for update existing consumer app. -An
        /// </summary>
        /// <param name="consumerAppsAc"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateConsumer")]
        public IActionResult UpdateConsumerApps([FromBody]ConsumerAppsAc consumerAppsAc)
        {
            try
            {
                var oldAppsDetails = _iConsumerAppRepository.GetAppsObjectById(consumerAppsAc.Id);
                if (oldAppsDetails != null)
                {
                    oldAppsDetails.Name = consumerAppsAc.Name;
                    oldAppsDetails.CallbackUrl = consumerAppsAc.CallbackUrl;
                    oldAppsDetails.Description = consumerAppsAc.Description;
                    oldAppsDetails.UpdatedDateTime = DateTime.Now;
                    _iConsumerAppRepository.UpdateConsumerApps(oldAppsDetails);
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}