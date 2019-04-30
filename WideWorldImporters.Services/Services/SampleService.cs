using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.Interfaces;
using WideWorldImporters.Services.ServiceCollections;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Services.Services
{

    /// <summary>
    /// Sample Service for Testing
    /// </summary>
    [ServiceLifeTime(Lifetime.Transient)]
    public class SampleService : ISampleService
    {

        /// <summary>
        /// Application Services
        /// </summary>
        private ApplicationServices AppServices { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationServices"></param>
        public SampleService(ApplicationServices applicationServices)
        {
            AppServices = applicationServices;
        }

        /// <summary>
        /// Hello world
        /// </summary>
        /// <returns></returns>
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<VehicleTemperatures>> GetVehicleTempsAsync()
        {
            var vehicleTemps = AppServices.DbContext.VehicleTemperatures;

            var data = await AppServices.DbContext.VehicleTemperatures
                .OrderBy(x => x.RecordedWhen)
                .Skip(1000)
                .Take(2)
                .ToListAsync();

            return data;
        }
    }

}
