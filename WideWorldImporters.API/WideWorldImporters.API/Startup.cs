using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.ExtensionMethods;
using WideWorldImporters.Services.Interfaces;
using WideWorldImporters.Services.Services;

namespace WideWorldImporters.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        readonly Info info = new Info();
        readonly ApiKeyScheme apiKeyScheme = new ApiKeyScheme();

        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
    
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

            //Configure Compression level
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            //Add Response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            #endregion

            services.RegisterServices();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwaggerDocumentation(info);
            app.UseResponseCompression();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
