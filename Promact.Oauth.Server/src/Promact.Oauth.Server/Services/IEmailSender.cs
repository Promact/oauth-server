namespace Promact.Oauth.Server.Services
{
    public interface IEmailSender
    {
        /// <summary>
        /// This method is used to send email.
        /// </summary>
        /// <param name="email">Passed email</param>
        /// <param name="subject">Passed email subject</param>
        /// <param name="body">Passed email body</param>
        void SendEmail(string email, string subject, string body);
    }
}
