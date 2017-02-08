using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{
    public class CustomProfileService : IProfileService
    {
        #region Private Variables
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringConstant _stringConstant;
        #endregion

        #region Constructor
        public CustomProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, UserManager<ApplicationUser> userManager, IStringConstant stringConstant)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
            _stringConstant = stringConstant;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Method used to pass value for our custom scope allowed
        /// </summary>
        /// <param name="context">ProfileDataRequestContext object</param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(userId);
            var principal = await _claimsFactory.CreateAsync(user);
            var claim = principal.Claims.ToList();
            claim = claim.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
            claim.Add(new System.Security.Claims.Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
            context.IssuedClaims = claim;
        }

        /// <summary>
        /// Method used to pass value for our custom scope allowed and set user isActive
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(userId);
            context.IsActive = user != null;
        }
        #endregion
    }
}
