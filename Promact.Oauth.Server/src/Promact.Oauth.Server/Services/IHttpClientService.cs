using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{
    public interface IHttpClientService
    {
        Task<string> GetAsync(string baseUrl, string contentUrl);
    }
}
