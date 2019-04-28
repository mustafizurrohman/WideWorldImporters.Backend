using System;
using System.Collections.Generic;
using System.Text;

namespace WideWorldImporters.Core.Enumerations
{
    public class ServiceLifetime
    {
        public enum Lifetime
        {
            Singleton,
            Scoped,
            Transient
        };

    }
}
