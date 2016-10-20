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
        private readonly IStringConstant _stringConstant;
    
        public SendGridEmailSender(IOptions<AppSettings> appSettings, IStringConstant stringConstant, ILogger<AuthMessageSender> logger)
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
            if (string.IsNullOrEmpty(_appSettings.Value.From))
            {
                _logger.LogInformation("SendGrid: Email setting From is empty");
                throw new System.ArgumentNullException("Email from is null");
            }
            else
            {
                _logger.LogInformation("SendGrid: Email setting From obtained");
                myMessage.From = new MailAddress(_appSettings.Value.From, _stringConstant.PromactName);
            }
            myMessage.Subject = subject;
            _logger.LogInformation("SendGrid: SendGrid Subject");
            myMessage.Text = message;
            _logger.LogInformation("SendGrid: SendGrid Message");

            _logger.LogInformation("SendGrid: SendGrid Api Not Empty");
            if (string.IsNullOrEmpty(_appSettings.Value.SendGridApi))
            {
                _logger.LogInformation("SendGrid: SendGrid Api is empty");
                throw new System.ArgumentNullException("SendGrid Api is null");
            }
            else
            {
                _logger.LogInformation("SendGrid: SendGrid Api is obtained");
            }
            var transportWeb = new SendGrid.Web(_appSettings.Value.SendGridApi);
            _logger.LogInformation("SendGrid: SendGrid Api transport Web");
            transportWeb.DeliverAsync(myMessage);
            _logger.LogInformation("SendGrid: SendGrid mail send");
        }

    }
}
