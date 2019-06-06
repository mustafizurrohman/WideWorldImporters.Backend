using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WideWorldImporters.API.Controllers.Base;
using WideWorldImporters.Core.ExtensionMethods;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.Interfaces;
using WideWorldImporters.Services.ServiceCollections;

namespace WideWorldImporters.API.Controllers
{

    /// <summary>
    /// Sample controller
    /// </summary>
    public class ValuesController : BaseAPIController
    {

        private readonly ISampleService _sampleService;
        private readonly IDistributedCache _redisCache;

        private readonly string _vehicleCacheKey = "VehicleTemperatures";

        private readonly DistributedCacheEntryOptions _cacheOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices"></param>
        /// <param name="sampleService"></param>
        /// <param name="distributedCache"></param>
        public ValuesController(ApplicationServices applicationServices, ISampleService sampleService, IDistributedCache distributedCache) : base(applicationServices)
        {
            this._sampleService = sampleService;
            this._redisCache = distributedCache;

            this._cacheOptions = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };

        }
        

        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("Test")]
        public IActionResult Test()
        {
            string data = _sampleService.HelloWorld();

            return Ok(data);
        }

        /// <summary>
        /// Db Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures")]
        [Produces("application/json")]
        public async Task<IActionResult> GetDataAsync()
        {
            DbSet<VehicleTemperatures> vehicleTemps = AppServices.DbContext.VehicleTemperatures;

            var data = await DbContext.VehicleTemperatures
                .OrderBy(x => x.RecordedWhen)
                .Skip(1000)
                .Take(2)
                .ToListAsync();
            
            return Ok(data);
        }

        /// <summary>
        /// Db Test using service
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures/service")]
        public async Task<IActionResult> GetDataFromServiceAsync()
        {

            var data = await _sampleService.GetVehicleTempsAsync();

            return Ok(data);
        }

        /// <summary>
        /// Redis Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures/cache")]
        [Produces("application/json")]
        public async Task<IActionResult> GetDataRedisAsync()
        {
            var data = await _redisCache.GetAsync(_vehicleCacheKey);

            var temperatures = JsonConvert.DeserializeObject<List<VehicleTemperatures>>(Encoding.UTF8.GetString(data));

            return Ok(temperatures);
        }

        /// <summary>
        /// Redis Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures/cache/clear")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteDataRedisAsync()
        {
            await _redisCache.RemoveAsync(_vehicleCacheKey);

            return Ok(true);
        }


        /// <summary>
        /// Build the redis cache
        /// </summary>
        /// <returns></returns>
        [HttpGet("buildcache")]
        public async Task<IActionResult> BuildCache()
        {
            List<VehicleTemperatures> data = await DbContext.VehicleTemperatures.ToListAsync();

            var dataByteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

            await _redisCache.SetAsync(_vehicleCacheKey, dataByteArray, _cacheOptions);

            return Ok(true);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("exception")]
        public IActionResult NotImplementedFunction()
        {
            throw new NotImplementedException();
        }



        #region -- Sample Methods -- 

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// GET api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// POST api/values
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// PUT api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// DELETE api/values/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
