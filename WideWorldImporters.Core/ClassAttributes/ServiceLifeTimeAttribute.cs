using System;
using System.Collections.Generic;
using System.Text;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Core.ClassAttributes
{

    /// <summary>
    /// ServiceLifeTimeAttribute- Used to determine how a service should be injected
    /// Can be used only for classes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceLifeTimeAttribute : Attribute
    {
        private readonly Lifetime _lifetime;

        public ServiceLifeTimeAttribute(Lifetime lifetime)
        {
            _lifetime = lifetime;
        }

    }

}
