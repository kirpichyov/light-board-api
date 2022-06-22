namespace LightBoard.Application.Abstractions.Services;

public interface IMailingTemplateService
{
    Task<string> GetResetPasswordCodeTemplate(string code);
    Task<string> GetConfirmEmailTemplate(string code, string username);
}