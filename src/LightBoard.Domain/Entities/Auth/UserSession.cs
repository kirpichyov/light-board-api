using LightBoard.Domain.Contracts;

namespace LightBoard.Domain.Entities.Auth;

public class UserSession : EntityBase<string>, IRedisKeyPart<string>
{
    public UserSession(User user, string key, DateTime createdAtUtc, DateTime expiresAtUtc)
        : base(key)
    {
        UserId = user.Id;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
    }
    
    public UserSession()
    {
    }

    public Guid UserId { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime ExpiresAtUtc { get; init; }

    public string RedisKeyPart => Id;
}