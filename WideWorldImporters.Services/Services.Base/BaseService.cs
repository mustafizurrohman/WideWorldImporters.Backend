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
        /// Application Services
        /// </summary>
        protected ApplicationServices AppServices { get; }

        /// <summary>
        /// Application Database context
        /// </summary>
        protected WideWorldImportersContext DbContext { get; }

        /// <summary>
        /// Authentication Provider DATA
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
        /// <param name="message"></param>
        public void Log(string message) => Task.Factory.StartNew(() => Logger.Log(message));

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="ex"></param>
        public void Log(Exception ex) => Task.Factory.StartNew(() => Logger.Log(ex));

        #endregion


    }

}
