using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
        /// Memory Caching
        /// </summary>
        public IMemoryCache MemoryCache { get; }

        /// <summary>
        /// Redis Cache
        /// </summary>
        public IRedisService RedisService { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="autoMapper">Automapper</param>
        /// <param name="memoryCache">Memory Caching</param>
        /// <param name="redisService">Redis Caching</param>
        public ApplicationServices(WideWorldImportersContext dbContext, IMapper autoMapper, 
            IMemoryCache memoryCache, IRedisService redisService)
        {
            DbContext = dbContext;
            AutoMapper = autoMapper;
            MemoryCache = memoryCache;
            RedisService = redisService;
        }

    }
}
