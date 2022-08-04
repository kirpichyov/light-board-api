namespace LightBoard.Application.Abstractions.Services;

public interface IHtmlSanitizerService
{
    string Sanitize(string value);
}