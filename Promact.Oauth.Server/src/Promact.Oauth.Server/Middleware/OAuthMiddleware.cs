using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Middleware
{
    public class OAuthMiddleware
    {
        readonly RequestDelegate next;
        readonly ILogger _logger;

        public OAuthMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<OAuthMiddleware>();
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"Request Headers starts for { context.Request.Path}");
            context.Request.Headers.Keys.ToList().ForEach(key => _logger.LogInformation($"{ key } - {context.Request.Headers[key]}"));
            _logger.LogInformation("Request Headers ends");
            await next.Invoke(context);
        }
    }
}
