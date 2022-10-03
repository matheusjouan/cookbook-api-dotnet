using Cookbook.Core.Excpetions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cookbook.API.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context) { }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var messages = context.ModelState
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var resultBadRequest = new 
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Some fields are invalid",
                Detail = messages
            };

            context.Result = new BadRequestObjectResult(resultBadRequest);
        }
    }
}
