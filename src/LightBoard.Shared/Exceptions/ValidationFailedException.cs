namespace LightBoard.Shared.Exceptions;

public class ValidationFailedException : ExceptionBase
{
    public ValidationFailedException(string message) 
        : base(message, ExceptionIdentifiers.AppValidation)
    {
    }
}