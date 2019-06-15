using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WideWorldImporters.API.Controllers.Base;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.ServiceCollections;

namespace WideWorldImporters.API.Controllers
{

    /// <summary>
    /// Controller for Redis
    /// </summary>
    public class RedisController : BaseAPIController
    {
        private readonly string _vehicleCacheKey = "VehicleTemperatures";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices">Application Services</param>
        public RedisController(ApplicationServices applicationServices)
            : base(applicationServices)
        {
        }

        /// <summary>
        /// Build the redis cache
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures/cache/build")]
        public async Task<IActionResult> BuildCache(int skip = 100, int take = 100)
        {
            IQueryable<VehicleTemperatures> data = DbContext.VehicleTemperatures.AsQueryable();

            var filteredData = await data.Skip(skip).Take(take).ToListAsync();

            await RedisService.SetAsync(_vehicleCacheKey, filteredData);

            return Ok(true);

        }

        /// <summary>
        /// Redis Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures/cache/get")]
        [Produces("application/json")]
        public async Task<IActionResult> GetDataRedisAsync(int take = 10, int skip = 10)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var temperatures = await RedisService.GetAsync<IEnumerable<VehicleTemperatures>>(_vehicleCacheKey);

            float size = JsonConvert.SerializeObject(temperatures).Length / (1024 * 1024);

            string sizeInMB = size.ToString() + " MB";

            stopwatch.Stop();

            long time1 = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            stopwatch.Start();

            var filtered = temperatures.Skip(skip).Take(take);

            stopwatch.Stop();

            long time2 = stopwatch.ElapsedMilliseconds;

            var results = new Tuple<IEnumerable<VehicleTemperatures>, long, long, string>(filtered, time1, time2, sizeInMB);

            return Ok(results);
        }

        /// <summary>
        /// Redis Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures/cache/clear")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteDataRedisAsync()
        {
            await RedisService.DeleteAsync(_vehicleCacheKey);
            await RedisService.DeleteAllAsync();

            return Ok(true);
        }

    }
}