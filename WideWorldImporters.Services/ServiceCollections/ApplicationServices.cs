using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using WideWorldImporters.AuthenticationProvider.Database;
using Microsoft.Extensions.DependencyInjection;
using WideWorldImporters.Core.CoreServices.Interfaces;
using WideWorldImporters.Logger.Implementation;
using WideWorldImporters.Models.Database;

namespace WideWorldImporters.Services.ServiceCollections
{

    /// <summary>
    /// Services for the application
    /// </summary>
    public class ApplicationServices
    {
        /// <summary>
        /// Application Database context
        /// </summary>
        public WideWorldImportersContext DbContext { get; }

        /// <summary>
        /// Authentication Provider DATA
        /// </summary>
        public AuthenticationProviderContext AuthDbContext { get; }

        /// <summary>
        /// AutoMapper 
        /// </summary>
        public IMapper AutoMapper { get; }

        /// <summary>
        /// InMemory Local Caching
        /// </summary>
        public IMemoryCache MemoryCache { get; }

        /// <summary>
        /// Redis Caching
        /// </summary>
        public IRedisService RedisService { get; }

        /// <summary>
        /// Logging service
        /// </summary>
        public AppLoggers Logger { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="autoMapper">AutoMapper</param>
        /// <param name="memoryCache">Memory Caching</param>
        /// <param name="redisService">Redis Caching</param>
        /// <param name="logger">Logging Service</param>
        /// <param name="authDbContext">Authorization Database Context</param>
        public ApplicationServices(WideWorldImportersContext dbContext, IMapper autoMapper, 
            IMemoryCache memoryCache, IRedisService redisService, AppLoggers logger,
            AuthenticationProviderContext authDbContext)
        {
            DbContext = dbContext;
            AutoMapper = autoMapper;
            MemoryCache = memoryCache;
            RedisService = redisService;
            AuthDbContext = authDbContext;
            Logger = logger;
        }

    }
}
