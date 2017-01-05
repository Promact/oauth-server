namespace Promact.Oauth.Server.StringLiterals
{

    public class ConsumerApp
    {
        public string AlphaNumericString { get; set; }
        public string CapitalAlphaNumericString { get; set; }
    }
    public class Account
    {
        public string EmailNotExists { get; set; }
        public string SuccessfullySendMail { get; set; }
    }
    public class StringLiteral
    {
        public Account Account { get; set; }
        public ConsumerApp ConsumerApp { get; set; }
    }
    
}
