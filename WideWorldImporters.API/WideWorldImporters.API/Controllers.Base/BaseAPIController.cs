using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using WideWorldImporters.API.ActionFilters;
using WideWorldImporters.AuthenticationProvider.Database;
using WideWorldImporters.Core.CoreServices.Interfaces;
using WideWorldImporters.Logger.Implementation;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.ServiceCollections;

namespace WideWorldImporters.API.Controllers.Base
{

    /// <summary>
    /// Base Controller for API Controllers
    /// </summary>
    [Benchmark]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
    // ReSharper disable once InconsistentNaming
    public class BaseAPIController : ControllerBase
    {

        #region -- Services --

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
        public AppLoggers Logger { get; }

        #endregion

        #region -- Constructor --

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices">Collection of services frequently used in the application</param>
        public BaseAPIController(ApplicationServices applicationServices)
        {
            AppServices = applicationServices;

            DbContext = applicationServices.DbContext;
            AuthDbContext = applicationServices.AuthDbContext;
            AutoMapper = applicationServices.AutoMapper;
            MemoryCache = applicationServices.MemoryCache;
            RedisService = applicationServices.RedisService;
            Logger = applicationServices.Logger;
        }

        #endregion

        #region -- Utility Logging Methods -- 

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message"></param>
        [NonAction]
        public void Log(string message) => Task.Run(() => Logger.LogInfo(message));

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="ex"></param>
        [NonAction]
        public void Log(Exception ex) => Task.Run(() => Logger.Log(ex));

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The Debug message to log.</param>
        [NonAction]
        public void LogDebug(string message) => Task.Run(() => Logger.LogDebug(message));

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The Debug message to log.</param>
        [NonAction]
        public void LogError(string message) => Task.Run(() => Logger.LogError(message));

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="ex"></param>
        [NonAction]
        public void LogException(Exception ex) => Task.Run(() => Logger.LogException(ex));

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        [NonAction]
        public void LogInfo(string message) => Task.Run(() => Logger.LogInfo(message));

        /// <summary>
        /// Logs an warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        [NonAction]
        public void LogWarn(string message) => Task.Run(() => Logger.LogWarn(message));

        #endregion

    }
}
