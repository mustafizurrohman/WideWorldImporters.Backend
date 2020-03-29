using Microsoft.AspNetCore.Mvc.Filters;

namespace WideWorldImporters.API.ActionFilters
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    public sealed class TestingAttribute : ActionFilterAttribute
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
            context.HttpContext.Response.Headers.Add("x-testing-info", "Testing only. Not for production.");
        }
    }
}
