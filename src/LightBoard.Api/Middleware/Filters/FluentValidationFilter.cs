using LightBoard.Api.Middleware.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LightBoard.Api.Middleware.Filters;

public class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            FluentValidationResponse.ErrorNode[] errorNodes =
                context.ModelState.Where(modelState => modelState.Value.Errors.Any())
                    .Select(pair => new FluentValidationResponse.ErrorNode(pair.Key, pair.Value.Errors.Select(error => error.ErrorMessage).ToArray()))
                    .ToArray();

            context.Result = new JsonResult(new FluentValidationResponse(errorNodes))
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            return;
        }

        await next();
    }
}