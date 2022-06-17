namespace LightBoard.Domain.Entities.Auth;

public class User : EntityBase<Guid>
{
    public User(string email, string name, string passwordHash)
        : base(Guid.NewGuid())
    {
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
    }

    private User()
    {
    }
    
    public string Email { get; }
    public string Name { get; }
    public bool IsEmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string AvatarUrl { get; set; }
    public string AvatarBlobName { get; set; }
}