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
    /// 
    /// </summary>
    [ServiceLifeTime(Lifetime.Singleton)]
    public class RedisService : IRedisService
    {
        private IDistributedCache _redisCache { get; }

        private JsonSerializerSettings _jsonSerializerSettings { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distributedCache"></param>
        public RedisService(IDistributedCache distributedCache)
        {
            _redisCache = distributedCache;

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync<T>(string key)
        {
            var value = await _redisCache.GetStringAsync(key);
            return value != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _redisCache.GetStringAsync(key);
            return value == null 
                    ? default 
                    : JsonConvert.DeserializeObject<T>(value, _jsonSerializerSettings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(string key, T value)
        {
            await _redisCache.SetStringAsync(key, JsonConvert.SerializeObject(value, _jsonSerializerSettings));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string key)
        {
            await _redisCache.RemoveAsync(key);
        }
    }
}
