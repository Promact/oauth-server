using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Constants
{
    public class StringConstant:IStringConstant
    {

        public string ConsumerAppNameDemo { get { return "Demo Name"; } }
        public string ConsumerAppNameDemo1 { get { return "Demo Name1"; } }
        public string ConsumerAppNameDemo2 { get { return "Demo Name2"; } }
        public string ConsumerAppNameDemo3 { get { return "Demo Name3"; } }
        public string ConsumerAppNameDemo4 { get { return "Demo Name4"; } }
        public string ConsumerAppNameDemo5 { get { return "Demo Name5"; } }
        public string ConsumerAppNameDemo6 { get {return "Demo Name6"; } }
        public string ConsumerAppNameDemo7 { get { return "Demo Name7"; } }
        public string ConsumerAppNameDemo8 { get { return "Demo Name8"; } }
        public string TwitterName { get { return "Twitter Name"; } }
        public string FaceBookName { get { return "FaceBook Name"; } }
        public string ConsumerDescription { get { return "Consumer Description"; } }
        public string CallbackUrl { get { return "https://promact.slack.com/messages/@roshni/"; } }
        public string CreatedBy { get { return "Ankit"; } }
        public string UpdateBy{get { return "Roshni"; }}
        public string ATOZ0TO9 { get { return "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; } }
        public string ATOZaTOz0TO9 { get { return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; } }
        public string EmailNotExists { get { return "Email does not exist"; } }
        public string ForgotPassword { get {return "Forgot Password"; } }
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

        public string Port{get{return "Port";}}

        public string UserEmail { get { return "${{Email}}$"; } }

        public string UserPassword { get { return "${{Password}}$"; } }
        public string DefaultUserPassword { get { return "User@123"; } }

        public string Name { get { return "Project Name"; } }
        public string EditName { get { return "Project Name Edit"; } }
        public string SlackChannelName { get { return "Slack Channel Name"; } }
        public bool IsActive { get { return true; } }
        public string TeamLeaderId { get { return "1"; } }
        public string UserIdSecond {get{return "2"; } }
        public string UserIdThird { get { return "3"; } }
        public string FirstNameSecond {get{return "Secound First Name"; } }
        public string FirstNameThird { get { return "Third First Name"; } }
        public string ProjectName { get { return "Project Edit"; } }
        public string ProjectSlackChannelName { get { return "Slack Channel NameEdit"; } }

        public string FirstName {get{return "First"; } }
        public string LastName { get { return "Last"; } }
        public string Email {get{return "test@promactinfo.com"; } }
        public string UserId { get { return "1"; } }
        public string PasswordUser { get { return "User@123"; } }
        public string UserName {get{return "testUser@pronactinfo.com"; } }
        public string SlackUserName { get { return "testSlackUserName"; } }
        public string EmailUser  { get{ return "testUsers@promactinfo.com"; } }
        public string NewPassword { get { return "User@1"; } }
        public string ConfirmPassword { get { return "User@1"; } }
        public string UpadteFirstName { get { return "Updated User"; } }

        public string UpdateSlackUserName { get { return "Updated test"; } }

        public string Employee { get { return "Employee"; } }
        public string Admin { get { return "Admin"; } }
        public string TeamLeader { get { return "TeamLeader"; } }
        public string NormalizedName { get { return "EMPLOYEE"; } }
        public string NormalizedSecond { get { return "ADMIN";} }

        public string AccessToken {get {return "bcd34169-1434-40e9-abf5-c9e0e9d20cd8"; } }
        public string ClientIdForTest { get { return "adasfs21gv1drv1gd1sd"; } }
        public string CallBackUrl { get { return "http://www.example.com"; } }
        public string PasswordForTest { get { return "User@123"; } }
        public string EmptyString { get { return ""; } }
        public string InvalidLogin { get { return "Invalid login attempt."; } }
        public string RawEmailIdForTest { get { return "siddhartha@promactinfo.com"; } }
        public string RawFirstNameForTest { get { return "Siddhartha"; } }
        public string RawLastNameForTest {get { return "Shaw"; }} 
        public string RoleEmployee {get { return "Employee"; }}
        public string RoleAdmin {get { return "Admin"; }}
        public string RoleTeamLeader {get { return "TeamLeader"; }}
        public string OldPassword {get { return "User@123"; }}
        public string Url {get { return "Url"; }}
        public string Format {get { return "yyyy-MM-dd"; }}
        public string DateFormate{get { return "dd'/'MM'/'yyyy"; }}
        public string TeamLeaderNotAssign {get { return "Not Assigned"; }}
    }
}