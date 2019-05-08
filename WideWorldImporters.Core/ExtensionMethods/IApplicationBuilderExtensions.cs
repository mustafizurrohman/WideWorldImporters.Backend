using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Middleware.ExceptionHandler;

namespace WideWorldImporters.Core.ExtensionMethods
{
    /// <summary>
    /// ExtensionMethods for IApplcationBuilder
    /// </summary>
    public static class IApplicationBuilderExtensions
    {

        /// <summary>
        /// Extension Method to use custom exception handler
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandler>();
        }

        

    }
}
