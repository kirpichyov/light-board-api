namespace LightBoard.Domain.Entities.Auth;

public abstract class CodeBase : EntityBase<Guid>
{
    protected CodeBase(string resetCode, string email, DateTime expirationDate)
        : base(Guid.NewGuid())
    {
        ResetCode = resetCode;
        Email = email;
        ExpirationDate = expirationDate;
    }

    private CodeBase()
    {
        
    }

    public string ResetCode { get; }
    
    public string Email { get; }
    
    public DateTime ExpirationDate { get; }
}