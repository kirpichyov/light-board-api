namespace LightBoard.Shared.Exceptions;

public class ExceptionBase : Exception
{
    public string Identifier { get; }

    public ExceptionBase(string message, string identifier) : base(message)
    {
        Identifier = identifier;
    }
}