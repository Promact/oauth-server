using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Services
{
    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string message);
    }
}
