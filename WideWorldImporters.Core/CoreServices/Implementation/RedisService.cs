using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// List of keys
        /// </summary>
        private List<string> _storedKeys { get; set; }

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

            _storedKeys = new List<string>();
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
            var value = await _redisCache.GetStringAsync(key);
            return value == null
                    ? default
                    : JsonConvert.DeserializeObject<T>(value, _jsonSerializerSettings);
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
            return _storedKeys.Contains(key);
        }

        /// <summary>
        /// Deletes a specified redis key asynchronisly
        /// </summary>
        /// <param name="key">Key to delete</param>
        /// <returns></returns>
        public async Task DeleteAsync(string key)
        {
            await _redisCache.RemoveAsync(key);
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
                if (_storedKeys.Contains(key))
                {
                    _storedKeys.Remove(key);
                }

                return;
            }

            _storedKeys.Add(key);
            _storedKeys = _storedKeys.Distinct().ToList();
        }

        private List<string> GetKeys()
        {
            return _storedKeys;
        }

        #endregion

    }

}
