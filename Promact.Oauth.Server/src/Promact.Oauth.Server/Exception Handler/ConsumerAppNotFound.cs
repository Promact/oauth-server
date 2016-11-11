﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Oauth.Server.Exception_Handler
{
  
    public class ConsumerAppNotFound : Exception
    {
        /// <summary>
        /// Initializes Exception For Consumer App Not Found
        /// </summary>
        public ConsumerAppNotFound() : base()
        {

        }
    }
}
