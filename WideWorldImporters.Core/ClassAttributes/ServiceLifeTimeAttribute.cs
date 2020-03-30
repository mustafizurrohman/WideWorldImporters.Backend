using System;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Core.ClassAttributes
{

    /// <summary>
    /// ServiceLifeTimeAttribute- Used to determine how a service should be injected.
    /// Can be used only for classes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ServiceLifeTime : Attribute
    {

        /// <summary>
        /// Lifetime (value). 
        /// Readonly means that it can set only in the constructor
        /// </summary>
        private readonly Lifetime _lifetime;

        /// <summary>
        /// Gets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public Lifetime Lifetime => _lifetime;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lifetime">Lifetime attribute</param>
        public ServiceLifeTime(Lifetime lifetime)
        {
            _lifetime = lifetime;
        }

        /// <summary>
        /// Gets the Lifetime of current attribute
        /// </summary>
        /// <returns></returns>
        public Lifetime GetLifetime()
        {
            return _lifetime;
        }

    }

}
