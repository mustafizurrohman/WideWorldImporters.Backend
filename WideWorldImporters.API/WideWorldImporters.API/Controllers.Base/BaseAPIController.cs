using Microsoft.AspNetCore.Mvc;
using WideWorldImporters.API.ActionFilters;
using WideWorldImporters.Models.Database;

namespace WideWorldImporters.API.Controllers.Base
{

    /// <summary>
    /// Base Controller for API Controllers
    /// </summary>
    [Route("api/[controller]")]
    [Benchmark]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {

        /// <summary>
        /// Databse context
        /// </summary>
        public WideWorldImportersContext ApplicationDbContext { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public BaseAPIController(WideWorldImportersContext context)
        {
            ApplicationDbContext = context;
        }

    }
}
