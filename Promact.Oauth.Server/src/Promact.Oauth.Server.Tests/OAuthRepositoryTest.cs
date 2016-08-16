using Promact.Oauth.Server.Repository.OAuthRepository;
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
        public OAuthRepositoryTest():base()
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
            Assert.Null(response);
        }

        /// <summary>
        /// Checking method of client version.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async void GetAppDetailsFromClientNotNull()
        {
            var response = await _oAuthRepository.GetAppDetailsFromClient(RedirectUrl, refreshToken);
            Assert.NotNull(response);
        }

        /// <summary>
        /// Checking method of client version.
        /// </summary>
        [Fact, Trait("Category", "Required")]
        public async void GetAppDetailsFromClientTrueValue()
        {
            var response = await _oAuthRepository.GetAppDetailsFromClient(RedirectUrl, refreshToken);
            Assert.Equal(response.ClientSecret, ClientSecret);
        }

        /// <summary>
        /// Static Variables to be used in OAuth Repository Test
        /// </summary>
        private static string Email = "siddhartha@promactinfo.com";
        private static string ClientId = "dsfargazdfvfhfghkf";
        private static string RedirectUrl = "http://localhost:28182/oAuth/RefreshToken";
        private static string refreshToken = "adfjghefjakjdasdfsfhdjl";
        private static string ClientSecret = "q5UgIlqpRiFNkh8yK2i7SblNnqGEHZ";
    }
}
