namespace LightBoard.Shared.Exceptions;

public class NotFoundException : ExceptionBase
{
    public const string ExceptionIdentifier = "DATASOURCE_VALIDATION";
    
    public NotFoundException(string message) 
        : base(message, ExceptionIdentifier)
    {
    }
}