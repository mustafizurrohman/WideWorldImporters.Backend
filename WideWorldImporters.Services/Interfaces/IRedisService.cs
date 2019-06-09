using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WideWorldImporters.Services.Interfaces
{

    /// <summary>
    /// Interface for Redis Service
    /// </summary>
    public interface IRedisService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistAsync<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task DeleteAsync(string key);

    }

}
