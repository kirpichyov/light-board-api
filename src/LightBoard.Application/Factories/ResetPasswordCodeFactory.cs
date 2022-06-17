using LightBoard.Application.Abstractions.Factories;
using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Application.Factories;

public class ResetPasswordCodeFactory : IGeneratedCodeFactory<ResetPasswordCode>
{
    public ResetPasswordCode Create(string resetCode, string email, DateTime expirationDate)
    {
        return new ResetPasswordCode(resetCode, email, expirationDate);
    }
}