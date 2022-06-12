using LightBoard.Domain.Entities.Auth;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IResetCodeEmailsRepository : IRepositoryBase<ResetPasswordCode, Guid>
{
    Task<ResetPasswordCode?> GetByEmail(string email);
    Task<ResetPasswordCode?> GetByResetCode(string resetCode);

    Task<bool> IsExist(string email);
}