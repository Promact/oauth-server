using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
