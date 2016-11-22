
using Exceptionless.Json;

namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class SlackUserDetailAc
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
