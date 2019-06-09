using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WideWorldImporters.Core.Redis
{

    /// <summary>
    /// 
    /// </summary>
    public static class RedisCache
    {
        
        /// <summary>
        /// Set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value)
        {
            await cache.SetStringAsync(key, JsonConvert.SerializeObject(value));
        }
        
        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var value = await cache.GetStringAsync(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Verify if an object exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> ExistAsync<T>(this IDistributedCache cache, string key)
        {
            var value = await cache.GetStringAsync(key);
            return value == null ? false : true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task DeleteAsync(this IDistributedCache cache, string key)
        {
            await cache.RemoveAsync(key);
            return;
        }

    }
}
