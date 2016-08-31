using MailKit.Net.Smtp;
using MimeKit;
using Promact.Oauth.Server.Constants;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713

    public class AuthMessageSender : IEmailSender, ISmsSender
    {

        public AuthMessageSender()
        {

        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Promact", Environment.GetEnvironmentVariable(StringConstant.From)));
            msg.To.Add(new MailboxAddress("User", email));
            msg.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            msg.Body = bodyBuilder.ToMessageBody();
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.ConnectAsync(Environment.GetEnvironmentVariable(StringConstant.Host), Convert.ToInt32(Environment.GetEnvironmentVariable(StringConstant.Port)), MailKit.Security.SecureSocketOptions.None).Wait();
                    smtp.AuthenticateAsync(credentials: new NetworkCredential(Environment.GetEnvironmentVariable(StringConstant.From), Environment.GetEnvironmentVariable(StringConstant.Password))).Wait();
                    smtp.SendAsync(msg, CancellationToken.None).Wait();
                    smtp.DisconnectAsync(true, CancellationToken.None).Wait();
                    return Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
