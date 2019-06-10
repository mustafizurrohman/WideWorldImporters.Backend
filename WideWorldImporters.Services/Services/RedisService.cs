using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Services.Interfaces;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Services.Services
{

    /// <summary>
    /// Implementation of redis caching service as Singleton
    /// </summary>
    [ServiceLifeTime(Lifetime.Singleton)]
    public class RedisService : IRedisService
    {

        /// <summary>
        /// Instance of redis
        /// </summary>
        private IDistributedCache _redisCache { get; }

        /// <summary>
        /// JSON Serializer settings
        /// </summary>
        private JsonSerializerSettings _jsonSerializerSettings { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="distributedCache">Distributed caching (Redis)</param>
        public RedisService(IDistributedCache distributedCache)
        {
            _redisCache = distributedCache;

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        /// <summary>
        /// Sets an object of type T to a redis key
        /// </summary>
        /// <typeparam name="T">Type of object to set</typeparam>
        /// <param name="key">Key to access the object</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(string key, T value)
        {
            await _redisCache.SetStringAsync(key, JsonConvert.SerializeObject(value, _jsonSerializerSettings));
        }

        /// <summary>
        /// Gets the data corresponding to a redis key
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="key">Key to access the object</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _redisCache.GetStringAsync(key);
            return value == null
                    ? default
                    : JsonConvert.DeserializeObject<T>(value, _jsonSerializerSettings);
        }

        /// <summary>
        /// Returns true if an entry with specified redis key exists
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="key">Key to verify</param>
        /// <returns></returns>
        public async Task<bool> ExistAsync<T>(string key)
        {
            var value = await _redisCache.GetStringAsync(key);
            return value != null;
        }

        /// <summary>
        /// Deletes a specified redis key asynchronisly
        /// </summary>
        /// <param name="key">Key to delete</param>
        /// <returns></returns>
        public async Task DeleteAsync(string key)
        {
            await _redisCache.RemoveAsync(key);
        }
    }
}
