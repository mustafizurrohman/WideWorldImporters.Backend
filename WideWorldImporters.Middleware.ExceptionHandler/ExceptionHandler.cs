using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WideWorldImporters.Middleware.Base;

namespace WideWorldImporters.Middleware.ExceptionHandler
{

    /// <summary>
    /// Exception Handler
    /// </summary>
    public class ExceptionHandler : BaseMiddleware
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestDelegate"></param>
        /// <param name="hostingEnvironment"></param>
        public ExceptionHandler(RequestDelegate requestDelegate, IHostingEnvironment hostingEnvironment) : base(requestDelegate)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public override async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                // await _next.Invoke(context);
                await Next(context);

            } catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                // TODO: Handle Exception in development mode
            } else
            {
                // TODO: Handle Exception when not in development mode (Staging or Production)
            }


            
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
  
            var result = JsonConvert.SerializeObject(new { error = ex.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }


    }
}
