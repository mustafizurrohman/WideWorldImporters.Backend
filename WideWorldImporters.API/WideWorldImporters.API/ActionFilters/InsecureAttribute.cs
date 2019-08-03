using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WideWorldImporters.API.ActionFilters
{
    /// <summary>
    /// 
    /// </summary>
    public class InsecureAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Executed before the start of execution
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }

        /// <summary>
        /// Executed after the end of execution
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Add("x-warning", "Insecure method. DO NOT USE IN PRODUCTION. Testing only.");
        }
    }
}
