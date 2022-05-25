using LightBoard.Shared.Exceptions;

namespace LightBoard.Api.Middleware.Exceptions;

internal class ExceptionBaseResponse : ExceptionResponse<string>
{
    public ExceptionBaseResponse(ExceptionBase exception)
        : base(exception.Identifier, exception.Message)
    {
    }
}