using AutoMapper;
using Promact.Oauth.Server.Data_Repository;
using Promact.Oauth.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Promact.Oauth.Server.ExceptionHandler;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.DbContexts;
using Promact.Oauth.Server.StringLliterals;
using Microsoft.Extensions.Options;

namespace Promact.Oauth.Server.Repository.ConsumerAppRepository
{
    public class ConsumerAppRepository : IConsumerAppRepository
    {
        #region "Private Variable(s)"
        private readonly IDataRepository<IdentityServer4.EntityFramework.Entities.Client, ConfigurationDbContext> _clientDataRepository;
        private readonly IStringConstant _stringConstant;
        private readonly IDataRepository<ClientScope, ConfigurationDbContext> _scopes;
        private readonly IDataRepository<ClientSecret, ConfigurationDbContext> _secret;
        private readonly IDataRepository<ClientRedirectUri, ConfigurationDbContext> _redirectUri;
        private readonly IDataRepository<ClientPostLogoutRedirectUri, ConfigurationDbContext> _logoutRedirectUri;
        private readonly StringLiterals _stringLiterals;
        #endregion

        #region "Constructor"
        public ConsumerAppRepository(IStringConstant stringConstant, IDataRepository<IdentityServer4.EntityFramework.Entities.Client, ConfigurationDbContext> clientDataRepository,
            IDataRepository<ClientScope, ConfigurationDbContext> scopes, IDataRepository<ClientSecret, ConfigurationDbContext> secret, IDataRepository<ClientRedirectUri, ConfigurationDbContext> redirectUri,
            IDataRepository<ClientPostLogoutRedirectUri, ConfigurationDbContext> logoutRedirectUri,, IOptionsMonitor<StringLiterals> stringLiterals)
        {
            _stringConstant = stringConstant;
            _clientDataRepository = clientDataRepository;
            _scopes = scopes;
            _secret = secret;
            _redirectUri = redirectUri;
            _logoutRedirectUri = logoutRedirectUri;
            _stringLiterals = stringLiterals.CurrentValue;
        }

        #endregion

        #region "Public Method(s)"

        /// <summary>
        /// This method used for get apps detail by client id. 
        /// </summary>
        /// <param name="clientId">App's clientId</param>
        /// <returns>App details</returns>
        public async Task<ConsumerApps> GetAppDetailsByClientIdAsync(string clientId)
        {
            var consumerApps = await GetConsumerByClientIdOfIdentityServerClient(clientId);
            if (consumerApps.Id != 0)
                return consumerApps;
            else
                throw new ConsumerAppNotFound();
        }

        /// <summary>
        /// This method used for added consumer app and return consumerApps Id. -An
        /// </summary>
        /// <param name="consumerApp">App details as object</param>
        /// <returns>App details after saving changes as object</returns>
        public async Task<IdentityServer4.Models.Client> AddConsumerAppsAsync(ConsumerApps consumerApp)
        {
            if (await _clientDataRepository.FirstOrDefaultAsync(x => x.ClientId == consumerApp.AuthId) == null)
            {
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
        /// <returns>list of App</returns>
        public async Task<List<ConsumerApps>> GetListOfConsumerAppsAsync()
        {
            var listOfClient = await _clientDataRepository.GetAll().ToListAsync();
            List<ConsumerApps> listOfConsumerApp = new List<ConsumerApps>();
            foreach (var client in listOfClient)
            {
                var app = await GetConsumerByClientIdOfIdentityServerClient(client.ClientId);
                listOfConsumerApp.Add(app);
            }
            return listOfConsumerApp;
        }

        /// <summary>
        /// This method used for update consumer app and return primary key. -An
        /// </summary>
        /// <param name="consumerApps">App details as object</param>
        /// <returns>updated app details</returns>
        public async Task<IdentityServer4.EntityFramework.Entities.Client> UpdateConsumerAppsAsync(ConsumerApps consumerApps)
        {
            var client = await _clientDataRepository.FirstOrDefaultAsync(x => x.Id == consumerApps.Id);
            client.ClientName = consumerApps.Name;
            client.ClientId = consumerApps.AuthId;
            _clientDataRepository.Update(client);
            await UpdateClientSecret(client.Id, consumerApps.AuthSecret);
            await UpdateClientScope(client.Id, ReturnListOfScopesInStringFromEnumAllowedScope(consumerApps.Scopes));
            await UpdateClientRedirectUri(client.Id, consumerApps.CallbackUrl);
            await UpdateClientLogoutRedirectUri(client.Id, consumerApps.LogoutUrl);
            return client;
        }

        /// <summary>
        /// This method used for get random number For AuthId and Auth Secreate. -An
        /// </summary>
        /// <param name="isAuthId">isAuthId = true (get random number for auth id.) and 
        /// isAuthId = false (get random number for auth secreate)</param>
        /// <returns>Random generated code</returns>
        public string GetRandomNumber(bool isAuthId)
        {
            var random = new Random();
            if (isAuthId)
            {
                return new string(Enumerable.Repeat(_stringLiterals.CapitalAlphaNumericString, 15)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            else
            {
                return new string(Enumerable.Repeat(_stringLiterals.AlphaNumericString, 30)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        #endregion

        #region "Private Method(s)"
        /// <summary>
        /// Method to get App details for IdentityServer4 Client table in ConsumerApp object
        /// </summary>
        /// <param name="clientId">clientId of App</param>
        /// <returns>App details as object of ConsumerApp</returns>
        private async Task<ConsumerApps> GetConsumerByClientIdOfIdentityServerClient(string clientId)
        {
            ConsumerApps app = new ConsumerApps();
            var client = await _clientDataRepository.FirstOrDefaultAsync(x => x.ClientId == clientId);
            if (client != null)
            {
                var scopesAllowed = await _scopes.FetchAsync(x => x.Client.ClientId == clientId);
                app.Scopes = new List<AllowedScope>();
                foreach (var scopes in scopesAllowed)
                {
                    var value = (AllowedScope)Enum.Parse(typeof(AllowedScope), scopes.Scope);
                    app.Scopes.Add(value);
                }
                app.Id = client.Id;
                app.AuthId = client.ClientId;
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

        /// <summary>
        /// Method to convert ConsumerApp object to IdentityServer4 Client
        /// </summary>
        /// <param name="consumerApp">App details as object of ConsumerApp</param>
        /// <param name="allowedScopes">list of allowed scopes</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method to return list of string from list of enum scope
        /// </summary>
        /// <param name="scopes">list of enum scopes</param>
        /// <returns>list of string scope</returns>
        private List<string> ReturnListOfScopesInStringFromEnumAllowedScope(List<AllowedScope> scopes)
        {
            List<string> allowedScope = new List<string>();
            foreach (var item in scopes)
            {
                allowedScope.Add(item.ToString());
            }
            return allowedScope;
        }

        /// <summary>
        /// Method to updated Client scopes
        /// </summary>
        /// <param name="clientIdPrimaryKey">IdentityServer's Client primary key</param>
        /// <param name="newScopes">list of allowed scopes</param>
        private async Task UpdateClientScope(int clientIdPrimaryKey, List<string> newScopes)
        {
            var client = await _clientDataRepository.FirstOrDefaultAsync(x => x.Id == clientIdPrimaryKey);
            var existingScopes = (await _scopes.FetchAsync(x => x.Client.Id == clientIdPrimaryKey)).ToList();
            foreach (var scope in newScopes)
            {
                if (existingScopes.FirstOrDefault(x => x.Scope == scope) == null)
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

        /// <summary>
        /// Method to updated Client RedirectUri
        /// </summary>
        /// <param name="clientIdPrimaryKey">IdentityServer's Client primary key</param>
        /// <param name="redirectUri">new redirectUri</param>
        private async Task UpdateClientRedirectUri(int clientIdPrimaryKey, string redirectUri)
        {
            var existingRedirectUri = await _redirectUri.FirstOrDefaultAsync(x => x.Client.Id == clientIdPrimaryKey);
            if (existingRedirectUri.RedirectUri != redirectUri)
            {
                existingRedirectUri.RedirectUri = redirectUri;
                _redirectUri.Update(existingRedirectUri);
            }
        }

        /// <summary>
        /// Method to update client logout-redirectUri
        /// </summary>
        /// <param name="clientIdPrimaryKey">IdentityServer's Client primary key</param>
        /// <param name="redirectUri">logout redirectUri</param>
        private async Task UpdateClientLogoutRedirectUri(int clientIdPrimaryKey, string redirectUri)
        {
            var existingRedirectUri = await _logoutRedirectUri.FirstOrDefaultAsync(x => x.Client.Id == clientIdPrimaryKey);
            if (existingRedirectUri.PostLogoutRedirectUri != redirectUri)
            {
                existingRedirectUri.PostLogoutRedirectUri = redirectUri;
                _logoutRedirectUri.Update(existingRedirectUri);
            }
        }

        /// <summary>
        /// Method to update Client secret
        /// </summary>
        /// <param name="clientIdPrimaryKey">IdentityServer's Client primary key</param>
        /// <param name="secret">new Client secret</param>
        private async Task UpdateClientSecret(int clientIdPrimaryKey, string secret)
        {
            var existingSecret = await _secret.FirstOrDefaultAsync(x => x.Client.Id == clientIdPrimaryKey);
            if (existingSecret.Value != secret)
            {
                existingSecret.Value = secret.Sha256();
                _secret.Update(existingSecret);
            }
        }
        #endregion
    }
}
