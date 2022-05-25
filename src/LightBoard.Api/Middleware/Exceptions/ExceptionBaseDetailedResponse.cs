using LightBoard.Shared.Exceptions;

namespace LightBoard.Api.Middleware.Exceptions;

internal class ExceptionBaseDetailedResponse : ExceptionBaseResponse
{
    public Exception Details { get; }

    public ExceptionBaseDetailedResponse(ExceptionBase exception) 
        : base(exception)
    {
        Details = exception;
    }
}