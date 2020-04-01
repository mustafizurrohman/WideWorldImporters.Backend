using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using WideWorldImporters.AuthenticationProvider.Database;
using WideWorldImporters.Core.CoreServices.Interfaces;
using WideWorldImporters.Logger.Implementation;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.ServiceCollections;

namespace WideWorldImporters.Services.Services.Base
{

    /// <summary>
    /// Base classes for all services
    /// </summary>
    public abstract class BaseService
    {

        /// <summary>
        /// Application Services Collection
        /// </summary>
        private ApplicationServices AppServices { get; }

        /// <summary>
        /// Application Database Db Context
        /// </summary>
        protected WideWorldImportersContext DbContext { get; }

        /// <summary>
        /// Authentication Provider Db Context
        /// </summary>
        protected AuthenticationProviderContext AuthDbContext { get; }

        /// <summary>
        /// AutoMapper
        /// </summary>
        protected IMapper AutoMapper { get; }

        /// <summary>
        /// Memory Caching
        /// </summary>
        protected IMemoryCache MemoryCache { get; }

        /// <summary>
        /// Redis Cache Service
        /// </summary>
        public IRedisService RedisService { get; }

        /// <summary>
        /// Logging service
        /// </summary>
        protected AppLoggers Logger { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices"></param>
        protected BaseService(ApplicationServices applicationServices)
        {
            AppServices = applicationServices;

            DbContext = applicationServices.DbContext;
            AuthDbContext = applicationServices.AuthDbContext;
            AutoMapper = applicationServices.AutoMapper;
            MemoryCache = applicationServices.MemoryCache;
            Logger = applicationServices.Logger;
            RedisService = applicationServices.RedisService;
        }

        #region -- Utility Functions --

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message) => Task.Factory.StartNew(() => Logger.Log(message));

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public void Log(Exception ex) => Task.Factory.StartNew(() => Logger.Log(ex));

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The Debug message to log.</param>
        public void LogDebug(string message) => Task.Run(() => Logger.LogDebug(message));

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The Debug message to log.</param>
        public void LogError(string message) => Task.Run(() => Logger.LogError(message));

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="ex"></param>
        public void LogException(Exception ex) => Task.Run(() => Logger.LogException(ex));

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        public void LogInfo(string message) => Task.Run(() => Logger.LogInfo(message));

        /// <summary>
        /// Logs an warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void LogWarn(string message) => Task.Run(() => Logger.LogWarn(message));

        #endregion


    }

}
