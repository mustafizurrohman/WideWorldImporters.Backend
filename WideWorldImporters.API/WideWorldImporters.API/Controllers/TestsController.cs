﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WideWorldImporters.API.Controllers.Base;
using WideWorldImporters.Core.Helpers;
using WideWorldImporters.Services.Interfaces;
using WideWorldImporters.Services.ServiceCollections;
using WideWorldImporters.Core.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using WideWorldImporters.Models.Database;

namespace WideWorldImporters.API.Controllers
{

    /// <summary>
    /// Sample controller
    /// </summary>
    public class TestsController : BaseAPIController
    {

        private readonly ISampleService _sampleService;
        

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices"></param>
        /// <param name="sampleService"></param>
        public TestsController(ApplicationServices applicationServices, ISampleService sampleService) 
            : base(applicationServices)
        {
            _sampleService = sampleService;
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
        /// Log
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet("Log")]
        public IActionResult LogMessage(string message)
        { 
            
            // BackgroundJob.Enqueue(() => Logger.Log(message));
            Logger.Log(message);
            Logger.LogDebug(message);
            Logger.LogError(message);
            Logger.LogException(new Exception(message));
            Logger.LogInfo(message);
            Logger.LogWarn(message);


            return Ok();
        }

        /// <summary>
        /// Db Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures/odata")]
        [Produces("application/json")]
        public IActionResult GetDataAsync()
        {
            // DbSet<VehicleTemperatures> vehicleTemps = AppServices.DbContext.VehicleTemperatures;

            var data = DbContext.VehicleTemperatures.Take(1000);
            
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("performance")]
        public IActionResult SortPerformance()
        {
            List<int> numbers = Enumerable.Range(0, 10000000)
                    .Select(x => IntHelpers.GetRandomNumber(int.MaxValue))
                    .ToList();

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            numbers.Sort();

            stopwatch.Stop();

            return Ok(numbers);

        }

        /// <summary>
        /// Tests if exceptions are logged properly
        /// </summary>
        /// <returns></returns>
        [HttpGet("exceptionLogging")]
        public IActionResult ExceptionLoggingTest(int number)
        {
            if (number % 2 == 0)
                throw new ArgumentException("Expected a odd number. " + number + " is even!");
            if (number %2 != 0)
                throw new ArgumentException("Expected a even number. " + number + " is odd!");

            throw new ArgumentException("Expected a positive number. " + number + " is negative!");

        }

        /// <summary>
        /// Array partitioning
        /// </summary>
        /// <returns></returns>
        [HttpGet("partition")]
        public IActionResult ListPartition(int number)
        {
            IEnumerable<int> list = Enumerable.Range(0, number);
                //.Select(num => IntHelpers.GetRandomNumber(number * 2));

            var partition = list.ToList().Partition().Partition();

            return Ok(partition);
        }

        /// <summary>
        /// IEnumerable shuffling
        /// </summary>
        /// <returns></returns>
        [HttpGet("shuffle")]
        public IActionResult ShuffleIEnumerable(int number)
        {
            IEnumerable<int> list = Enumerable.Range(0, number)
                .Select(num => IntHelpers.GetRandomNumber(number * 2));

            var shuffled = list.Shuffle();


            return Ok(new Tuple<IEnumerable<int>, IEnumerable<int>>(list, shuffled));
        }

        /// <summary>
        /// IQueryable Chunking
        /// </summary>
        /// <returns></returns>
        [HttpGet("chunk")]
        public async Task<IActionResult> ChunkIQueryableAsync(int total = 1000, int chunkSize = 50)
        {
            var chunkedQuery = DbContext.Colors.Take(total).Chunk(chunkSize);

            List<List<Colors>> colors = new List<List<Colors>>();

            // await foreach (var name in GetNamesAsync())

            //await foreach (var query.ToListAsync() in chunkedQuery)
            //{
            //    await query.ToListAsync()
            //}

            return Ok(true);
        }

        /// <summary>
        /// SQL Query Generation test
        /// </summary>
        /// <returns></returns>
        [HttpGet("sql")]
        public IActionResult SqlQuery()
        {
            var testQuery = DbContext.Colors.Take(10).Skip(10);

            return Ok(testQuery.ToSql());
        }

        /// <summary>
        /// Tests the skip.
        /// </summary>
        /// <returns></returns>
        [HttpGet("skip")]
        public async Task<IActionResult> TestSkip()
        {
            var count = await DbContext.Colors.CountAsync();

            var colors = DbContext.Colors.AsNonTrackingQueryable();

            var skipresult1 = colors.SmartTake(count / 2);
            var skipresult2 = colors.SmartTake(count * 2);
            var skipresult3 = colors.SmartTake(count + 2);
            var skipresult4 = colors.SmartTake(count - 2);
            var skipresult5 = colors.SmartTake(count);

            List<bool> response = new List<bool>
            {
                skipresult1.Item2,
                skipresult2.Item2, 
                skipresult3.Item2,
                skipresult4.Item2,
                skipresult5.Item2
            };

            return Ok(response);
            
        
        }


        #region -- Sample Methods -- 

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
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
