using Microsoft.AspNetCore.Mvc.Filters;

namespace WideWorldImporters.API.ActionFilters
{
    /// <summary>
    /// Adds a header to indicate that this API must not be available in production. 
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
            context.HttpContext.Response.Headers.Add("x-warning", "Insecure method. NOT FOR PRODUCTION. Testing only.");
        }
    }
}
