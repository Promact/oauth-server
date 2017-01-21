using Microsoft.Extensions.Options;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace Promact.Oauth.Server.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IOptions<SendGridAPI> _sendGridAPI;
        private readonly ILogger<AuthMessageSender> _logger;
        private readonly IStringConstant _stringConstant;

        public SendGridEmailSender(IOptions<SendGridAPI> sendGridAPI, IStringConstant stringConstant, ILogger<AuthMessageSender> logger)
        {
            _sendGridAPI = sendGridAPI;
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
            if (string.IsNullOrEmpty(_sendGridAPI.Value.From))
            {
                _logger.LogInformation("SendGrid: Email setting From is empty");
            }
            else
            {
                _logger.LogInformation("SendGrid: Email setting From obtained");
            }
            myMessage.From = new MailAddress(_sendGridAPI.Value.From, _stringConstant.PromactName);
            myMessage.Subject = subject;
            _logger.LogInformation("SendGrid: SendGrid Subject");
            myMessage.Html = message;
            _logger.LogInformation("SendGrid: SendGrid Message");

            _logger.LogInformation("SendGrid: SendGrid Api Not Empty");
            if (string.IsNullOrEmpty(_sendGridAPI.Value.SendGridApiKey))
            {
                _logger.LogInformation("SendGrid: SendGrid Api is empty");
            }
            else
            {
                _logger.LogInformation("SendGrid: SendGrid Api is obtained");
            }
            var transportWeb = new SendGrid.Web(_sendGridAPI.Value.SendGridApiKey);
            _logger.LogInformation("SendGrid: SendGrid Api transport Web");
            transportWeb.DeliverAsync(myMessage);
            _logger.LogInformation("SendGrid: SendGrid mail send");
        }

    }
}
