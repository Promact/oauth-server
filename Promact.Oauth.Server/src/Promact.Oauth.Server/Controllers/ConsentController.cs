using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models.IdentityServer4;
using Promact.Oauth.Server.Services;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Controllers
{
    /// <summary>
    /// This controller processes the consent UI
    /// </summary>
    [ServiceFilter(typeof(SecurityHeadersAttribute))]
    public class ConsentController : BaseController
    {
        private readonly ICustomConsentService _consent;

        public ConsentController(ICustomConsentService consent)
        {
            _consent = consent;
        }

        /// <summary>
        /// Shows the consent screen
        /// </summary>
        /// <param name="returnUrl">redirect url of client server as string</param>
        /// <returns>view of allowed scope</returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var consentViewModel = await _consent.BuildViewModelAsync(returnUrl);
            if (consentViewModel != null)
            {
                return View("Index", consentViewModel);
            }

            return View("Error");
        }

        /// <summary>
        /// Handles the consent screen postback
        /// <param name="model">scope allowed</param>
        /// <returns>redirect uri of client project</returns>
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            var result = await _consent.ProcessConsent(model);

            if (result.IsRedirect)
            {
                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
            {
                ModelState.AddModelError("", result.ValidationError);
            }

            if (result.ShowView)
            {
                return View("Index", result.ViewModel);
            }

            return View("Error");
        }
    }
}
