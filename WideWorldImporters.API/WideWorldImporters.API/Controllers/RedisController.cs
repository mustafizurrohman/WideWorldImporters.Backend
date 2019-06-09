using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WideWorldImporters.API.Controllers.Base;
using WideWorldImporters.Core.Redis;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.ServiceCollections;

namespace WideWorldImporters.API.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class RedisController : BaseAPIController
    {
        private readonly string _vehicleCacheKey = "VehicleTemperatures";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationServices"></param>
        public RedisController(ApplicationServices applicationServices)
            : base(applicationServices)
        {
        }

        /// <summary>
        /// Build the redis cache
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures/cache/build")]
        public async Task<IActionResult> BuildCache()
        {
            List<VehicleTemperatures> data = await DbContext.VehicleTemperatures.ToListAsync();

            await RedisService.SetAsync(_vehicleCacheKey, data);

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

            var size = JsonConvert.SerializeObject(temperatures).Length / 1024 / 1024;

            stopwatch.Stop();

            long time1 = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            stopwatch.Start();

            var filtered = temperatures.Skip(skip).Take(take);

            stopwatch.Stop();

            long time2 = stopwatch.ElapsedMilliseconds;

            var results = new Tuple<IEnumerable<VehicleTemperatures>, long, long, int>(filtered, time1, time2, size);

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

            return Ok(true);
        }

    }
}