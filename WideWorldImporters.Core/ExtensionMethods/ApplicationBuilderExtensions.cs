﻿using Microsoft.AspNetCore.Builder;
using WideWorldImporters.Middleware.ExceptionHandler;

namespace WideWorldImporters.Core.ExtensionMethods
{
    /// <summary>
    /// ExtensionMethods for IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
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