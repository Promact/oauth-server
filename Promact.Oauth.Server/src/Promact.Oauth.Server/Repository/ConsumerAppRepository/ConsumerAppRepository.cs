using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public class ConsumerAppRepository : IConsumerAppRepository
    {
        #region "Private Variable(s)"

        private readonly IDataRepository<ConsumerApps> _appsDataRepository;

        #endregion


        #region "Constructor"
        public ConsumerAppRepository(IDataRepository<ConsumerApps> appsDataRepository)
        {
            _appsDataRepository = appsDataRepository;
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// This method used for get apps detail by client id. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public ConsumerApps GetAppDetails(string clientId)
        {
            try
            {
                return _appsDataRepository.FirstOrDefault(x => x.AuthId == clientId);
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
        public int AddedConsumerApps(ConsumerApps consumerApps)
        {
            try
            {
                if (_appsDataRepository.FirstOrDefault(x => x.Name == consumerApps.Name) == null)
                {
                    consumerApps.AuthId = CreatedRandomNumer(true);
                    consumerApps.AuthSecret = CreatedRandomNumer(false);
                    _appsDataRepository.Add(consumerApps);
                    return consumerApps.Id;
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
        public List<ConsumerApps> GetListOfApps()
        {
            try
            {
                return _appsDataRepository.GetAll().ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// This method used fro get apps object by id. -An
        /// </summary>
        /// <param name="id">pass apps object primarykey</param>
        /// <returns></returns>
        public ConsumerApps GetAppsObjectById(int id)
        {
            try
            {
                return _appsDataRepository.FirstOrDefault(x => x.Id == id);
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
        public int UpdateConsumerApps(ConsumerApps consumerApps)
        {
            try
            {
                if (_appsDataRepository.FirstOrDefault(x => x.Name == consumerApps.Name && x.Id != consumerApps.Id) == null)
                {
                    _appsDataRepository.Update(consumerApps);
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
