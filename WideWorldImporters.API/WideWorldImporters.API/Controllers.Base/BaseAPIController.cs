﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using WideWorldImporters.API.ActionFilters;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.ServiceCollections;

namespace WideWorldImporters.API.Controllers.Base
{

    /// <summary>
    /// Base Controller for API Controllers
    /// </summary>
    [Route("api/[controller]")]
    [Benchmark]
    [ApiController]
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
        /// Automapper
        /// </summary>
        protected IMapper AutoMapper { get; }

        /// <summary>
        /// Memory Caching
        /// </summary>
        protected IMemoryCache MemoryCache { get; }

        /// <summary>
        /// Redis Cache
        /// </summary>
        public IDistributedCache RedisDistributedCache { get; }

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
            AutoMapper = applicationServices.AutoMapper;
            MemoryCache = applicationServices.MemoryCache;
            RedisDistributedCache = applicationServices.RedisDistributedCache;
        }

        #endregion

    }
}
