namespace LightBoard.Domain.Entities.Auth;

public class ResetPasswordCode : CodeBase
{
    public ResetPasswordCode(string resetCode, string email, DateTime expirationDate) 
        : base(resetCode, email, expirationDate)
    {
    }
}