using Microsoft.AspNetCore.Mvc;
using Promact.Oauth.Server.Models;
using Promact.Oauth.Server.Repository.ConsumerAppRepository;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Promact.Oauth.Server.ExceptionHandler;

namespace Promact.Oauth.Server.Controllers
{
    [Authorize(Roles = Admin)]
    [Route(BaseUrl)]
    public class ConsumerAppController : Controller
    {
        #region "Private Variable(s)"
        private readonly IConsumerAppRepository _consumerAppRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public const string Admin = "Admin";
        public const string BaseUrl = "api/[controller]";
        #endregion

        #region "Constructor"
        public ConsumerAppController(IConsumerAppRepository iConsumerAppRepository, UserManager<ApplicationUser> userManager)
        {
            _consumerAppRepository = iConsumerAppRepository;
            _userManager = userManager;
        }

        #endregion

        #region Public Methods
        /**
        * @api {post} api/consumerapp 
        * @apiVersion 1.0.0
        * @apiName AddConsumerAppAsync
        * @apiGroup ConsumerApp
        * @apiParam {object} consumerAppsAc  object
        * @apiParamExample {json} Request-Example:  
        *  {
        *     "Id":"0",
        *     "Name":"ProjectName",
        *     "CallbackUrl":"http://www.example.com/signin-oidc",
        *     "AuthId" : "ASFSGHTRDHX",
        *     "AuthSecret" : "aFHF142SFG15212DF1GDFDSFSD2",
        *     "LogoutUrl" : "http://www.example.com/",
        *     "Scopes" : 
        *     "{
        *                   "email",
        *                   "openid",
        *                   "profile",
        *                   "slack_user_id",
        *     }"
        *  }      
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *     "Id" :"1"
        *     "Name":"ProjectName",
        *     "CallbackUrl":"http://www.example.com/signin-oidc",
        *     "AuthId" : "ASFSGHTRDHX",
        *     "AuthSecret" : "aFHF142SFG15212DF1GDFDSFSD2",
        *     "LogoutUrl" : "http://www.example.com/",
        *     "Scopes" : 
        *     "{
        *                   "email",
        *                   "openid",
        *                   "profile",
        *                   "slack_user_id",
        *     }"
        *  } 
        * @apiError ConsumerAppNameIsAlreadyExists The ConsumerApp Name is Already Exists
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 400 Bad Request 
        * {
        *   "error": "ConsumerAppNameIsAlreadyExists"
        * }
        */
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddConsumerAppAsync([FromBody]ConsumerApps consumerApps)
        {
            try
            {
                return Ok(await _consumerAppRepository.AddConsumerAppsAsync(consumerApps));
            }
            catch (ConsumerAppNameIsAlreadyExists)
            {
                return BadRequest();
            }
        }


        /**
        * @api {get} api/consumerapp 
        * @apiVersion 1.0.0
        * @apiName GetConsumerAppsAsync
        * @apiGroup ConsumerApp
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *   {
        *     "Name":"ProjectName",
        *     "CallbackUrl":"http://www.example.com/signin-oidc",
        *     "AuthId" : "ASFSGHTRDHX",
        *     "AuthSecret" : "aFHF142SFG15212DF1GDFDSFSD2",
        *     "LogoutUrl" : "http://www.example.com/",
        *     "Scopes" : 
        *     "{
        *                   "email",
        *                   "openid",
        *                   "profile",
        *                   "slack_user_id",
        *     }"
        *   } 
        * }
        */
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetConsumerAppsAsync()
        {
            var listOfApps = await _consumerAppRepository.GetListOfConsumerAppsAsync();
            return Ok(listOfApps);
        }


        /**
       * @api {get} api/consumerapp/:id 
       * @apiVersion 1.0.0
       * @apiName GetConsumerAppByIdAsync
       * @apiGroup ConsumerApp
       * @apiParam {int} id
       * @apiParamExample {json} Request-Example:  
       *        {
       *            "id":"ASFSGHTRDHX"
       *        }     
       * @apiSuccessExample {json} Success-Response:
       * HTTP/1.1 200 OK 
       * {
       *     "Name":"ProjectName",
       *     "CallbackUrl":"http://www.example.com/signin-oidc",
       *     "AuthId" : "ASFSGHTRDHX",
       *     "AuthSecret" : "aFHF142SFG15212DF1GDFDSFSD2",
       *     "LogoutUrl" : "http://www.example.com/",
       *     "Scopes" : 
       *     "{
       *                   "email",
       *                   "openid",
       *                   "profile",
       *                   "slack_user_id",
       *     }"
       *  } 
       * @apiError ConsumerAppNotFound The id of the ConsumerApp was not found.
       * @apiErrorExample {json} Error-Response:
       * HTTP/1.1 404 Not Found
       * {
       *   "error": "ConsumerAppNotFound"
       * }
       */
        [HttpGet]
        [Route("{clientId}")]
        public async Task<IActionResult> GetConsumerAppByIdAsync(string clientId)
        {
            try
            {
                var consumerApps = await _consumerAppRepository.GetAppDetailsByClientIdAsync(clientId);
                return Ok(consumerApps);
            }
            catch (ConsumerAppNotFound)
            {
                return NotFound();
            }
        }

        /**
        * @api {put} api/consumerapp 
        * @apiVersion 1.0.0
        * @apiName UpdateConsumerAppAsync
        * @apiGroup ConsumerApp
        * @apiParam {object} consumerAppsAc object
        * @apiParamExample {json} Request-Example: 
        * {
        *     "Id" : "1"
        *     "Name":"ProjectName",
        *     "CallbackUrl":"http://www.example.com/signin-oidc",
        *     "AuthId" : "ASFSGHTRDHX",
        *     "AuthSecret" : "aFHF142SFG15212DF1GDFDSFSD2",
        *     "LogoutUrl" : "http://www.example.com/",
        *     "Scopes" : 
        *     "{
        *                   "email",
        *                   "openid",
        *                   "profile",
        *                   "slack_user_id",
        *                   "user_read"
        *     }"
        *  }
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *   true
        * }
        * @apiError ConsumerAppNameIsAlreadyExists The ConsumerApp Name is Already Exists
        * @apiErrorExample {json} Error-Response:
        * HTTP/1.1 400 Bad Request
        * {
        *   "error": "ConsumerAppNameIsAlreadyExists"
        * }
        */
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateConsumerAppAsync([FromBody]ConsumerApps consumerApp)
        {
            return Ok(await _consumerAppRepository.UpdateConsumerAppsAsync(consumerApp));
        }


        /**
        * @api {get} api/consumerappgeneraterandomnumber/{isAuthId:bool}
        * @apiVersion 1.0.0
        * @apiName GetRandomNumber
        * @apiGroup ConsumerApp
        * @apiParam {bool} isAuthId
        * @apiParamExample {json} Request-Example: 
        * {
        *       "isAuthId" : "true"
        *  }
        * @apiSuccessExample {json} Success-Response:
        * HTTP/1.1 200 OK 
        * {
        *       "DADEGSFGFDVGD"
        * }
        */
        [HttpGet]
        [Route("generaterandomnumber/{isAuthId:bool}")]
        public IActionResult GetRandomNumber(bool isAuthId)
        {
            return Ok(_consumerAppRepository.GetRandomNumber(isAuthId));
        }

        #endregion
    }
}