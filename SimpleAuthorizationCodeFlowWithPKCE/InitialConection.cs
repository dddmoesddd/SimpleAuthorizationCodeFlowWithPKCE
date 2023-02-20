using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Formats.Asn1.AsnWriter;

namespace SimpleAuthorizationCodeFlowWithPKCE
{
    public class InitialConection : IInitialConection
    {

        private readonly IHttpClientFactory _clientFactory;
   
        public InitialConection(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

        }

        public async Task<int> GetCode()
        {
            string code =   Utility.GetAuthorizationCodeWithPikce().Result;
            var token =await Utility.GetTokens(code);
            return 1;

        }
    }
}

