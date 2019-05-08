using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WideWorldImporters.API.ActionFilters
{

    /// <summary>
    /// 
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
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
