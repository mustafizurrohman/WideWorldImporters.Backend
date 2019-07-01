using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WideWorldImporters.API
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">Array of string as argument</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// CreateWebHostBuilder
        /// </summary>
        /// <param name="args">Array of string as argument</param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    // Do not let the client know that we are using Kestrel
                    options.AddServerHeader = false;
                })
                .UseStartup<Startup>();
    }
}
