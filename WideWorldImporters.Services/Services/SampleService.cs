using System;
using System.Collections.Generic;
using System.Text;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Services.Interfaces;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Services.Services
{

    /// <summary>
    /// Sample Service for Testing
    /// </summary>
    [ServiceLifeTime(Lifetime.Singleton)]
    public class SampleService : ISampleService
    {

        /// <summary>
        /// Hello world
        /// </summary>
        /// <returns></returns>
        public string HelloWorld()
        {
            return "Hello World";
        }
    }

}
