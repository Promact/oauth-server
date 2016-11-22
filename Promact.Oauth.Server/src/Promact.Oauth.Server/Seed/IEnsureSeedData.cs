using System;

namespace Promact.Oauth.Server.Seed
{
    public interface IEnsureSeedData
    {
        void Seed(IServiceProvider serviceProvider);  
    }
}
