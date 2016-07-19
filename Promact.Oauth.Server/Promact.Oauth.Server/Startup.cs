using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Promact.Oauth.Server.Startup))]
namespace Promact.Oauth.Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
