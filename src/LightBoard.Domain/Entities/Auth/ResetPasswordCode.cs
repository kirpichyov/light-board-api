namespace LightBoard.Domain.Entities.Auth;

public class ResetPasswordCode : EntityBase<Guid>
{
    public ResetPasswordCode(string resetCode, string email, DateTime expirationDate)
        : base(Guid.NewGuid())
    {
        ResetCode = resetCode;
        Email = email;
        ExpirationDate = expirationDate;
    }

    private ResetPasswordCode()
    {
    }
    
    public string ResetCode { get; }
    
    public string Email { get; }
    
    public DateTime ExpirationDate { get; }
}