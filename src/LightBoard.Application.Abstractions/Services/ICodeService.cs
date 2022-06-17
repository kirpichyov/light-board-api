using LightBoard.Domain.Entities.Auth;

namespace LightBoard.Application.Abstractions.Services;

public interface ICodeService
{
    public TCode GenerateCode<TCode>(int symbolCount, string email, int expirationDate)
        where TCode : CodeBase;
}