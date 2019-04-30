using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Models.Database;

namespace WideWorldImporters.Services.ServiceCollections
{

    /// <summary>
    /// Services for the application
    /// </summary>
    public class ApplicationServices
    {
        /// <summary>
        /// Database context
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
        /// <param name="dbContext">Database context</param>
        /// <param name="autoMapper">Automapper</param>
        /// <param name="memoryCache">Memory Caching</param>
        public ApplicationServices(WideWorldImportersContext dbContext, IMapper autoMapper, IMemoryCache memoryCache)
        {
            DbContext = dbContext;
            AutoMapper = autoMapper;
            MemoryCache = memoryCache;
        }

    }
}
