using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WideWorldImporters.API.Swagger
{
    public static class SwaggerExtensions
    {

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, Info info, ApiKeyScheme apiKeyScheme)
        {
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

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, Info info)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/" + info.Version + "/swagger.json", info.Title + " v" + info.Version);
            });

            return app;
        }

    }
}
