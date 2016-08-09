using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Promact.Oauth.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly AppSettings _appSettings;
        public AuthMessageSender(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            //Creates MimeMessage object and sets necessary parameters
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_appSettings.From, _appSettings.Email));
            msg.To.Add(new MailboxAddress("User", email));
            msg.Subject = subject;
            msg.Body = new TextPart("plain")
            {
                Text = message
            };

            //Creates an SMTP object, connects to the server and sends mail
            try
            {
                using (var client = new SmtpClient())
                {
                    client.ConnectAsync("webmail.promactinfo.com", 25, MailKit.Security.SecureSocketOptions.None).Wait();
                    client.AuthenticateAsync(credentials: new NetworkCredential(_appSettings.Email, _appSettings.Password)).Wait();
                    client.SendAsync(msg, CancellationToken.None).Wait();
                    client.DisconnectAsync(true, CancellationToken.None).Wait();

                    return Task.FromResult(0);
                }
            }
            catch(Exception e)
            {
                return Task.FromException(e);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
