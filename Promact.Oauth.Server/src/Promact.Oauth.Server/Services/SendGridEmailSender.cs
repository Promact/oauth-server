using Microsoft.Extensions.Options;
using Promact.Oauth.Server.Constants;
using Promact.Oauth.Server.Models;
using System.Net.Mail;

namespace Promact.Oauth.Server.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly StringConstant _stringConstant;

        public SendGridEmailSender(IOptions<AppSettings> appSettings, StringConstant stringConstant)
        {
            _appSettings = appSettings;
            _stringConstant = stringConstant;
        }

        public void SendEmail(string email, string subject, string message)
        {
            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new MailAddress(_appSettings.Value.From, _stringConstant.PromactName);
            myMessage.Subject = subject;
            myMessage.Text = message;

            var transportWeb = new SendGrid.Web(_appSettings.Value.SendGridApi);
            transportWeb.DeliverAsync(myMessage);
        }

    }
}
