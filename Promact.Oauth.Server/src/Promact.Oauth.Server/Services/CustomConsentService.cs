using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models.IdentityServer4;
using Promact.Oauth.Server.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{
    public class CustomConsentService : ICustomConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<CustomConsentService> _logger;
        private readonly IStringConstant _stringConstant;

        public CustomConsentService(IIdentityServerInteractionService interaction, IClientStore clientStore,
            IResourceStore resourceStore, ILogger<CustomConsentService> logger, IStringConstant stringConstant)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _logger = logger;
            _stringConstant = stringConstant;
        }

        public async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
        {
            var result = new ProcessConsentResult();

            ConsentResponse grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (model.Button == _stringConstant.No)
            {
                grantedConsent = ConsentResponse.Denied;
            }
            // user clicked 'yes' - validate the data
            else if (model.Button == _stringConstant.Yes)
            {
                // if the user consented to some scope, build the response model
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    var scopes = model.ScopesConsented;
                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = scopes.ToArray()
                    };
                }
                else
                {
                    result.ValidationError = _stringConstant.MuchChooseOneErrorMessage;
                }
            }
            else
            {
                result.ValidationError = _stringConstant.InvalidSelectionErrorMessage;
            }

            if (grantedConsent != null)
            {
                // validate return url is still valid
                var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

                // communicate outcome of consent back to identityserver
                await _interaction.GrantConsentAsync(request, grantedConsent);

                // indiate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model.ReturnUrl;
            }
            else
            {
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
            }

            return result;
        }

        public async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (request != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
                if (client != null)
                {
                    var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        return CreateConsentViewModel(model, returnUrl, client, resources);
                    }
                    else
                    {
                        _logger.LogError(string.Format(_stringConstant.NoScopesMatching, request.ScopesRequested.Aggregate((x, y) => x + ", " + y)));
                    }
                }
                else
                {
                    _logger.LogError(string.Format(_stringConstant.InvalidClientId, request.ClientId));
                }
            }
            else
            {
                _logger.LogError(string.Format(_stringConstant.NoConsentRequestMatchingRequest, returnUrl));
            }

            return null;
        }

        private ConsentViewModel CreateConsentViewModel(ConsentInputModel model, string returnUrl, Client client, Resources resources)
        {
            var consentViewModel = new ConsentViewModel();
            consentViewModel.RememberConsent = model?.RememberConsent ?? true;
            consentViewModel.ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>();

            consentViewModel.ReturnUrl = returnUrl;

            consentViewModel.ClientName = client.ClientName;
            consentViewModel.ClientUrl = client.ClientUri;
            consentViewModel.ClientLogoUrl = client.LogoUri;
            consentViewModel.AllowRememberConsent = client.AllowRememberConsent;

            consentViewModel.IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, consentViewModel.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            consentViewModel.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => CreateScopeViewModel(x, consentViewModel.ScopesConsented.Contains(x.Name) || model == null)).ToArray();
            if (resources.OfflineAccess)
            {
                consentViewModel.ResourceScopes = consentViewModel.ResourceScopes.Union(new ScopeViewModel[] {
                    GetOfflineAccessScope(consentViewModel.ScopesConsented.Contains(IdentityServerConstants.StandardScopes.OfflineAccess) || model == null)
                });
            }

            return consentViewModel;
        }

        public ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required,
            };
        }

        public ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required,
            };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = _stringConstant.OfflineAccessDisplayName,
                Description = _stringConstant.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
            };
        }
    }
}
