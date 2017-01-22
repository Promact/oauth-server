using IdentityServer4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Models
{
    public enum AllowedScope
    {
        email,
        openid,
        profile,
        slack_user_id,
        user_read,
        project_read
    }
}
