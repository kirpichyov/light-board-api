namespace LightBoard.Api.Middleware.Exceptions;

internal class GenericExceptionDetailedResponse : GenericExceptionResponse
{
    public Exception Details { get; }

    public GenericExceptionDetailedResponse(Exception exception)
        : base(exception)
    {
        Details = exception;
    }
}