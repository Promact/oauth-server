using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Promact.Oauth.Server.Constants;
using System.IO;

namespace Promact.Oauth.Server.Utility
{
    public class EmailUtil : IEmailUtil
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStringConstant _stringConstant;
        private readonly ILogger _logger;

        public EmailUtil(IHostingEnvironment hostingEnvironment, IStringConstant stringConstant, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EmailUtil>();
            _hostingEnvironment = hostingEnvironment;
            _stringConstant = stringConstant;
        }

        /// <summary>
        /// This method used to get email template for sending email. - An
        /// </summary>
        /// <param name="firstName">Passed first name</param>
        /// <param name="email">Passed user email address</param>
        /// <param name="password">Passed user password</param>
        /// <returns>User detail template</returns>
        public string GetEmailTemplateForUserDetail(string firstName, string email, string password)
        {
            _logger.LogInformation("Log Information start");
            _logger.LogInformation("ContentRootPath", _hostingEnvironment.ContentRootPath);
            _logger.LogInformation("UserDetialTemplateFolderPath", _stringConstant.UserDetialTemplateFolderPath);
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, _stringConstant.UserDetialTemplateFolderPath);
            _logger.LogError("path", path);
            string finaleTemplate = File.ReadAllText(path);
            finaleTemplate = finaleTemplate.Replace(_stringConstant.UserEmail, email).Replace(_stringConstant.UserPassword, password).Replace(_stringConstant.ResertPasswordUserName, firstName);
            return finaleTemplate;
        }
    }
}
