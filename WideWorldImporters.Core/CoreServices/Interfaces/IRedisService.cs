using System.Threading.Tasks;

namespace WideWorldImporters.Core.CoreServices.Interfaces
{

    /// <summary>
    /// Interface for Redis Service
    /// </summary>
    public interface IRedisService
    {

        /// <summary>
        /// Sets an object of type T to a redis key
        /// </summary>
        /// <typeparam name="T">Type of object to set</typeparam>
        /// <param name="key">Key to access the object</param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value);

        /// <summary>
        /// Gets the data corresponding to a redis key
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="key">Key to access the object</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Returns true if an entry with specified redis key exists
        /// </summary>
        /// <param name="key">Key to verify</param>
        /// <returns></returns>
        bool Exist(string key);

        /// <summary>
        /// Deletes a specified redis key asynchronisly
        /// </summary>
        /// <param name="key">Key to delete</param>
        /// <returns></returns>
        Task DeleteAsync(string key);

        /// <summary>
        /// Deletes everything from the redis database
        /// </summary>
        /// <returns></returns>
        Task DeleteAllAsync();

    }

}
