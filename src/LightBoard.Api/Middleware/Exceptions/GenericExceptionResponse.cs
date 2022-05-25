using LightBoard.Shared.Exceptions;

namespace LightBoard.Api.Middleware.Exceptions;

internal class GenericExceptionResponse : ExceptionResponse<string>
{
    public GenericExceptionResponse(Exception exception)
        : base(ExceptionIdentifiers.Generic, exception.Message)
    {
    }
}