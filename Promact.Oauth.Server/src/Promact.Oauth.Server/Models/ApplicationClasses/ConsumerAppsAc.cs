namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class ConsumerAppsAc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CallbackUrl { get; set; }
        public string AuthId { get; set; }
        public string AuthSecret { get; set; }
        public string CreatedBy { get; set; }
    }
}
