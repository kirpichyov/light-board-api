using LightBoard.Application.Abstractions.Factories;
using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Application.Factories;

public class ConfirmEmailCodeFactory : IGeneratedCodeFactory<ConfirmEmailCode>
{
    public ConfirmEmailCode Create(string resetCode, string email, DateTime expirationDate)
    {
        return new ConfirmEmailCode(resetCode, email, expirationDate);
    }
}