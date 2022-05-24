namespace LightBoard.Domain.Entities.Auth;

public class UserSession
{
    public UserSession(User user, string key, DateTime createdAtUtc, DateTime expiresAtUtc)
    {
        UserId = user.Id;
        Key = key;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
    }
    
    public UserSession()
    {
    }

    public Guid UserId { get; init; }
    public string Key { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime ExpiresAtUtc { get; init; }
    public bool IsInvalidated { get; set; }
}