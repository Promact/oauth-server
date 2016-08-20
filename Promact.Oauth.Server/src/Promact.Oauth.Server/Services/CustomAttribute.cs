using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Promact.Oauth.Server.Repository.OAuthRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{
    public class CustomAttribute : ActionFilterAttribute
    {
        private readonly IOAuthRepository _oAuthRepository;
        public CustomAttribute()
        {

        }
        public CustomAttribute(IOAuthRepository oAuthRepository)
        {
            _oAuthRepository = oAuthRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var token = filterContext.HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = token.Split(' ');
            var data = _oAuthRepository.GetDetailsClientByAccessToken(accessToken[1]);
            if (data == false)
            {
                base.OnActionExecuting(filterContext);
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                filterContext.Result = new JsonResult("Forbidden");
            }
        }
    }
}
