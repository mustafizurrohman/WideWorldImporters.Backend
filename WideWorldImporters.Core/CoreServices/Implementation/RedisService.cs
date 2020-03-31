using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Core.CoreServices.Interfaces;
using WideWorldImporters.Core.ExtensionMethods;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Core.CoreServices.Implementation
{
    /// <summary>
    /// Implementation of redis caching service (Singleton)
    /// </summary>
    [Service(Lifetime.Singleton)]
    public class RedisService : IRedisService
    {

        /// <summary>
        /// Instance of redis
        /// </summary>
        private IDistributedCache RedisCache { get; }

        /// <summary>
        /// JSON Serializer settings
        /// </summary>
        private JsonSerializerSettings JsonSerializerSettings { get; }

        /// <summary>
        /// List of keys
        /// </summary>
        private List<string> StoredKeys { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="distributedCache">Distributed caching (Redis)</param>
        public RedisService(IDistributedCache distributedCache)
        {
            RedisCache = distributedCache;

            JsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            StoredKeys = new List<string>();
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
            await RedisCache.SetStringAsync(key, JsonConvert.SerializeObject(value, JsonSerializerSettings));
            ManageKeys(key);
        }

        /// <summary>
        /// Gets the data corresponding to a redis key
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="key">Key to access the object</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var value = await RedisCache.GetStringAsync(key);
            return value == null
                    ? default
                    : JsonConvert.DeserializeObject<T>(value, JsonSerializerSettings);
        }

        /// <summary>
        /// Returns true if an entry with specified redis key exists
        /// </summary>
        /// <param name="key">Key to verify</param>
        /// <returns></returns>
        public bool Exist(string key)
        {
            // var value = await _redisCache.GetStringAsync(key);
            // return value != null;
            return StoredKeys.Contains(key);
        }

        /// <summary>
        /// Deletes a specified redis key asynchronously
        /// </summary>
        /// <param name="key">Key to delete</param>
        /// <returns></returns>
        public async Task DeleteAsync(string key)
        {
            await RedisCache.RemoveAsync(key);
            ManageKeys(key, true);
        }

        /// <summary>
        /// Deletes everything from the redis database
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAllAsync()
        {
            var allKeys = GetKeys();

            if (allKeys.IsEmpty())
            {
                return;
            }

            // Delete each key one by one
            foreach (var key in allKeys)
            {
                await DeleteAsync(key);
            }
        }

        #region -- Private Methods --

        private void ManageKeys(string key, bool delete = false)
        {
            if (delete)
            {
                if (StoredKeys.Contains(key))
                {
                    StoredKeys.Remove(key);
                }

                return;
            }

            StoredKeys.Add(key);
            StoredKeys = StoredKeys.Distinct().ToList();
        }

        private List<string> GetKeys()
        {
            return StoredKeys;
        }

        #endregion

    }

}
