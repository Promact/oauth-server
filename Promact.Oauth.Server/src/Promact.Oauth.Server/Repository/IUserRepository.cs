using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClass;

namespace Promact.Oauth.Server.Repository
{
    public interface IUserRepository
    {
        void AddUser(UserModel newUser);
        IEnumerable<ApplicationUser> GetAllUsers();
        void UpdateUserDetails(ApplicationUser editedUser);
    }
}
