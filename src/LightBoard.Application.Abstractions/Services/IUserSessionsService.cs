namespace LightBoard.Application.Abstractions.Services;

public interface IUserSessionsService
{
    Task DeleteBySessionKey(string sessionKey);
    Task Invalidate(Guid userId);
}