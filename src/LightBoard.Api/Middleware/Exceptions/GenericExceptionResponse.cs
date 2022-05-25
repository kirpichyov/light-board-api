using LightBoard.Shared.Exceptions;

namespace LightBoard.Api.Middleware.Exceptions;

internal class GenericExceptionResponse : ExceptionResponse<string>
{
    public GenericExceptionResponse()
        : base(ExceptionIdentifiers.Generic, "Unexpected error occured")
    {
    }
}