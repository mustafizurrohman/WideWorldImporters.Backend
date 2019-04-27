using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WideWorldImporters.Models.Database;

namespace WideWorldImporters.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        WideWorldImportersContext _dbContext;

        public ValuesController(WideWorldImportersContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("VehicleTemperatures")]
        public async Task<IActionResult> GetDataAsync()
        {
            var data = await _dbContext.VehicleTemperatures
                .OrderBy(x => x.RecordedWhen)
                .Skip(1000)
                .Take(2)
                .ToListAsync();
            
            return Ok(data);
        }

        #region -- Sample Methods -- 

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
