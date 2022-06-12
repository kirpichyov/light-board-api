using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace LightBoard.DataAccess.Repositories;

public class ResetCodeEmailsRepository : RepositoryBase<ResetPasswordCode, Guid>, IResetCodeEmailsRepository
{
    public ResetCodeEmailsRepository(PostgreSqlContext context)
        : base(context)
    {
        
    }
    public async Task<ResetPasswordCode?> GetByEmail(string email)
    {
        return await Context.ResetCodeEmails.FirstOrDefaultAsync(resetCodeEmails => resetCodeEmails.Email == email);
    }

    public async Task<ResetPasswordCode?> GetByResetCode(string resetCode)
    {
        return await Context.ResetCodeEmails.FirstOrDefaultAsync(resetCodeEmails => resetCodeEmails.ResetCode == resetCode);
    }

    public async Task<bool> IsExist(string email)
    {
        return await Context.ResetCodeEmails.AnyAsync(resetCodeEmail => resetCodeEmail.Email == email);
    }
}