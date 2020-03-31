using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WideWorldImporters.Core.ClassAttributes;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.Interfaces;
using WideWorldImporters.Services.ServiceCollections;
using WideWorldImporters.Services.Services.Base;
using static WideWorldImporters.Core.Enumerations.ServiceLifetime;

namespace WideWorldImporters.Services.Services
{

    /// <summary>
    /// Sample Service for Testing
    /// </summary>
    [Service(Lifetime.Transient)]
    public class SampleService : BaseService, ISampleService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationServices"></param>
        public SampleService(ApplicationServices applicationServices) : base(applicationServices)
        {
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
            var _ = DbContext.VehicleTemperatures;

            var data = await DbContext.VehicleTemperatures
                .OrderBy(x => x.RecordedWhen)
                .Skip(1000)
                .Take(2)
                .ToListAsync();

            return data;
        }
    }

}
