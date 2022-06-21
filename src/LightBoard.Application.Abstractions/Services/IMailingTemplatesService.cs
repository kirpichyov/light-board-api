namespace LightBoard.Application.Abstractions.Services;

public interface IMailingTemplatesService
{
    Task<string> GetResetPasswordCodeTemplate(string code);
    Task<string> GetConfirmEmailTemplate(string code, string username);
}