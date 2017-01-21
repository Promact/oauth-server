using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Promact.Oauth.Server.Constants;

namespace Promact.Oauth.Server.Services
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        private readonly IStringConstant _stringConstant;
        public SecurityHeadersAttribute(IStringConstant stringConstant)
        {
            _stringConstant = stringConstant;
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is ViewResult)
            {
                if (!context.HttpContext.Response.Headers.ContainsKey(_stringConstant.XContentTypeOptions))
                {
                    context.HttpContext.Response.Headers.Add(_stringConstant.XContentTypeOptions, _stringConstant.Nosniff);
                }
                if (!context.HttpContext.Response.Headers.ContainsKey(_stringConstant.XFrameOptions))
                {
                    context.HttpContext.Response.Headers.Add(_stringConstant.XFrameOptions, _stringConstant.Sameorigin);
                }

                var csp = _stringConstant.DefaultSrcSelf;
                // once for standards compliant browsers
                if (!context.HttpContext.Response.Headers.ContainsKey(_stringConstant.ContentSecurityPolicy))
                {
                    context.HttpContext.Response.Headers.Add(_stringConstant.ContentSecurityPolicy, csp);
                }
                // and once again for IE
                if (!context.HttpContext.Response.Headers.ContainsKey(_stringConstant.XContentSecurityPolicy))
                {
                    context.HttpContext.Response.Headers.Add(_stringConstant.XContentSecurityPolicy, csp);
                }
            }
        }
    }
}
