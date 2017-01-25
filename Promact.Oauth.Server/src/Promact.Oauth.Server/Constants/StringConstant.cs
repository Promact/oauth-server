namespace Promact.Oauth.Server.Constants
{
    public class StringConstant : IStringConstant
    {

        public string ConsumerAppNameDemo { get { return "Demo Name"; } }
        public string ConsumerAppNameDemo1 { get { return "Demo Name1"; } }
        public string ConsumerAppNameDemo2 { get { return "Demo Name2"; } }
        public string ConsumerAppNameDemo3 { get { return "Demo Name3"; } }
        public string ConsumerAppNameDemo4 { get { return "Demo Name4"; } }
        public string ConsumerAppNameDemo5 { get { return "Demo Name5"; } }
        public string ConsumerAppNameDemo6 { get { return "Demo Name6"; } }
        public string ConsumerAppNameDemo7 { get { return "Demo Name7"; } }
        public string ConsumerAppNameDemo8 { get { return "Demo Name8"; } }
        public string TwitterName { get { return "Twitter Name"; } }
        public string FaceBookName { get { return "FaceBook Name"; } }
        public string ConsumerDescription { get { return "Consumer Description"; } }
        public string CallbackUrl { get { return "https://promact.slack.com/messages/@roshni/"; } }
        public string CreatedBy { get { return "Ankit"; } }
        public string UpdateBy { get { return "Roshni"; } }
        public string CapitalAlphaNumericString { get { return "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; } }
        public string AlphaNumericString { get { return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; } }
        public string EmailNotExists { get { return "Email does not exist"; } }
        public string ForgotPassword { get { return "Forgot Password"; } }
        public string LoginCredentials { get { return "Login Credentials"; } }
        public string SuccessfullySendMail { get { return "We have sent you a link on {{emailaddress}} to reset password.Please check your email."; } }
        public string ResetPasswordLink { get { return "${{RestPassWordLink}}$"; } }
        public string ResertPasswordUserName { get { return "${{Username}}$"; } }
        public string ForgotPasswordTemplateFolderPath { get { return "\\Template\\ForgotPassword.html"; } }
        public string UserDetialTemplateFolderPath { get { return "\\Template\\UserDetial.html"; } }
        public string From { get { return "From"; } }
        public string Password { get { return "Password"; } }
        public string Host { get { return "Host"; } }
        public string PromactName { get { return "Promact"; } }

        public string Port { get { return "Port"; } }

        public string UserEmail { get { return "${{Email}}$"; } }
        public string SlackUserNameForTest { get { return "SlackTest"; } }
        public string UserPassword { get { return "${{Password}}$"; } }
        public string Name { get { return "Project Name"; } }
        public string EditName { get { return "Project Name Edit"; } }
        public string SlackChannelName { get { return "Slack Channel Name"; } }
        public bool IsActive { get { return true; } }
        public string TeamLeaderId { get { return "1"; } }
        public string UserIdSecond { get { return "2"; } }
        public string UserIdThird { get { return "3"; } }
        public string FirstNameSecond { get { return "Secound First Name"; } }
        public string FirstNameThird { get { return "Third First Name"; } }
        public string ProjectName { get { return "Project Edit"; } }
        public string ProjectSlackChannelName { get { return "Slack Channel NameEdit"; } }
        public string SlackUsersUrl { get { return "/oAuth/SlackUserDetails"; } }
        public string SlackUserByIdUrl { get { return "/oAuth/SlackUserDetails/"; } }
        public string FirstName { get { return "First"; } }
        public string LastName { get { return "Last"; } }
        public string Email { get { return "test@promactinfo.com"; } }
        public string UserId { get { return "1"; } }
        public string PasswordUser { get { return "User@123"; } }
        public string UserName { get { return "testUser@pronactinfo.com"; } }
        public string SlackUserName { get { return "testSlackUserName"; } }
        public string EmailUser { get { return "testUsers@promactinfo.com"; } }
        public string NewPassword { get { return "User@1"; } }
        public string ConfirmPassword { get { return "User@1"; } }
        public string ErpAuthorizeUrl { get { return "/Home/SlackAuthorize"; } }
        public string InCorrectSlackName { get { return "Incorrect Slack Username. Please contact your administrator to edit your Slack Name in Promact OAuth server."; } }
        public string Message { get { return "?message="; } }
        public string OAuthExternalLoginUrl { get { return "{0}/OAuth/ExternalLogin?clientId={1}"; } }
        public string Employee { get { return "Employee"; } }
        public string Admin { get { return "Admin"; } }
        public string TeamLeader { get { return "TeamLeader"; } }
        public string NormalizedName { get { return "EMPLOYEE"; } }
        public string NormalizedSecond { get { return "ADMIN"; } }

        public string AccessToken { get { return "bcd34169-1434-40e9-abf5-c9e0e9d20cd8"; } }
        public string ClientIdForTest { get { return "adasfs21gv1drv1gd1sd"; } }
        public string CallBackUrl { get { return "http://www.example.com"; } }
        public string PasswordForTest { get { return "User@123"; } }
        public string EmptyString { get { return ""; } }
        public string InvalidLogin { get { return "Invalid login attempt."; } }
        public string RawEmailIdForTest { get { return "siddhartha@promactinfo.com"; } }
        public string RawFirstNameForTest { get { return "Siddhartha"; } }
        public string RawLastNameForTest { get { return "Shaw"; } }
        public string OldPassword { get { return "User@123"; } }
        public string Url { get { return "Url"; } }
        public string Format { get { return "yyyy-MM-dd"; } }
        public string DateFormate { get { return "dd'/'MM'/'yyyy"; } }
        public string TeamLeaderNotAssign { get { return "Not Assigned"; } }
        public string DomainAddress { get { return "@promactinfo.com"; } }
        public string SlackUserId { get { return "U0HJ49KJ4"; } }
        public string GetAppDetailsFromClientAsyncResponse
        {
            get
            {
                return "{\"ClientId\":\"adasfs21gv1drv1gd1sd\",\"ClientSecret\":\"dsjfhsijhfaJjJKDakjka521\",\"RefreshToken\":\"22719b46-e0d5-4ef2-ad63-6a5a9fe9842e\",\"ReturnUrl\":\"http://localhost:28182/Home/ExtrenalLoginCallBack\",\"UserId\":\"U0HJ49KJ4\"}";
            }
        }
        public string ReturnUrl { get { return "http://localhost:28182/Home/ExtrenalLoginCallBack"; } }
        public string PromactAppNotSet
        {
            get
            {
                return "Seems like getting error while fetching your promact app details. Please check your Promact app details";
            }
        }

        public string PromactAppNotFoundClientId
        {
            get
            {
                return "Seems like we don't have any Promact app for {0} client Id.";
            }
        }

        public string PromactAppNotFoundClientSecret
        {
            get
            {
                return "Seems like we don't have any Promact app for {0} client secret.";
            }
        }

        public string GetAppDetailsFromClientAsyncResponseInCorrectSlackName
        {
            get
            {
                return "{\"ClientId\":\"{0}\",\"ClientSecret\":\"{1}\",\"RefreshToken\":\"22719b46-e0d5-4ef2-ad63-6a5a9fe9842e\",\"ReturnUrl\":\"http://localhost:28182/Home/ExtrenalLoginCallBack\",\"UserId\":\"\"}";
            }
        }

        public string OAuthAfterLoginResponseUrl
        {
            get
            {
                return "{0}?accessToken={1}&email={2}&slackUserId={3}&userId={4}";
            }
        }
        public string SetSmtpUnSecure { get { return "unsecured"; } }
        public string SetSmtpSSL { get { return "ssl"; } }
        public string HttpRequestExceptionErrorMessage
        {
            get
            {
                return "An error occurred while sending the request to other server";
            }
        }
        public string GetAppDetailsFromClientAsyncUrl
        {
            get
            {
                return "?refreshToken={0}&slackUserName={1}";
            }
        }
        public string OAuthAfterLoginResponse
        {
            get
            {
                return "{0}?accessToken={1}&email={2}&slackUserId={3}&userId={4}";
            }
        }
        public string UserIdForTest { get { return "asder12346eewsee5s"; } }
        public string EmailForTest { get { return "abc@promactinfo.com"; } }
        public string UserNameForTest { get { return "XyzTest"; } }
        public string RandomString { get { return "abcdefghijklmnopqrstuvwxyz|ABCDEFGHIJKLMNOPQRSTUVWXYZ|012345789|@#$%^!&*()"; } }
        public string PromactErpUrlForTest { get { return "http://www.example.com"; } }

        #region IdentityServer4
        public string APIResourceName
        {
            get
            {
                return "read-only";
            }
        }

        public string APIResourceDisplayName
        {
            get
            {
                return "Promact OAuth Server details read only";
            }
        }

        public string APIResourceApiSecrets
        {
            get
            {
                return "promactUserInfo";
            }
        }

        public string APIResourceSlackUserIdScope
        {
            get
            {
                return "slack_user_id";
            }
        }

        public string APIResourceUserReadScope
        {
            get
            {
                return "user_read";
            }
        }

        public string APIResourceProjectReadScope
        {
            get
            {
                return "project_read";
            }
        }

        public string XContentTypeOptions
        {
            get
            {
                return "X-Content-Type-Options";
            }
        }

        public string Nosniff
        {
            get
            {
                return "nosniff";
            }
        }

        public string XFrameOptions
        {
            get
            {
                return "X-Frame-Options";
            }
        }

        public string Sameorigin
        {
            get
            {
                return "SAMEORIGIN";
            }
        }

        public string ContentSecurityPolicy
        {
            get
            {
                return "Content-Security-Policy";
            }
        }

        public string DefaultSrcSelf
        {
            get
            {
                return "default-src 'self'";
            }
        }

        public string XContentSecurityPolicy
        {
            get
            {
                return "X-Content-Security-Policy";
            }
        }

        public string No
        {
            get
            {
                return "no";
            }
        }

        public string Yes
        {
            get
            {
                return "yes";
            }
        }

        public string NoScopesMatching
        {
            get
            {
                return "No scopes matching: {0}";
            }
        }

        public string InvalidClientId
        {
            get
            {
                return "Invalid client id: {0}";
            }
        }

        public string NoConsentRequestMatchingRequest
        {
            get
            {
                return "No consent request matching request: {0}";
            }
        }

        #region ConsentOptions
        public string OfflineAccessDisplayName
        {
            get
            {
                return "Offline Access";
            }
        }
        public string OfflineAccessDescription
        {
            get
            {
                return "Access to your applications and resources, even when you are offline";
            }
        }

        public string MuchChooseOneErrorMessage
        {
            get
            {
                return "You must pick at least one permission";
            }
        }
        public string InvalidSelectionErrorMessage
        {
            get
            {
                return "Invalid selection";
            }
        }
        #endregion

        #endregion

        #region Test cases
        public string RandomClientId
        {
            get
            {
                return "ADFDASJASAJS";
            }
        }

        public string RandomClientSecret
        {
            get
            {
                return "ADFDASJASAJS12d5fsdf1dsf1sd5";
            }
        }

        public string ExceptionMessageConsumerAppNameIsAlreadyExists
        {
            get
            {
                return "Exception of type 'Promact.Oauth.Server.ExceptionHandler.ConsumerAppNameIsAlreadyExists' was thrown.";
            }
        }

        public string ExceptionMessageConsumerAppNotFound
        {
            get
            {
                return "Exception of type 'Promact.Oauth.Server.ExceptionHandler.ConsumerAppNotFound' was thrown.";
            }
        }

        public string ExceptionMessageSlackUserNotFound
        {
            get
            {
                return "Exception of type 'Promact.Oauth.Server.ExceptionHandler.SlackUserNotFound' was thrown.";
            }
        }

        public string ExceptionMessageFailedToFetchDataException
        {
            get
            {
                return "Exception of type 'Promact.Oauth.Server.ExceptionHandler.FailedToFetchDataException' was thrown.";
            }
        }
        #endregion
    }
}