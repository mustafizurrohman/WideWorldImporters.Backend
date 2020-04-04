using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
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
        protected IRedisService RedisService { get; }

        /// <summary>
        /// Logging service
        /// </summary>
        protected AppLoggers Logger { get; }

        /// <summary>
        /// The console logger.
        /// </summary>
        protected ConsoleLogger ConsoleLogger { get; }

        /// <summary>
        /// The file logger.
        /// </summary>
        protected NLogFileLogger FileLogger { get; }

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
            ConsoleLogger = applicationServices.Logger.ConsoleLogger;
            FileLogger = applicationServices.Logger.FileLogger;
        }

        #endregion

        #region -- Utility Logging Methods -- 

        /// <summary>
        /// Logs a message
        /// </summary>
        /// <param name="message"></param>
        [NonAction]
        public void Log(string message) => Logger.LogInfo(message);

        /// <summary>
        /// Logs an exception
        /// </summary>
        /// <param name="ex"></param>
        [NonAction]
        public void Log(Exception ex) => Logger.Log(ex);

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The Debug message to log.</param>
        [NonAction]
        public void LogDebug(string message) => Logger.LogDebug(message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The Debug message to log.</param>
        [NonAction]
        public void LogError(string message) => Logger.LogError(message);

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="ex"></param>
        [NonAction]
        public void LogException(Exception ex) => Logger.LogException(ex);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        [NonAction]
        public void LogInfo(string message) => Logger.LogInfo(message);

        /// <summary>
        /// Logs an warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        [NonAction]
        public void LogWarn(string message) => Logger.LogWarn(message);

        #endregion

    }
}
