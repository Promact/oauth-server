using Microsoft.Extensions.Options;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace Promact.Oauth.Server.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<AuthMessageSender> _logger;
        private readonly StringConstant _stringConstant;

        public SendGridEmailSender(IOptions<AppSettings> appSettings, ILogger<AuthMessageSender> logger)
        {
            _appSettings = appSettings;
            _stringConstant = stringConstant;
            _logger = logger;
        }

        public void SendEmail(string email, string subject, string message)
        {
            _logger.LogInformation("SendGrid: Start SendGrid Email Sending Method");
            var myMessage = new SendGrid.SendGridMessage();
            _logger.LogInformation("SendGrid: SendGrid Initiation");
            myMessage.AddTo(email);
            _logger.LogInformation("SendGrid: SendGrid AddTo");
            myMessage.From = new MailAddress(_appSettings.Value.From, StringConstant.PromactName);
            _logger.LogInformation("SendGrid: SendGrid From");
            myMessage.Subject = subject;
            _logger.LogInformation("SendGrid: SendGrid Subject");
            myMessage.Text = message;
            _logger.LogInformation("SendGrid: SendGrid Message");

            _logger.LogInformation("SendGrid: SendGrid Api Not Empty");
            var transportWeb = new SendGrid.Web(_appSettings.Value.SendGridApi);
            _logger.LogInformation("SendGrid: SendGrid Api transport Web");
            transportWeb.DeliverAsync(myMessage);
            _logger.LogInformation("SendGrid: SendGrid mail send");

        }

    }
}
