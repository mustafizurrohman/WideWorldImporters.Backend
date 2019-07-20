using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WideWorldImporters.API.ActionFilters
{

    /// <summary>
    /// Validate Model Attribute
    /// Not used as the moment as it is done by default by WebApi 2.2.
    /// Should be enabled only when required
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// Override of OnActionExecuting method. 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if model state is invalid
            if (!context.ModelState.IsValid)
            {
                var modelErrors = context.ModelState
                    .ToDictionary(
                        key => key.Key,
                        detail => detail.Value.Errors.Select(err => err.Exception.Message).ToList()
                    ).ToList();

                context.Result = new BadRequestObjectResult(modelErrors);
            }
        }

    }

}
