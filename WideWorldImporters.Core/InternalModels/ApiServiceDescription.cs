using System;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Core.InternalModels
{

    /// <summary>
    /// Describes a service
    /// </summary>
    public class ApiServiceDescription
    {

        /// <summary>
        /// Type
        /// </summary>
        public Type Implementation { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public Type Interface { get; set; }

        /// <summary>
        /// Describes the lifetime of the service
        /// </summary>
        public Lifetime Lifetime { get; set; }

    }
}
