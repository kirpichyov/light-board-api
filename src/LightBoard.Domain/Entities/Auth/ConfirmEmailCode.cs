namespace LightBoard.Domain.Entities.Auth;

public class ConfirmEmailCode : CodeBase
{
    public ConfirmEmailCode(string resetCode, string email, DateTime expirationDate) 
        : base(resetCode, email, expirationDate)
    {
    }
}