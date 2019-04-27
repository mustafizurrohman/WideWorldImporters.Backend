using Microsoft.AspNetCore.Mvc;
using WideWorldImporters.Models.Database;

namespace WideWorldImporters.API.Controllers.Base
{

    /// <summary>
    /// Base Controller for API Controllers
    /// </summary>
    [Route("api/[controller]")]
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
