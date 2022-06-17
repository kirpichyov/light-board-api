using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Application.Abstractions.Factories;

public interface IGeneratedCodeFactory<out TCode>
    where TCode: CodeBase
{
    public TCode Create(string resetCode, string email, DateTime expirationDate);
}