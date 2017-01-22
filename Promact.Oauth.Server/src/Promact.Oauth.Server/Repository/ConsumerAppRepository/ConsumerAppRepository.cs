using AutoMapper;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.ExceptionHandler;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.DbContexts;

namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public class ConsumerAppRepository : IConsumerAppRepository
    {
        #region "Private Variable(s)"
        private readonly IDataRepository<IdentityServer4.EntityFramework.Entities.Client, ConfigurationDbContext> _clientDataRepository;
        private readonly IMapper _mapperContext;
        private readonly IStringConstant _stringConstant;
        private readonly IDataRepository<ClientScope, ConfigurationDbContext> _scopes;
        private readonly IDataRepository<ClientSecret, ConfigurationDbContext> _secret;
        private readonly IDataRepository<ClientRedirectUri, ConfigurationDbContext> _redirectUri;
        private readonly IDataRepository<ClientPostLogoutRedirectUri, ConfigurationDbContext> _logoutRedirectUri;
        #endregion

        #region "Constructor"
        public ConsumerAppRepository(IMapper mapperContext, IStringConstant stringConstant, IDataRepository<IdentityServer4.EntityFramework.Entities.Client, ConfigurationDbContext> clientDataRepository,
            IDataRepository<ClientScope, ConfigurationDbContext> scopes, IDataRepository<ClientSecret, ConfigurationDbContext> secret, IDataRepository<ClientRedirectUri, ConfigurationDbContext> redirectUri,
            IDataRepository<ClientPostLogoutRedirectUri, ConfigurationDbContext> logoutRedirectUri)
        {
            _mapperContext = mapperContext;
            _stringConstant = stringConstant;
            _clientDataRepository = clientDataRepository;
            _scopes = scopes;
            _secret = secret;
            _redirectUri = redirectUri;
            _logoutRedirectUri = logoutRedirectUri;
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// This method used for get apps detail by client id. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<ConsumerApps> GetAppDetailsByClientIdAsync(string clientId)
        {
            var consumerApps = await GetConsumerByClientIdOfIdentityServerClient(clientId);
            if (consumerApps != null)
                return consumerApps;
            else
                throw new ConsumerAppNotFound();
        }

        /// <summary>
        /// This method used for added consumer app and return primary key. -An
        /// </summary>
        /// <param name="aapsObject"></param>
        /// <returns></returns>
        public async Task<IdentityServer4.Models.Client> AddConsumerAppsAsync(ConsumerApps consumerApp)
        {
            if (await _clientDataRepository.FirstOrDefaultAsync(x => x.ClientName == consumerApp.Name) == null)
            {
                consumerApp.AuthId = GetRandomNumber(true);
                consumerApp.AuthSecret = "superSecretPassword";
                var clientApp = ReturnIdentityServerClientFromConsumerApp(consumerApp, ReturnListOfScopesInStringFromEnumAllowedScope(consumerApp.Scopes));
                _clientDataRepository.Add(clientApp.ToEntity());
                await _clientDataRepository.SaveChangesAsync();
                return clientApp;
            }
            else
                throw new ConsumerAppNameIsAlreadyExists();
        }


        /// <summary>
        /// This method used for get list of apps. -An
        /// </summary>
        /// <returns></returns>
        public async Task<List<ConsumerApps>> GetListOfConsumerAppsAsync()
        {

            var listOfClient = await _clientDataRepository.GetAll().ToListAsync();
            List<ConsumerApps> listOfConsumerApp = new List<ConsumerApps>();
            ConsumerApps app;
            foreach (var client in listOfClient)
            {
                app = await GetConsumerByClientIdOfIdentityServerClient(client.ClientId);
                listOfConsumerApp.Add(app);
            }
            return listOfConsumerApp;
        }

        /// <summary>
        /// This method used for update consumer app and return primary key. -An
        /// </summary>
        /// <param name="apps"></param>
        /// <returns></returns>
        public async Task<IdentityServer4.EntityFramework.Entities.Client> UpdateConsumerAppsAsync(ConsumerApps consumerApps)
        {
            if (await _clientDataRepository.FirstOrDefaultAsync(x => x.ClientName == consumerApps.Name && x.ClientId != consumerApps.AuthId) == null)
            {
                var client = await _clientDataRepository.FirstOrDefaultAsync(x => x.ClientId == consumerApps.AuthId);
                client.ClientName = consumerApps.Name;
                _clientDataRepository.Update(client);
                await UpdateClientScope(client.Id, ReturnListOfScopesInStringFromEnumAllowedScope(consumerApps.Scopes));
                await UpdateClientRedirectUri(client.Id, consumerApps.CallbackUrl);
                await UpdateClientLogoutRedirectUri(client.Id, consumerApps.LogoutUrl);
                return client;
            }
            else
                throw new ConsumerAppNameIsAlreadyExists();
        }

        #endregion

        #region "Private Method(s)"

        /// <summary>
        /// This method used for get random number For AuthId and Auth Secreate. -An
        /// </summary>
        /// <param name="isAuthId">isAuthId = true (get random number for auth id.) and 
        /// isAuthId = false (get random number for auth secreate)</param>
        /// <returns></returns>
        private string GetRandomNumber(bool isAuthId)
        {
            var random = new Random();
            if (isAuthId)
            {
                return new string(Enumerable.Repeat(_stringConstant.CapitalAlphaNumericString, 15)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                return new string(Enumerable.Repeat(_stringConstant.AlphaNumericString, 30)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }

        private async Task<ConsumerApps> GetConsumerByClientIdOfIdentityServerClient(string clientId)
        {
            ConsumerApps app = new ConsumerApps();
            var client = await _clientDataRepository.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client != null)
            {
                var scopesAllowed = await _scopes.FetchAsync(x => x.Client.ClientId == clientId);
                List<AllowedScope> allowedScope = new List<AllowedScope>();
                app.Scopes = new List<AllowedScope>();
                foreach (var scopes in scopesAllowed)
                {
                    var value = (AllowedScope)Enum.Parse(typeof(AllowedScope), scopes.Scope);
                    app.Scopes.Add(value);
                }
                app.AuthId = clientId;
                var secret = await _secret.FirstOrDefaultAsync(x => x.Client.ClientId == clientId);
                app.AuthSecret = secret.Value;
                var callBackUri = await _redirectUri.FirstOrDefaultAsync(x => x.Client.ClientId == clientId);
                app.CallbackUrl = callBackUri.RedirectUri;
                app.Name = client.ClientName;
                var logoutUri = await _logoutRedirectUri.FirstOrDefaultAsync(x => x.Client.ClientId == clientId);
                app.LogoutUrl = logoutUri.PostLogoutRedirectUri;
            }
            return app;
        }

        private IdentityServer4.Models.Client ReturnIdentityServerClientFromConsumerApp(ConsumerApps consumerApp, List<string> allowedScopes)
        {
            return new IdentityServer4.Models.Client
            {
                ClientId = consumerApp.AuthId,
                ClientName = consumerApp.Name,
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                AllowAccessTokensViaBrowser = true,
                // where to redirect to after login
                RedirectUris = { consumerApp.CallbackUrl },
                // where to redirect to after logout
                PostLogoutRedirectUris = { consumerApp.LogoutUrl },
                AllowedScopes = allowedScopes,
                ClientSecrets = new List<IdentityServer4.Models.Secret>()
                        {
                            new IdentityServer4.Models.Secret(consumerApp.AuthSecret.Sha256())
                        },
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                AccessTokenLifetime = 86400,
                AuthorizationCodeLifetime = 86400,
                IdentityTokenLifetime = 86400,
                AbsoluteRefreshTokenLifetime = 5184000,
                SlidingRefreshTokenLifetime = 5184000
            };
        }

        private List<string> ReturnListOfScopesInStringFromEnumAllowedScope(List<AllowedScope> scopes)
        {
            List<string> allowedScope = new List<string>();
            foreach (var item in scopes)
            {
                var value = item.ToString();
                allowedScope.Add(item.ToString());
            }
            return allowedScope;
        }

        private async Task UpdateClientScope(int clientIdPrimaryKey, List<string> newScopes)
        {
            var client = await _clientDataRepository.FirstOrDefaultAsync(x => x.Id == clientIdPrimaryKey);
            var existingScopes = await _scopes.FetchAsync(x => x.Client.Id == clientIdPrimaryKey);
            foreach (var scope in newScopes)
            {
                if (!(existingScopes.FirstOrDefault(x => x.Scope == scope) != null))
                {
                    _scopes.Add(new ClientScope { Scope = scope, Client = client });
                    await _scopes.SaveChangesAsync();
                }
            }
            var scopesToBeDeleted = existingScopes.Where(x => !newScopes.Contains(x.Scope)).ToList();
            foreach (var scope in scopesToBeDeleted)
            {
                _scopes.Delete(scope);
                await _scopes.SaveChangesAsync();
            }
        }

        private async Task UpdateClientRedirectUri(int clientIdPrimaryKey, string redirectUri)
        {
            var existingRedirectUri = await _redirectUri.FirstOrDefaultAsync(x => x.Client.Id == clientIdPrimaryKey);
            if (!(existingRedirectUri.RedirectUri == redirectUri))
            {
                existingRedirectUri.RedirectUri = redirectUri;
                _redirectUri.Update(existingRedirectUri);
            }
        }

        private async Task UpdateClientLogoutRedirectUri(int clientIdPrimaryKey, string redirectUri)
        {
            var existingRedirectUri = await _logoutRedirectUri.FirstOrDefaultAsync(x => x.Client.Id == clientIdPrimaryKey);
            if (!(existingRedirectUri.PostLogoutRedirectUri == redirectUri))
            {
                existingRedirectUri.PostLogoutRedirectUri = redirectUri;
                _logoutRedirectUri.Update(existingRedirectUri);
            }
        }
        #endregion
    }
}
