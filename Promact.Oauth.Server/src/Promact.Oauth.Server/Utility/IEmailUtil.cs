namespace Promact.Oauth.Server.Utility
{
    public interface IEmailUtil
    {
        /// <summary>
        /// This method used to get email template for sending email. - An
        /// </summary>
        /// <param name="firstName">Passed first name</param>
        /// <param name="email">Passed user email address</param>
        /// <param name="password">Passed user password</param>
        /// <returns>User detail template</returns>
        string GetEmailTemplateForUserDetail(string firstName,string email,string password);
    }
}
