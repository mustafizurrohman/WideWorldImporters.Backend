using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WideWorldImporters.AuthenticationProvider.BaseClasses
{
    /// <summary>
    /// Base Authentication Provider
    /// </summary>
    public abstract class BaseAuthenticationProvider
    {
        /// <summary>
        /// Authentication function
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task Authenticate(HttpContext context);
    }
}
