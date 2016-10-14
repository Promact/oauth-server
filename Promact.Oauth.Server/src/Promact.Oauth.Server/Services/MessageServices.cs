using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using Promact.Oauth.Server.Constants;

namespace Promact.Oauth.Server.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713

    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly ILogger<AuthMessageSender> _logger;
        private readonly IOptions<AppSettings> _appSettings;
        
        public AuthMessageSender(IOptions<AppSettings> appSettings, ILogger<AuthMessageSender> logger)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public void SendEmail(string email, string subject, string message)
        {
            if (string.IsNullOrEmpty(_appSettings.Value.SendGridApi))
            {
                // Plug in your email service here to send an email.
                var msg = new MimeMessage();
                msg.From.Add(new MailboxAddress(StringConstant.PromactName, _appSettings.Value.From));
                msg.To.Add(new MailboxAddress("User", email));
                msg.Subject = subject;
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = message;
                msg.Body = bodyBuilder.ToMessageBody();
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_appSettings.Value.Host, _appSettings.Value.Port, _appSettings.Value.SslOnConnect == true ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.None);
                    smtp.Authenticate(credentials: new NetworkCredential(_appSettings.Value.UserName, _appSettings.Value.Password));
                    smtp.Send(msg, CancellationToken.None);
                    smtp.Disconnect(true, CancellationToken.None);
                }
            }
            else
            {
                var myMessage = new SendGrid.SendGridMessage();
                myMessage.AddTo(email);
                myMessage.From = new MailAddress(_appSettings.Value.From, StringConstant.PromactName);
                myMessage.Subject = subject;
                myMessage.Text = message;

                var transportWeb = new SendGrid.Web(_appSettings.Value.SendGridApi);
                transportWeb.DeliverAsync(myMessage);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }

    }
}
