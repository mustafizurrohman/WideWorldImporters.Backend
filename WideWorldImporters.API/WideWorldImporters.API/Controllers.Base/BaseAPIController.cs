using Microsoft.AspNetCore.Mvc;
using WideWorldImporters.API.ActionFilters;
using WideWorldImporters.Models.Database;
using WideWorldImporters.Services.ServiceCollections;

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
        /// Application Services
        /// </summary>
        public ApplicationServices AppServices { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationServices"></param>
        public BaseAPIController(ApplicationServices applicationServices)
        {
            AppServices = applicationServices;
        }

    }
}
