using AutoMapper;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.Constants;

namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public class ConsumerAppRepository : IConsumerAppRepository
    {
        #region "Private Variable(s)"

        private readonly IDataRepository<ConsumerApps> _appsDataRepository;
        private readonly IMapper _mapperContext;
        private readonly IStringConstant _stringConstant;
        #endregion

        #region "Constructor"
        public ConsumerAppRepository(IDataRepository<ConsumerApps> appsDataRepository, IMapper mapperContext, IStringConstant stringConstant)
        {
            _appsDataRepository = appsDataRepository;
            _mapperContext = mapperContext;
            _stringConstant = stringConstant;
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
            return await _appsDataRepository.FirstOrDefaultAsync(x => x.AuthId == clientId);
        }

        /// <summary>
        /// This method used for added consumer app and return primary key. -An
        /// </summary>
        /// <param name="aapsObject"></param>
        /// <returns></returns>
        public async Task<int> AddConsumerApps(ConsumerAppsAc consumerApps)
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


        /// <summary>
        /// This method used for get list of apps. -An
        /// </summary>
        /// <returns></returns>
        public async Task<List<ConsumerApps>> GetListOfApps()
        {
            return await _appsDataRepository.GetAll().ToListAsync();
        }

        /// <summary>
        /// This method used fro get apps object by id. -An
        /// </summary>
        /// <param name="id">pass app object primary key</param>
        /// <returns></returns>
        public async Task<ConsumerApps> GetConsumerAppsById(int id)
        {
            return await _appsDataRepository.FirstOrDefaultAsync(x => x.Id == id);
            
        }


        /// <summary>
        /// This method used for update consumer app and return primary key. -An
        /// </summary>
        /// <param name="apps"></param>
        /// <returns></returns>
        public async Task<int> UpdateConsumerApps(ConsumerApps consumerApps)
        {

            if (_appsDataRepository.FirstOrDefault(x => x.Name == consumerApps.Name && x.Id != consumerApps.Id) == null)
            {
                _appsDataRepository.UpdateAsync(consumerApps);
                await _appsDataRepository.SaveChangesAsync();
                return consumerApps.Id;
            }
            return 0;


        }

        #endregion

        #region "Private Method(s)"

        /// <summary>
        /// This method used for created random number For AuthId and Auth Secreate. -An
        /// </summary>
        /// <param name="isAuthId"></param>
        /// <returns></returns>
        private string CreatedRandomNumer(bool isAuthId)
        {

            var random = new Random();
            if (isAuthId)
            {
                return new string(Enumerable.Repeat(_stringConstant.SecretKeyGeneratorString, 15)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                return new string(Enumerable.Repeat(_stringConstant.SecureKeyGeneratorString, 30)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }

        }
        #endregion
    }
}
