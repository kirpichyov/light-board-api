using LightBoard.Domain.Entities.Auth;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IGeneratedCodesRepository : IRepositoryBase<CodeBase, Guid>
{
    Task<TCodeBase?> GetLastByEmail<TCodeBase>(string email)
        where TCodeBase : CodeBase;
    
    Task<TCodeBase?> GetByResetCode<TCodeBase>(string resetCode)
        where TCodeBase : CodeBase;

    Task<bool> IsExist<TCodeBase>(string email)
        where TCodeBase : CodeBase;
}