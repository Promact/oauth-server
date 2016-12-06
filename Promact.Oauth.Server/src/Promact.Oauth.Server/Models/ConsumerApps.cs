using System.ComponentModel.DataAnnotations;

namespace Promact.Oauth.Server.Models
{
    public class ConsumerApps : OAuthBase
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [StringLength(255)]
        [Url]
        public string CallbackUrl { get; set; }

        [Required]
        [StringLength(15)]
        public string AuthId { get; set; }

        [Required]
        [StringLength(30)]
        public string AuthSecret { get; set; }

    }
}