using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Data_Repository;

namespace Promact.Oauth.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private IDataRepository<ApplicationUser> applicationUserDataRepository;

        public UserRepository(IDataRepository<ApplicationUser> _applicationUserDataRepository)
        {
            applicationUserDataRepository = _applicationUserDataRepository;
        }


        /// <summary>
        /// Registers User
        /// </summary>
        /// <param name="applicationUser"></param>
        public void AddUser(ApplicationUser applicationUser)
        {
            applicationUserDataRepository.Add(applicationUser);
        }


        /// <summary>
        /// Gets the list of all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return applicationUserDataRepository.List().ToList();
        }


        /// <summary>
        /// Edites the details of an user
        /// </summary>
        /// <param name="editedUser"></param>
        public void UpdateUserDetails(ApplicationUser editedUser)
        {
            applicationUserDataRepository.Update(editedUser);
        }


        
    }
}
