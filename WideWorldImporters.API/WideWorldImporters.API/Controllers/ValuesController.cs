using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WideWorldImporters.API.Controllers.Base;
using WideWorldImporters.Models.Database;

namespace WideWorldImporters.API.Controllers
{

    /// <summary>
    /// Sample controller
    /// </summary>

    public class ValuesController : BaseAPIController
    {
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public ValuesController(WideWorldImportersContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        [HttpGet("VehicleTemperatures")]
        public async Task<IActionResult> GetDataAsync()
        {
            var data = await ApplicationDbContext.VehicleTemperatures
                .OrderBy(x => x.RecordedWhen)
                .Skip(1000)
                .Take(2)
                .ToListAsync();
            
            return Ok(data);
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
