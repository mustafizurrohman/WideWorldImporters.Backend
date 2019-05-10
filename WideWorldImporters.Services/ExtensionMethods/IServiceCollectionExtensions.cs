using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Core.ExtensionMethods;
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
        /// Register services for DI
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, string namespaceName = "WideWorldImporters")
        {

            var applicationAssemply = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(asm => asm.FullName.Contains(namespaceName));

            var apiServices = applicationAssemply
                .SelectMany(asm => asm.GetExportedTypes())
                .Where(asm => asm.Namespace.Contains(namespaceName))
                .Where(asm => !asm.GetInterfaces().IsEmpty())
                .Where(asm => asm.Assembly.GetName().Name != "mscorlib")
                .Where(asm => !asm.GetCustomAttributes(typeof(ServiceLifeTimeAttribute), true).IsEmpty())
                .Select(asm => new
                {
                    Implementation = asm,
                    Interface = asm.GetInterfaces().First(),
                    Lifetime = ((ServiceLifeTimeAttribute)asm.GetCustomAttributes(typeof(ServiceLifeTimeAttribute), true).Single()).GetLifetime()
                })
                .ToList();

            var singletonServices = apiServices.Where(reg => reg.Lifetime == Lifetime.Singleton);
            var transientServices = apiServices.Where(reg => reg.Lifetime == Lifetime.Transient);
            var scopedServices = apiServices.Where(reg => reg.Lifetime == Lifetime.Scoped);

            foreach (var singleton in singletonServices)
            {
                serviceCollection.AddSingleton(singleton.Interface, singleton.Implementation);
            }

            foreach (var transient in transientServices)
            {
                serviceCollection.AddTransient(transient.Interface, transient.Implementation);
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
    }

}
