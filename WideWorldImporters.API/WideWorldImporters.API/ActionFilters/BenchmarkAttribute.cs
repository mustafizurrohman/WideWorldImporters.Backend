using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace WideWorldImporters.API.ActionFilters
{
    /// <summary>
    /// Benchmark attribute
    /// </summary>
    public class BenchmarkAttribute : ActionFilterAttribute
    {

        private Stopwatch _timer = new Stopwatch();

        
        /// <summary>
        /// Executed before the start of execution
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _timer = Stopwatch.StartNew();
        }

        /// <summary>
        /// Executed after the end of execution
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _timer.Stop();

            context.HttpContext.Response.Headers.Add("x-response-time", _timer.ElapsedMilliseconds + " ms");
        }

    }

}
