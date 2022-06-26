using LightBoard.Application.Abstractions.Services;
using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.Shared.Exceptions;

namespace LightBoard.Application.Services;

public class UserSessionsService : IUserSessionsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserSessionsCache _sessionsCache;

    public UserSessionsService(IUnitOfWork unitOfWork, IUserSessionsCache sessionsCache)
    {
        _unitOfWork = unitOfWork;
        _sessionsCache = sessionsCache;
    }

    public async Task DeleteBySessionKey(string sessionKey)
    {
        var session = await _unitOfWork.UserSessions.GetBySessionKey(sessionKey);
        if (session is null)
        {
            throw new NotFoundException($"Session with key {sessionKey} not found.");
        }

        _unitOfWork.UserSessions.Delete(session);
        await _unitOfWork.SaveChangesAsync();
        await _sessionsCache.RemoveAsync(session.Id);
    }

    public async Task Invalidate(Guid userId)
    {
        var userSessions = await _unitOfWork.UserSessions.GetAllByUserId(userId);

        _unitOfWork.UserSessions.Delete(userSessions.ToArray());
        await _unitOfWork.SaveChangesAsync();
        
        foreach (var session in userSessions)
        {
            await _sessionsCache.RemoveAsync(session.Id);
        }
    }
}