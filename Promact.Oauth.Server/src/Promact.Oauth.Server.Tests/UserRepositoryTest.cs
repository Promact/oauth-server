using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promact.Oauth.Server.Data;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Models.ApplicationClasses;
using Promact.Oauth.Server.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Promact.Oauth.Server.Tests
{
    public class UserRepositoryTest
    {
        private readonly IUserRepository _userRepository;

        public UserRepositoryTest() : base()
        {
            //_userRepository = 
        }
    }
}
