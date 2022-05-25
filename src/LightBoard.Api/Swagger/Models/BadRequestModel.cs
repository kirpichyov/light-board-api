using LightBoard.Api.Middleware.Exceptions;

namespace LightBoard.Api.Swagger.Models;

internal class BadRequestModel : FluentValidationResponse
{
    public BadRequestModel() 
        : base(Array.Empty<ErrorNode>())
    {
    }
}