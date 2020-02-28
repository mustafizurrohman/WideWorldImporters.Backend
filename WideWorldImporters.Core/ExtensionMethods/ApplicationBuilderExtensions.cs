using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WideWorldImporters.AuthenticationProvider.Database;
using WideWorldImporters.Middleware.ExceptionHandler;
using WideWorldImporters.Models.Database;

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
        /// <param name="applicationBuilder">Application Builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ExceptionHandler>();
        }

        /// <summary>
        /// Migrates the database.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <returns></returns>
        public static void MigrateDatabase(this IApplicationBuilder applicationBuilder)
        {
            using IServiceScope serviceScope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            {
                using var context = serviceScope.ServiceProvider.GetService<WideWorldImportersContext>();
                context.Database.Migrate();

                using var contextAuthProvider = serviceScope.ServiceProvider.GetService<AuthenticationProviderContext>();
                contextAuthProvider.Database.Migrate();

            }


        }

    }

}
