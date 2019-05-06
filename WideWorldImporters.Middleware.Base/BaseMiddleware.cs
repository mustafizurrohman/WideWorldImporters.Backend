using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WideWorldImporters.Middleware.Base
{

    public abstract class BaseMiddleware
    {
        public RequestDelegate Next { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        public BaseMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public abstract Task InvokeAsync(HttpContext context);

    }

}
