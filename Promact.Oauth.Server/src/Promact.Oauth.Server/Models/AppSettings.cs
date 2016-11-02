namespace Promact.Oauth.Server.Models
{
    public class EmailCrednetials
    {
        public string From { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool SslOnConnect { get; set; }

    }

    public class SendGridAPI
    {
        public string From { get; set; }

        public string SendGridApi { get; set; }
    }
    
}
