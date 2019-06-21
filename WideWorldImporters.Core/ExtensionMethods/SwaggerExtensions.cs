using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace WideWorldImporters.Core.ExtensionMethods
{

    /// <summary>
    /// Extensions for Swagger
    /// </summary>
    public static class SwaggerExtensions
    {

        /// <summary>
        /// Extension method to configure swagger and add documentation
        /// </summary>
        /// <param name="services"></param>
        /// <param name="info"></param>
        /// <param name="apiKeyScheme"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, Info info, ApiKeyScheme apiKeyScheme)
        {
            info ??= default;
            apiKeyScheme ??= default;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(info.Version, new Info
                {
                    Version = info.Version,
                    Title = info.Title,
                    Description = info.Description,
                    TermsOfService = info.TermsOfService,
                    Contact = new Contact()
                    {
                        Name = info.Contact.Name,
                        Email = info.Contact.Email,
                        Url = info.Contact.Url
                    }
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = apiKeyScheme.Description,
                    Name = apiKeyScheme.Name,
                    In = apiKeyScheme.In,
                    Type = apiKeyScheme.Type
                });


            });

            return services;
        }

        /// <summary>
        /// Extension method to use swagger documentation
        /// </summary>
        /// <param name="app"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, Info info)
        {
            info ??= default;

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/" + info.Version + "/swagger.json", info.Title + " v" + info.Version);
            });

            return app;
        }

    }
}
