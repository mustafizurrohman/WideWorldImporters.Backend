using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using NLog;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using WideWorldImporters.Core.Enumerations;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Core.Options;
using WideWorldImporters.Logger.Implementation;
using WideWorldImporters.Logger.Interfaces;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.ExtensionMethods;

namespace WideWorldImporters.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        readonly Info info = new Info();
        readonly ApiKeyScheme apiKeyScheme = new ApiKeyScheme();
        readonly PerformanceOptions performanceOptions = new PerformanceOptions();
        readonly List<string> allowedCorsOrigins = new List<string>();

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public void ConfigureServices(IServiceCollection services)
        {

            Configuration.GetSection("PerformanceOptions").Bind(performanceOptions);
    
            #region -- Swagger Configuration --

            Configuration.GetSection("Swagger").Bind(info);
            Configuration.GetSection("ApiKeyScheme").Bind(apiKeyScheme);
            services.AddSwaggerDocumentation(info, apiKeyScheme);


            #endregion

            #region -- Database Configuration --

            services.AddDbContext<WideWorldImportersContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("WideWorldDb"));
            });

            #endregion

            #region -- Response Compression Configuration --

            if (performanceOptions.UseResponseCompression)
            {

                // Configure Compression level
                services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

                // Add Response compression services
                services.AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                });

            }

            #endregion

            #region -- CORS Configuration --

            // Default cors policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin();
                });
            });

            Configuration.GetSection("AllowedCorsOrigins").Bind(allowedCorsOrigins);

            
            // Cors policy allowing only specific origins
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicies.CorsWithSpecificOrigins, builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithOrigins(allowedCorsOrigins.ToArray());
                });
            });

            #endregion

            #region -- Redis Configuration --

            var url = Configuration["RedisURL"];

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = url;
            });

            #endregion

            #region -- Service Configuration --

            services.RegisterServices();
            services.AddOData();

            services.AddSingleton<IWWILogger, WWILogger>();

            #endregion

            #region -- MVC Configuration --

            services.AddMvc(options => {

                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                options.MaxModelValidationErrors = int.MaxValue;
                options.MaxValidationDepth = 100;

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #endregion

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder to configure the application request pipeline</param>
        /// <param name="env">Current hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                // Force all comms to go through HTTPS!
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts(options => options.MaxAge(365).IncludeSubdomains().Preload());
            }

            // This response header prevents pages from loading in modern browsers 
            // When reflected cross-site scription is detected.
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            // Ensure that site content is not being embedded in an iframe on other sites 
            //  - used for avoid clickjacking attacks.
            app.UseXfo(options => options.SameOrigin());

            // Blocks any content sniffing that could happen that might change an innocent MIME type (e.g. text/css) 
            // into something executable that could do some real damage.
            app.UseXContentTypeOptions();

            app.UseSwaggerDocumentation(info);

            if (performanceOptions.UseResponseCompression)
            {
                app.UseResponseCompression();
            }

            if (performanceOptions.UseExceptionHandlingMiddleware)
            {
                app.UseCustomExceptionHandler();
            }

            if (!allowedCorsOrigins.IsEmpty())
            {
                app.UseCors(CorsPolicies.CorsWithSpecificOrigins);

            } 

            app.UseCors();
            
            app.UseHttpsRedirection();

            app.UseMvc(routeBuilder => {

                routeBuilder.EnableDependencyInjection();

                routeBuilder.Expand(QueryOptionSetting.Allowed)
                    .Count(QueryOptionSetting.Allowed)
                    .Select(QueryOptionSetting.Allowed)
                    .OrderBy(QueryOptionSetting.Allowed)
                    .Filter(QueryOptionSetting.Allowed)
                    .MaxTop(int.MaxValue);
            });

        }
    }
}
