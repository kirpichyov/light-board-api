namespace LightBoard.Shared.Exceptions;

public class ValidationFailedException : ExceptionBase
{
    public const string ExceptionIdentifier = "APP_VALIDATION";
    
    public ValidationFailedException(string message) 
        : base(message, ExceptionIdentifier)
    {
    }
}