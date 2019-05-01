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
        public ApplicationServices AppServices { get; }

        /// <summary>
        /// Application Database context
        /// </summary>
        public WideWorldImportersContext DbContext { get; }

        /// <summary>
        /// Automapper
        /// </summary>
        public IMapper AutoMapper { get; }

        /// <summary>
        /// Memory Caching
        /// </summary>
        public IMemoryCache MemoryCache { get; }

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
