using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WideWorldImporters.Logger.Implementation;
using WideWorldImporters.Logger.Interfaces;

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

        /// <summary>
        /// Returns an instance of the Logger
        /// </summary>
        /// <param name="_serviceProvider"></param>
        /// <returns></returns>
        public IWWILogger GetAppLogger(IServiceProvider _serviceProvider)
        {
            if (_serviceProvider is ISupportRequiredService requiredServiceSupportingProvider)
            {
                return requiredServiceSupportingProvider.GetRequiredService(typeof(AppLoggers)) as AppLoggers;
            }

            var service = _serviceProvider.GetService(typeof(AppLoggers));

            return service as AppLoggers;
        }

    }

}
