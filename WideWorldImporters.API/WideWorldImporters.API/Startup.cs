using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Hangfire;
using Hangfire.SqlServer;
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
using Microsoft.Net.Http.Headers;
using NLog;
using Swashbuckle.AspNetCore.Swagger;
using WideWorldImporters.AuthenticationProvider.Database;
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
        private readonly Info _info = new Info();
        private readonly ApiKeyScheme _apiKeyScheme = new ApiKeyScheme();
        private readonly PerformanceOptions _performanceOptions = new PerformanceOptions();
        private readonly List<string> _allowedCorsOrigins = new List<string>();

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

            Configuration.GetSection("PerformanceOptions").Bind(_performanceOptions);
    
            #region -- Swagger Configuration --

            Configuration.GetSection("Swagger").Bind(_info);
            Configuration.GetSection("ApiKeyScheme").Bind(_apiKeyScheme);
            services.AddSwaggerDocumentation(_info, _apiKeyScheme);


            #endregion

            #region -- Database Configuration --

            services.AddDbContext<WideWorldImportersContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("WideWorldDb"));
            });

            services.AddDbContext<AuthenticationProviderContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AuthenticationDb"));
            });

            #endregion

            #region -- Response Compression Configuration --

            if (_performanceOptions.UseResponseCompression)
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

            Configuration.GetSection("AllowedCorsOrigins").Bind(_allowedCorsOrigins);

            
            // Cors policy allowing only specific origins
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicies.CorsWithSpecificOrigins, builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithOrigins(_allowedCorsOrigins.ToArray());
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

            #endregion

            #region -- Hangfire Configuration

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            #endregion

            #region -- Logger Configuration --

            // services.AddSingleton<IWWILogger, NLogFileLogger>();
            // services.AddSingleton<IWWILogger, ConsoleLogger>();

            services.AddSingleton<NLogFileLogger>()
                    .AddSingleton<IWWILogger, NLogFileLogger>(s => s.GetService<NLogFileLogger>());

            services.AddSingleton<ConsoleLogger>()
                    .AddSingleton<IWWILogger, ConsoleLogger>(s => s.GetService<ConsoleLogger>());

            services.AddSingleton<AppLoggers>()
                    .AddSingleton<IWWILogger, AppLoggers>(s => s.GetService<AppLoggers>());


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
                // Force all communication to go through HTTPS!
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts(options => options.MaxAge(365).IncludeSubdomains().Preload());
            }

            // This response header prevents pages from loading in modern browsers 
            // When reflected cross-site scripting is detected.
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            // Ensure that site content is not being embedded in an iframe on other sites 
            //  - used for avoid click-jacking attacks.
            app.UseXfo(options => options.SameOrigin());

            // Blocks any content sniffing that could happen that might change an innocent MIME type (e.g. text/css) 
            // into something executable that could do some real damage.
            app.UseXContentTypeOptions();

            app.UseSwaggerDocumentation(_info);

            if (_performanceOptions.UseResponseCompression)
            {
                app.UseResponseCompression();
            }

            if (_performanceOptions.UseExceptionHandlingMiddleware)
            {
                app.UseCustomExceptionHandler();
            }

            if (!_allowedCorsOrigins.IsEmpty())
            {
                app.UseCors(CorsPolicies.CorsWithSpecificOrigins);

            } 

            app.UseCors();
            
            app.UseHttpsRedirection();

            app.UseHangfireDashboard();

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
