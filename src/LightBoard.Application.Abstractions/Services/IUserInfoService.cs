namespace LightBoard.Application.Abstractions.Services;

public interface IUserInfoService
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
}