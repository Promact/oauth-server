using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;

namespace Promact.Oauth.Server.Repository
{
    public interface IUserRepository
    {
        void AddUser(ApplicationUser user);
        IEnumerable<ApplicationUser> GetAllUsers();
        void UpdateUserDetails(ApplicationUser editedUser);
    }
}
