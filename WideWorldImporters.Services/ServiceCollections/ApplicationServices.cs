using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using WideWorldImporters.Core.CoreServices.Interfaces;
using WideWorldImporters.Logger.Interfaces;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.Interfaces;

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
        /// Automapper
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
        public IWWILogger Logger { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="autoMapper">Automapper</param>
        /// <param name="memoryCache">Memory Caching</param>
        /// <param name="redisService">Redis Caching</param>
        /// <param name="logger">Logging Service</param>
        public ApplicationServices(WideWorldImportersContext dbContext, IMapper autoMapper, 
            IMemoryCache memoryCache, IRedisService redisService, IWWILogger logger)
        {
            DbContext = dbContext;
            AutoMapper = autoMapper;
            MemoryCache = memoryCache;
            RedisService = redisService;
            Logger = logger;
        }

    }
}
