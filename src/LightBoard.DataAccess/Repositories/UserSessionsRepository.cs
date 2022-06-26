using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Auth;
using LightBoard.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class UserSessionsRepository : RelationalRepositoryBase<UserSession, string>, IUserSessionsRepository
{
    public UserSessionsRepository(PostgreSqlContext context)
        : base(context)
    {
    }

    public async Task<UserSession?> GetBySessionKey(string sessionKey)
    {
        return await Context.UserSessions
            .AsNoTracking()
            .SingleOrDefaultAsync(cacheRecord => cacheRecord.Id == sessionKey);
    }

    public async Task<IReadOnlyCollection<UserSession>> GetAllByUserId(Guid userId)
    {
        return await Context.UserSessions
            .AsNoTracking()
            .Where(cacheRecord => cacheRecord.UserId == userId)
            .ToArrayAsync();
    }
}