﻿using Promact.Oauth.Server.Repository.OAuthRepository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Promact.Oauth.Server.Tests
{
    public class OAuthRepositoryTest : BaseProvider
    {
        private readonly IOAuthRepository _oAuthRepository;
        public OAuthRepositoryTest() : base()
        {
            _oAuthRepository = serviceProvider.GetService<IOAuthRepository>();
        }

        /// <summary>
        /// Checking client is exist or not. If not it will create OAuth response for request.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void OAuthClientChecking()
        {
            var response = _oAuthRepository.OAuthClientChecking(Email, ClientId);
            Assert.Equal(response.Id,1);
        }

        /// <summary>
        /// Method to check verification of accesstoken 
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public void GetDetailsClientByAccessToken()
        {
            var response = _oAuthRepository.GetDetailsClientByAccessToken(accessToken);
            Assert.Equal(false, response);
        }


        /// <summary>
        /// Static Variables to be used in OAuth Repository Test
        /// </summary>
        private static string Email = "siddhartha@promactinfo.com";
        private static string ClientId = "dsfargazdfvfhfghkf";
        private static string accessToken = "bcd34169-1434-40e9-abf5-c9e0e9d20cd8";
    }
}
