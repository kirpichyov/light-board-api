using LightBoard.Application.Abstractions.Arguments;

namespace LightBoard.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendConfirmEmail(SendMailArgs args, string mailTemplate, string resetCode, string username);

    Task SendResetCode(SendMailArgs args, string mailTemplate, string resetCode);
}