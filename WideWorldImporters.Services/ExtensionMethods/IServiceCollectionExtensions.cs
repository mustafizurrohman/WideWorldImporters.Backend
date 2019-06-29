using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Core.InternalModels;
using WideWorldImporters.Services.Interfaces;
using WideWorldImporters.Services.ServiceCollections;
using WideWorldImporters.Services.Services;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Services.ExtensionMethods
{

    /// <summary>
    /// Extension methods for ServiceCollection
    /// </summary>
    public static class IServiceCollectionExtensions
    {

        /// <summary>
        /// Register services for Dependency Injection
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, string namespaceName = "WideWorldImporters")
        {
            var apiServices = GetAllServices(namespaceName);

            var singletonServices = apiServices.Where(reg => reg.Lifetime == Lifetime.Singleton);
            var transientServices = apiServices.Where(reg => reg.Lifetime == Lifetime.Transient);
            var scopedServices = apiServices.Where(reg => reg.Lifetime == Lifetime.Scoped);

            // Register transient services
            foreach (var transient in transientServices)
            {
                serviceCollection.AddTransient(transient.Interface, transient.Implementation);
            }

            // Register singleton services
            foreach (var singleton in singletonServices)
            {
                serviceCollection.AddSingleton(singleton.Interface, singleton.Implementation);
            }

            /*
            // Scoped services must be injected differently
            foreach (var scoped in scopedServices)
            {
                serviceCollection.AddScoped(scoped.Interface, scoped.Implementation);
            } 
            */
            


            serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            serviceCollection.AddMemoryCache();

            serviceCollection.AddTransient(typeof(ApplicationServices));

            return serviceCollection;
        }

        /// <summary>
        /// Gets all Singleton services
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        public static IEnumerable<ApiServiceDescription> GetSingletonServices(string namespaceName = "WideWorldImporters")
        {
            return GetServicesByType(Lifetime.Singleton, namespaceName);
        }

        /// <summary>
        /// Gets all Transient services
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        public static IEnumerable<ApiServiceDescription> GetTransientServices(string namespaceName = "WideWorldImporters")
        {
            return GetServicesByType(Lifetime.Transient, namespaceName);
        }

        /// <summary>
        /// Gets all Scoped services
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        public static IEnumerable<ApiServiceDescription> GetScopedServices(string namespaceName = "WideWorldImporters")
        {
            return GetServicesByType(Lifetime.Scoped, namespaceName);
        }


        #region -- Private Methods --

        /// <summary>
        /// Returns all services of a particular type
        /// </summary>
        /// <param name="namespaceName">Namespace name</param>
        /// <param name="lifetime">Type of service</param>
        /// <returns></returns>
        private static IEnumerable<ApiServiceDescription> GetServicesByType(Lifetime lifetime, string namespaceName = "WideWorldImporters")
        {
            IEnumerable<ApiServiceDescription> apiServiceDescriptions = GetAllServices(namespaceName);

            var servicesByType = apiServiceDescriptions.Where(svc => svc.Lifetime == lifetime);

            return servicesByType;
        }
        
        /// <summary>
        /// Retrives all services in a namespace
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        private static IEnumerable<ApiServiceDescription> GetAllServices(string namespaceName = "WideWorldImporters")
        {
            var applicationAssemply = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(asm => asm.FullName.Contains(namespaceName));

            var apiServices = applicationAssemply
                .SelectMany(asm => asm.GetExportedTypes())
                .Where(asm => asm.Namespace.Contains(namespaceName))
                .Where(asm => !asm.GetInterfaces().IsEmpty())
                .Where(asm => asm.Assembly.GetName().Name != "mscorlib")
                .Where(asm => !asm.GetCustomAttributes(typeof(ServiceLifeTime), true).IsEmpty())
                .Select(asm => new ApiServiceDescription()
                {
                    Implementation = asm,
                    Interface = asm.GetInterfaces().First(),
                    Lifetime = ((ServiceLifeTime)asm.GetCustomAttributes(typeof(ServiceLifeTime), true).Single()).GetLifetime()
                })
                .ToList();

            return apiServices;
        }

        #endregion

    }

}
