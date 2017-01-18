namespace Promact.Oauth.Server.Models.ApplicationClasses
{
    public class UserRoleAc
    {
        public UserRoleAc(string userId, string userName, string name, string role)
        {
            UserId = userId;
            UserName = userName;
            Name = name;
            Role = role;
        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
