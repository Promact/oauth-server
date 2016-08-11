using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace Promact.Oauth.Server.Controllers
{
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

        #region public Methods

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
                var newApps = Mapper.Map<ConsumerAppsAc, ConsumerApps>(consumerAppsAc);
                var createdBy = _userManager.GetUserId(User);
                newApps.CreatedBy = createdBy;
                if (_consumerAppRepository.AddedConsumerApps(newApps) != 0)
                    return Ok(true);
                else
                    return Ok(false);
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
                List<ConsumerApps> listOfApps = _consumerAppRepository.GetListOfApps();
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
                var consumerApps = _consumerAppRepository.GetAppsObjectById(id);
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
                var oldAppsDetails = _consumerAppRepository.GetAppsObjectById(consumerAppsAc.Id);
                if (oldAppsDetails != null)
                {
                    oldAppsDetails.Name = consumerAppsAc.Name;
                    oldAppsDetails.CallbackUrl = consumerAppsAc.CallbackUrl;
                    oldAppsDetails.Description = consumerAppsAc.Description;
                    oldAppsDetails.UpdatedDateTime = DateTime.Now;
                    if (_consumerAppRepository.UpdateConsumerApps(oldAppsDetails) != 0)
                        return Ok(true);
                    else
                        return Ok(false);
                }
                return Ok(oldAppsDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}