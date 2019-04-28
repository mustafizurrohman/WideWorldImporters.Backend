using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Services.Interfaces;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Services.Services
{

    [ServiceLifeTime(Lifetime.Scoped)]
    public class SampleService : ISampleService
    {
        public string HelloWorld()
        {
            return "Hello World";
        }
    }

}
