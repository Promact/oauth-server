using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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

        #region public Methods



        /// <summary>
        /// This method used for add new consumer app. -An
        /// </summary>
        /// <param name="consumerAppsAc"></param>
        /// <returns></returns>
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
                throw ex;
            }
        }



        /// <summary>
        /// This method used for get consumer app list. -An
        /// </summary>
        /// <returns></returns>
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
        public async Task<IActionResult> GetConsumerById(int id)
        {
            try
            {
                var consumerApps = await _consumerAppRepository.GetConsumerAppsById(id);
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
                throw ex;
            }
        }

        #endregion
    }
}