namespace Promact.Oauth.Server.Constants
{
    public interface IStringConstant
    {
        string ConsumerAppNameDemo { get; }
        string ConsumerAppNameDemo1 { get; }
        string ConsumerAppNameDemo2 { get; }
        string ConsumerAppNameDemo3 { get; }
        string ConsumerAppNameDemo4 { get; }
        string ConsumerAppNameDemo5 { get; }
        string ConsumerAppNameDemo6 { get; }
        string ConsumerAppNameDemo7 { get; }
        string ConsumerAppNameDemo8 { get; }
        string TwitterName { get; }
        string FaceBookName { get; }
        string ConsumerDescription { get; }
        string CallbackUrl { get; }
        string CreatedBy { get; }
        string UpdateBy { get; }
        string CapitalAlphaNumericString { get; }
        string AlphaNumericString { get; }
        string EmailNotExists { get; }
        string ForgotPassword { get; }
        string LoginCredentials { get; }
        string SuccessfullySendMail { get; }
        string ResetPasswordLink { get; }
        string ResertPasswordUserName { get; }
        string ForgotPasswordTemplateFolderPath { get; }
        string UserDetialTemplateFolderPath { get; }
        string From { get; }
        string Password { get; }
        string Host { get; }
        string PromactName { get; }
        string SlackUsersUrl { get; }
        string SlackUserByIdUrl { get; }
        string Port { get; }
        string OAuthExternalLoginUrl { get; }
        string Message { get; }
        string UserEmail { get; }
        string UserPassword { get; }
        string Name { get; }
        string EditName { get; }
        string SlackChannelName { get; }
        string ErpAuthorizeUrl { get; }
        string InCorrectSlackName { get; }
        bool IsActive { get; }
        string TeamLeaderId { get; }
        string UserIdSecond { get; }
        string UserIdThird { get; }
        string FirstNameSecond { get; }
        string FirstNameThird { get; }
        string ProjectName { get; }
        string ProjectSlackChannelName { get; }

        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string UserId { get; }
        string PasswordUser { get; }
        string UserName { get; }
        string SlackUserName { get; }
        string EmailUser { get; }
        string NewPassword { get; }
        string ConfirmPassword { get; }
        string Employee { get; }
        string Admin { get; }
        string TeamLeader { get; }
        string NormalizedName { get; }
        string NormalizedSecond { get; }
        string AccessToken { get; }
        string ClientIdForTest { get; }
        string CallBackUrl { get; }
        string PasswordForTest { get; }
        string EmptyString { get; }
        string InvalidLogin { get; }
        string RawEmailIdForTest { get; }
        string RawFirstNameForTest { get; }
        string RawLastNameForTest { get; }
        string OldPassword { get; }
        string Url { get; }
        string Format { get; }
        string DateFormate { get; }
        string TeamLeaderNotAssign { get; }
        string DomainAddress { get; }
        string SlackUserId { get; }
        string GetAppDetailsFromClientAsyncResponse { get; }
        string ReturnUrl { get; }
        string PromactAppNotSet { get; }
        string PromactAppNotFoundClientId { get; }
        string PromactAppNotFoundClientSecret { get; }
        string GetAppDetailsFromClientAsyncResponseInCorrectSlackName { get; }
        string OAuthAfterLoginResponse { get; }
        string SetSmtpUnSecure { get; }
        string SetSmtpSSL { get; }
        string HttpRequestExceptionErrorMessage { get; }
        string UserIdForTest { get; }
        string UserNameForTest { get; }
        string EmailForTest { get; }
        string SlackUserNameForTest { get; }
        string RandomString { get; }
        string PromactErpUrlForTest { get; }

        #region IdentityServer4
        string APIResourceName { get; }
        string APIResourceDisplayName { get; }
        string APIResourceApiSecrets { get; }
        string APIResourceSlackUserIdScope { get; }
        string APIResourceUserReadScope { get; }
        string APIResourceProjectReadScope { get; }
        string XContentTypeOptions { get; }
        string Nosniff { get; }
        string XFrameOptions { get; }
        string Sameorigin { get; }
        string ContentSecurityPolicy { get; }
        string DefaultSrcSelf { get; }
        string XContentSecurityPolicy { get; }
        string No { get; }
        string Yes { get; }
        string NoScopesMatching { get; }
        string InvalidClientId { get; }
        string NoConsentRequestMatchingRequest { get; }

        #region ConsentOptions
        string OfflineAccessDisplayName { get; }
        string OfflineAccessDescription { get; }
        string MuchChooseOneErrorMessage { get; }
        string InvalidSelectionErrorMessage { get; }
        #endregion

        #endregion

        #region Test cases
        string RandomClientId { get; }
        string RandomClientSecret { get; }
        string ExceptionMessageConsumerAppNameIsAlreadyExists { get; }
        string ExceptionMessageConsumerAppNotFound { get; }
        #endregion
    }
}
