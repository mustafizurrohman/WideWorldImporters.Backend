using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using WideWorldImporters.Logger.Interfaces;
using WideWorldImporters.Middleware.Base;

namespace WideWorldImporters.Middleware.ExceptionHandler
{

    /// <summary>
    /// Exception Handler
    /// </summary>
    public class ExceptionHandler : BaseMiddleware
    {

        /// <summary>
        /// Current hosting environment
        /// </summary>
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestDelegate">Request Delegate</param>
        /// <param name="hostingEnvironment">Hosting Environment</param>
        /// <param name="serviceProvider">ServiceProvider. For DI</param>
        public ExceptionHandler(RequestDelegate requestDelegate, IHostingEnvironment hostingEnvironment, IServiceProvider serviceProvider) : base(requestDelegate)
        {
            _hostingEnvironment = hostingEnvironment;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Middleware Implementation
        /// </summary>
        /// <param name="context">Http Context</param>
        /// <returns></returns>
        public override async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await Next.Invoke(context);

            } catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Async handling of exception
        /// </summary>
        /// <param name="context">Http Context</param>
        /// <param name="ex">Exception</param>
        /// <returns></returns>
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                // TODO: Handle Exception in development mode
            } else
            {
                // TODO: Handle Exception when not in development mode (Staging or Production)
            }

            // var result = JsonConvert.SerializeObject(new { error = ex.Message });
            var logger = GetLogger();
            logger.LogException(ex);


            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(ex.Message);
        }

        private IWWILogger GetLogger()
        {
            var x = _serviceProvider.GetService(typeof(IWWILogger));

            return x as IWWILogger;
        }


    }
}
