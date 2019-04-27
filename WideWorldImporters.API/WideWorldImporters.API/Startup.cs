using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using WideWorldImporters.API.Swagger;
using WideWorldImporters.Models.Database;

namespace WideWorldImporters.API
{
    public class Startup
    {
        readonly Info info = new Info();
        readonly ApiKeyScheme apiKeyScheme = new ApiKeyScheme();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
    
            #region -- Swagger --

            Configuration.GetSection("Swagger").Bind(info);
            Configuration.GetSection("ApiKeyScheme").Bind(apiKeyScheme);
            services.AddSwaggerDocumentation(info, apiKeyScheme);

            #endregion

            services.AddDbContext<WideWorldImportersContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("WideWorldDb"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
