using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WideWorldImporters.Middleware.Base
{

    /// <summary>
    /// Base Middleware- All Middlewares inherit this
    /// </summary>
    public abstract class BaseMiddleware
    {

        /// <summary>
        /// Request delagate
        /// </summary>
        public RequestDelegate Next { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        public BaseMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        /// <summary>
        /// Middleware implementation goes here
        /// Must be implemented by the middleware
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task InvokeAsync(HttpContext context);

    }

}
