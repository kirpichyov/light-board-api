using FluentValidation;
using LightBoard.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LightBoard.Api.Middleware.Exceptions;

public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly bool _exposeExceptionDetails; 
    
    public ExceptionFilter(IHostEnvironment environment)
    {
        _exposeExceptionDetails = environment.IsDevelopment();
    }
    
    public override void OnException(ExceptionContext context)
    {
        HandleExceptionAsync(context);
        context.ExceptionHandled = true;
    }
    
    private void HandleExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;

        switch (exception)
        {
            case NotFoundException:
                SetExceptionResult(context, StatusCodes.Status404NotFound);
                break;
            case ValidationFailedException:
                SetExceptionResult(context, StatusCodes.Status400BadRequest);
                break;
            case AccessDeniedException:
                SetExceptionResult(context, StatusCodes.Status403Forbidden);
                break;
            case ValidationException:
                HandleFluentValidationException(context);
                break;
            default:
                SetExceptionResult(context, StatusCodes.Status500InternalServerError);
                break;
        }
    }
    
    private void SetExceptionResult(ExceptionContext context, int code)
    {
        Exception exception = context.Exception;
        object responseModel;

        if (string.IsNullOrEmpty(exception.Message))
        {
            context.Result = new StatusCodeResult(code);
            return;
        }

        if (exception is ExceptionBase exceptionBase)
        {
            responseModel = _exposeExceptionDetails
                ? new ExceptionBaseDetailedResponse(exceptionBase)
                : new ExceptionBaseResponse(exceptionBase);
        }
        else
        {
            responseModel = _exposeExceptionDetails
                ? new GenericExceptionDetailedResponse(exception)
                : new GenericExceptionResponse();
        }

        context.Result = new JsonResult(responseModel)
        {
            StatusCode = code
        };
    }
    
    private static void HandleFluentValidationException(ExceptionContext context)
    {
        ValidationException exception = (ValidationException)context.Exception;

        FluentValidationResponse.ErrorNode[] errorNodes =
            exception.Errors.GroupBy(error => error.PropertyName, error => error.ErrorMessage)
                .Select(group => new FluentValidationResponse.ErrorNode(group.Key, group.ToArray()))
                .ToArray();

        context.Result = new JsonResult(new FluentValidationResponse(errorNodes))
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}