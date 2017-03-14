using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Promact.Oauth.Server.Repository
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        
        public GlobalExceptionFilter(ILoggerFactory logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger.CreateLogger("GlobalExceptionFilter");
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError("GlobalExceptionFilter: "+ context.Exception.Message);
            _logger.LogError("GlobalExceptionFilter: " + context.Exception.StackTrace);
        }
    }
}
