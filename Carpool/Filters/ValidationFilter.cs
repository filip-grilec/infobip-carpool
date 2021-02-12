using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpool.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Carpool.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorResponse = AddModelErrorsToResponse(context);

                context.Result = new BadRequestObjectResult(errorResponse);
                
                return;
            }

            await next();
        }

        private static ErrorResponse AddModelErrorsToResponse(ActionExecutingContext context)
        {
            var errorsInModelState = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp
                    => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

            var errorResponse = AddErrorsToResponse(errorsInModelState);

            return errorResponse;
        }

        private static ErrorResponse AddErrorsToResponse(KeyValuePair<string, IEnumerable<string>>[] errorsInModelState)
        {
            var errorResponse = new ErrorResponse();

            foreach (var error in errorsInModelState)
            {
                foreach (var subError in error.Value)
                {
                    var errorModel = new ErrorModel
                    {
                        FiledName = error.Key,
                        Message = subError
                    };

                    errorResponse.Errors.Add(errorModel);
                }
            }

            return errorResponse;
        }
    }
}