namespace LightBoard.Api.Middleware.Exceptions;

internal class ExceptionResponse<TErrorNode>
{
    public string Reason { get; }
    public TErrorNode[] Errors { get; }

    public ExceptionResponse(string reason, TErrorNode[] errors)
    {
        Reason = reason;
        Errors = errors;
    }

    public ExceptionResponse(string reason, TErrorNode error)
    {
        Reason = reason;
        Errors = new[] { error };
    }
}