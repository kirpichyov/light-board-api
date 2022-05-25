using LightBoard.Shared.Exceptions;

namespace LightBoard.Api.Middleware.Exceptions;

internal class FluentValidationResponse : ExceptionResponse<FluentValidationResponse.ErrorNode>
{
    public FluentValidationResponse(ErrorNode[] errorNodes)
        : base(ExceptionIdentifiers.ModeValidation, errorNodes)
    {
    }
    
    internal class ErrorNode
    {
        public string Property { get; }
        public string[] Messages { get; }
        
        public ErrorNode(string property, string[] messages)
        {
            Property = property;
            Messages = messages;
        }
    }
}