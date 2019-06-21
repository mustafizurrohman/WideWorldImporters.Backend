using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WideWorldImporters.AuthenticationProvider.BaseClasses
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseAuthenticationProvider
    {


        public abstract Task Authenticate(HttpContext context);
    }
}
