using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Promact.Oauth.Server.Repository.OAuthRepository;
using System.Net;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{

    public class CustomAttribute : ActionFilterAttribute
    {
        private readonly IOAuthRepository _oAuthRepository;
        public CustomAttribute(IOAuthRepository oAuthRepository)
        {
            _oAuthRepository = oAuthRepository;
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            var token = filterContext.HttpContext.Request.Headers["Authorization"].ToString();
            var data = _oAuthRepository.GetDetailsClientByAccessToken(token).Result;
            if (data == true)
            {
                await base.OnActionExecutionAsync(filterContext, next);
            }
            else
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                filterContext.Result = new JsonResult("Forbidden");
                await base.OnActionExecutionAsync(filterContext, next);
            }
        }

    }
}
