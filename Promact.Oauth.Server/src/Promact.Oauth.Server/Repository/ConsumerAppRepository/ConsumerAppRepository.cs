using AutoMapper;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public class ConsumerAppRepository : IConsumerAppRepository
    {
        #region "Private Variable(s)"

        private readonly IDataRepository<ConsumerApps> _appsDataRepository;
        private readonly IMapper _mapperContext;

        #endregion


        #region "Constructor"
        public ConsumerAppRepository(IDataRepository<ConsumerApps> appsDataRepository, IMapper mapperContext)
        {
            _appsDataRepository = appsDataRepository;
            _mapperContext = mapperContext;
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// This method used for get apps detail by client id. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<ConsumerApps> GetAppDetails(string clientId)
        {
            try
            {
                return await _appsDataRepository.FirstOrDefaultAsync(x => x.AuthId == clientId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// This method used for added Consumer apps and return primary key. -An
        /// </summary>
        /// <param name="aapsObject"></param>
        /// <returns></returns>
        public async Task<int> AddConsumerApps(ConsumerAppsAc consumerApps)
        {
            try
            {
                if (_appsDataRepository.FirstOrDefault(x => x.Name == consumerApps.Name) == null)
                {
                    var consumerAppObject = _mapperContext.Map<ConsumerAppsAc, ConsumerApps>(consumerApps);
                    consumerAppObject.AuthId = CreatedRandomNumer(true);
                    consumerAppObject.AuthSecret = CreatedRandomNumer(false);
                    consumerAppObject.CreatedDateTime = DateTime.Now;
                     _appsDataRepository.AddAsync(consumerAppObject);
                    await _appsDataRepository.SaveChangesAsync();
                    return consumerAppObject.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// This method used forget list of apps. -An
        /// </summary>
        /// <returns></returns>
        public async Task<List<ConsumerApps>> GetListOfApps()
        {
            try
            {
                return await _appsDataRepository.GetAll().ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// This method used fro get apps object by id. -An
        /// </summary>
        /// <param name="id">pass apps object primarykey</param>
        /// <returns></returns>
        public async Task<ConsumerApps> GetConsumerAppsById(int id)
        {
            try
            {
                return await _appsDataRepository.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// This method used for update consumer apps and return primary key. -An
        /// </summary>
        /// <param name="apps"></param>
        /// <returns></returns>
        public async Task<int> UpdateConsumerApps(ConsumerApps consumerApps)
        {
            try
            {
                if (_appsDataRepository.FirstOrDefault(x => x.Name == consumerApps.Name && x.Id != consumerApps.Id) == null)
                {
                    _appsDataRepository.UpdateAsync(consumerApps);
                    await _appsDataRepository.SaveChangesAsync();
                    return consumerApps.Id;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region "Private Method(s)"

        /// <summary>
        /// This method used for Created Random Number For AuthId and Auth Secreate. -An
        /// </summary>
        /// <param name="isAuthId"></param>
        /// <returns></returns>
        private string CreatedRandomNumer(bool isAuthId)
        {
            try
            {
                var random = new Random();
                if (isAuthId)
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    return new string(Enumerable.Repeat(chars, 15)
                      .Select(s => s[random.Next(s.Length)]).ToArray());
                }
                else
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    return new string(Enumerable.Repeat(chars, 30)
                      .Select(s => s[random.Next(s.Length)]).ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
