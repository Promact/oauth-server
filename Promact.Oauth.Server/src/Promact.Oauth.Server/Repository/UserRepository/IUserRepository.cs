using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;

namespace Promact.Oauth.Server.Repository
{
    public interface IUserRepository
    {
        void AddUser(UserAc newUser);

        UserAc GetById(string id);

        //void UpdateUserDetails(ApplicationUser editedUser);
        void UpdateUserDetails(UserAc editedUser);

        IEnumerable<UserAc> GetAllUsers();
    }
}
