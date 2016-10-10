using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Promact.Oauth.Server.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation("Start Email Send Method in Message Service");
            // Plug in your email service here to send an email.
            var msg = new MimeMessage();
            _logger.LogInformation("Email Credential 1");
            msg.From.Add(new MailboxAddress("Promact", _appSettings.Value.From));
            msg.To.Add(new MailboxAddress("User", email));
            msg.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            msg.Body = bodyBuilder.ToMessageBody();
            using (var smtp = new SmtpClient())
            {
                _logger.LogInformation("Smtp Connect");
                smtp.Connect(_appSettings.Value.Host, _appSettings.Value.Port);
                _logger.LogInformation("Authenticate");
                smtp.Authenticate(credentials: new NetworkCredential(_appSettings.Value.UserName, _appSettings.Value.Password));
                smtp.Send(msg, CancellationToken.None);
                smtp.Disconnect(true, CancellationToken.None);
                _logger.LogInformation("SendEmail Mail Successfully");
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }

    }
}
