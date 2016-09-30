using MailKit.Net.Smtp;
using MimeKit;
using Promact.Oauth.Server.Constants;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
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

        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger _logger;

        public AuthMessageSender(IOptions<AppSettings> appSettings, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AuthMessageSender>();
            _appSettings = appSettings;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            _logger.LogInformation("Start Email Service");
            // Plug in your email service here to send an email.
            var msg = new MimeMessage();

            msg.From.Add(new MailboxAddress("Promact", _appSettings.Value.From));
            msg.To.Add(new MailboxAddress("User", email));
            msg.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            msg.Body = bodyBuilder.ToMessageBody();
            try
            {
                using (var smtp = new SmtpClient())
                {
                    _logger.LogInformation("Start Email Sending");
                    smtp.ConnectAsync(_appSettings.Value.Host, _appSettings.Value.Port, MailKit.Security.SecureSocketOptions.None).Wait();
                    smtp.AuthenticateAsync(credentials: new NetworkCredential(_appSettings.Value.UserName, _appSettings.Value.Password)).Wait();
                    smtp.SendAsync(msg, CancellationToken.None).Wait();
                    smtp.DisconnectAsync(true, CancellationToken.None).Wait();
                    return Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Throw Email Error", ex);
                _logger.LogInformation("Throw Email Error Message", ex.Message);
                throw ex;
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
