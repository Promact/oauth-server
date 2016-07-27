using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Seed
{
    public interface IEnsureSeedData
    {
        void Seed(IServiceProvider serviceProvider);
    }
}
