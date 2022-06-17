using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class GeneratedCodesRepository : RepositoryBase<CodeBase, Guid>, IGeneratedCodesRepository
{
    public GeneratedCodesRepository(PostgreSqlContext context) : base(context)
    {
    }
    
    public async Task<TCodeType?> GetLastByEmail<TCodeType>(string email)
        where TCodeType : CodeBase
    {
        return await Context.GeneratedCodes
            .OfType<TCodeType>()
            .OrderBy(resetCodeEmails => resetCodeEmails.ExpirationDate)
            .LastOrDefaultAsync();
    }

    public async Task<TCodeType?> GetByResetCode<TCodeType>(string resetCode)
        where TCodeType : CodeBase
    {
        return await Context.GeneratedCodes
            .OfType<TCodeType>()
            .FirstOrDefaultAsync(resetCodeEmails => resetCodeEmails.ResetCode == resetCode);
    }

    public async Task<bool> IsExist<TCodeType>(string email)
        where TCodeType : CodeBase
    {
        return await Context.GeneratedCodes
            .OfType<TCodeType>()
            .AnyAsync(resetCodeEmail => resetCodeEmail.Email == email);
    }
}