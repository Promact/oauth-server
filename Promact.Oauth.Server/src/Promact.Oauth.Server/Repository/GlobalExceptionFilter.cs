using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var response = GetResponse(context);

            context.Result = new ObjectResult(response)
            {
                StatusCode = 500,
                DeclaredType = typeof(ErrorResponse)
            };

            _logger.LogError("GlobalExceptionFilter: "+ context.Exception);
        }

        private ErrorResponse GetResponse(ExceptionContext context)
        {
            return new ErrorResponse()
            {
                Message = context.Exception.Message,
                StackTrace = context.Exception.StackTrace
            };
        }
    }

    public class ErrorResponse
    {
        public ErrorResponse()
        {
        }

        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

}
