using Promact.Oauth.Server.Models.IdentityServer4;
using Promact.Oauth.Server.Utility;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{
    public interface IConsentService
    {
        Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model);
        Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null);
    }
}
