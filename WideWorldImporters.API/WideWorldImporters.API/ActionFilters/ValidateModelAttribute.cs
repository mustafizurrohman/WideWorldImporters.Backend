using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace WideWorldImporters.API.ActionFilters
{

    /// <summary>
    /// Validate Model Attribute
    /// Not used as the moment as it is done by default by WebApi 2.2.
    /// Should be enabled only when required
    /// </summary>
    public sealed class ValidateModelAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// Overrude of OnActionExecuted Method.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// Override of OnActionExecuting Method. 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // If the model state is valid then just return
            if (context.ModelState.IsValid) return;

            var modelErrors = context.ModelState
                .ToDictionary(
                    key => key.Key,
                    detail => detail.Value.Errors.Select(err => err.Exception.Message).ToList()
                ).ToList();

            context.Result = new BadRequestObjectResult(modelErrors);
        }

    }

}
