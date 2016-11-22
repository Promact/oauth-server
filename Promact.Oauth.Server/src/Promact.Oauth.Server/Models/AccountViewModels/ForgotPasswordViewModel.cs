using System.ComponentModel.DataAnnotations;

namespace Promact.Oauth.Server.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid email address")]
        public string Email { get; set; }
    }
}
