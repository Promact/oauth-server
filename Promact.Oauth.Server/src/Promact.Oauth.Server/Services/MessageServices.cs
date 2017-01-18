using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Threading;
using Promact.Oauth.Server.Models;
using Microsoft.Extensions.Options;
using Promact.Oauth.Server.Constants;
using MailKit.Security;

namespace Promact.Oauth.Server.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713

    public class AuthMessageSender : IEmailSender
    {
        
        private readonly IStringConstant _stringConstant;
        private readonly IOptions<EmailCrednetials> _emailCrednetials;

        public AuthMessageSender(IOptions<EmailCrednetials> emailCrednetials, IStringConstant stringConstant)
        {
            _stringConstant = stringConstant;
            _emailCrednetials = emailCrednetials;
        }


        /// <summary>
        /// This method is used to send email.
        /// </summary>
        /// <param name="email">Passed email</param>
        /// <param name="subject">Passed email subject</param>
        /// <param name="body">Passed email body</param>
        public void SendEmail(string email, string subject, string body)
        {
            // Plug in your email service here to send an email.
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Promact", _emailCrednetials.Value.From));
            msg.To.Add(new MailboxAddress("User", email));
            msg.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            msg.Body = bodyBuilder.ToMessageBody();
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_emailCrednetials.Value.Host, _emailCrednetials.Value.Port, GetSecureSocketOptions());
                smtp.Authenticate(credentials: new NetworkCredential(_emailCrednetials.Value.UserName, _emailCrednetials.Value.Password));
                smtp.Send(msg, CancellationToken.None);
                smtp.Disconnect(true, CancellationToken.None);
            }
        }
        
        #region Private Methods

        /// <summary>
        /// This method used for get secure Socket options.
        /// </summary>
        /// <returns>secure socket options</returns>
        private SecureSocketOptions GetSecureSocketOptions()
        {
            string smtpProtocol = _emailCrednetials.Value.SetSmtpProtocol.ToLower();
            //if user set stmp protocol way as SSL 
            if (string.Compare(smtpProtocol, _stringConstant.SetSmtpSSL) == 0)
               return SecureSocketOptions.SslOnConnect;
            //if user set stmp protocol way as UnSecure 
            else if (string.Compare(smtpProtocol, _stringConstant.SetSmtpUnSecure) == 0)
                return SecureSocketOptions.None;
            else
            return SecureSocketOptions.StartTls;
        }

        #endregion

    }
}
