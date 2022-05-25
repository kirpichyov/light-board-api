namespace LightBoard.Shared.Exceptions;

public class AccessDeniedException : ExceptionBase
{
    public AccessDeniedException(string message)
        : base(message, ExceptionIdentifiers.AppValidation)
    {
    }
}