using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.ServiceCollections;

namespace WideWorldImporters.Services.Services.Base
{

    /// <summary>
    /// 
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
        /// Automapper
        /// </summary>
        protected IMapper AutoMapper { get; }

        /// <summary>
        /// Memory Caching
        /// </summary>
        protected IMemoryCache MemoryCache { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices"></param>
        public BaseService(ApplicationServices applicationServices)
        {
            AppServices = applicationServices;

            DbContext = applicationServices.DbContext;
            AutoMapper = applicationServices.AutoMapper;
            MemoryCache = applicationServices.MemoryCache;
        }

    }

}
