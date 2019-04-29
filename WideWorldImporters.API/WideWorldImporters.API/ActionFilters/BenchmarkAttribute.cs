using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WideWorldImporters.API.ActionFilters
{
    /// <summary>
    /// Benchmark attribute
    /// </summary>
    public class BenchmarkAttribute : ActionFilterAttribute
    {

        private Stopwatch _timer = new Stopwatch();

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _timer = Stopwatch.StartNew();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _timer.Stop();

            context.HttpContext.Response.Headers.Add("x-response-time", _timer.ElapsedMilliseconds + " ms");
        }

    }

}
