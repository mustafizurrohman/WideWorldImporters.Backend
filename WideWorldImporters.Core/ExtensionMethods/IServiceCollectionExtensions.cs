using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WideWorldImporters.Core.ClassAttributes;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Core.ExtensionMethods
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
        public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection, string namespaceName = "Services")
        {

            var applicationAssemply = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(asm => asm.FullName.Contains(namespaceName))
                .FirstOrDefault();

            var apiServices = applicationAssemply.GetExportedTypes()
                // .SelectMany(asm => asm.GetExportedTypes())
                .Where(asm => asm.Namespace.Contains(namespaceName))
                .Where(asm => !asm.GetInterfaces().IsEmpty())
                .Where(asm => asm.Assembly.GetName().Name != "mscorlib")
                .Where(asm => !asm.GetCustomAttributes(typeof(ServiceLifeTimeAttribute), true).IsEmpty())
                .Select(asm => new
                {
                    Implementation = asm,
                    Interface = asm.GetInterfaces().First(),
                    Lifetime = ((ServiceLifeTimeAttribute)asm.GetCustomAttributes(typeof(ServiceLifeTimeAttribute), true).First()).GetLifetime()
                })
                .ToList();

            var singletonServices = apiServices.Where(reg => reg.Lifetime == Lifetime.Singleton);
            var transientServices = apiServices.Where(reg => reg.Lifetime == Lifetime.Transient);
            var scopedServices = apiServices.Where(reg => reg.Lifetime == Lifetime.Scoped);

            foreach(var singleton in singletonServices)
            {
                serviceCollection.AddSingleton(singleton.Interface, singleton.Implementation);
            }

            foreach (var transient in transientServices)
            {
                serviceCollection.AddTransient(transient.Interface, transient.Implementation);
            }

            return serviceCollection;
        }
    }

}
