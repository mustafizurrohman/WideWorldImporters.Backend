using System;
using System.Collections.Generic;
using System.Text;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Core.ClassAttributes
{

    /// <summary>
    /// ServiceLifeTimeAttribute- Used to determine how a service should be injected.
    /// Can be used only for classes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceLifeTimeAttribute : Attribute
    {

        private readonly Lifetime _lifetime;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lifetime"></param>
        public ServiceLifeTimeAttribute(Lifetime lifetime)
        {
            _lifetime = lifetime;
        }

        /// <summary>
        /// Gets the lifetime of current attribute
        /// </summary>
        /// <returns></returns>
        public Lifetime GetLifetime()
        {
            return _lifetime;
        }

    }

}
