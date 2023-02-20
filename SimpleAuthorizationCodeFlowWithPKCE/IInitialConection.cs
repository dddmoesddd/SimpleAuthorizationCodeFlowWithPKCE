using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuthorizationCodeFlowWithPKCE
{
    public  interface IInitialConection
    {
        Task<int> GetCode();
  
    }
}
