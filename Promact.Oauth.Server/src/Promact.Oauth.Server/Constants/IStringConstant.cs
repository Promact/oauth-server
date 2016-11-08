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
         string SecureKeyGeneratorString { get; }
         string From { get; }
         string Password { get; }
         string Host { get; }
         string PromactName { get; }
         string Port { get; }
         string DefaultUserPassword { get; }
         string Name { get; }
         string EditName { get; }
         string SlackChannelName { get; }
         bool IsActive { get; }
         string TeamLeaderId { get; }
         string UserIdSecond { get; }
         string UserIdThird { get; }
         string FirstNameSecond { get; }
         string FirstNameThird { get; }
         string ProjectName { get; }
         string ProjectSlackChannelName { get; }
         string UserId { get; }
         string PasswordUser { get; }
         string UserName { get; }
         string SlackUserName { get; }
         string EmailUser { get; }
         string NewPassword { get; }
         string ConfirmPassword { get; }
         string UpadteFirstName { get; }
         string UpdateSlackUserName { get; }
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
         string RawEmailIdForTest { get; }
         string RawFirstNameForTest { get; }
         string RawLastNameForTest { get; }
         
         
         
         string OldPassword { get; }
         string Url { get; }



        string LoginCredentials { get;}
        string UserPassword { get;  }
        string UserEmail { get;  }
        string UserDetialTemplateFolderPath { get;  }
        string Format { get;  }
        string RoleEmployee { get; }
        string RoleTeamLeader { get;  }
        string RoleAdmin { get; }
        string Email { get;  }
        string LastName { get; }
        string FirstName { get;  }
        string SecretKeyGeneratorString { get;  }
        string InvalidLogin { get; }
        string SuccessfullySendMail { get; }
        string ForgotPassword { get;  }
        string ResertPasswordUserName { get; }
        string ResetPasswordLink { get;  }
        string ForgotPasswordTemplateFolderPath { get;  }
        string EmailNotExists { get; }
        string DomainAddress { get;  }

        AppConstant JsonDeserializeObject();
    }
}
