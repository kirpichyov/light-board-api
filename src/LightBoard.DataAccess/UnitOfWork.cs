using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.DataAccess.Repositories;

namespace LightBoard.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly PostgreSqlContext _context;
    private IUsersRepository? _users;

    public UnitOfWork(PostgreSqlContext context)
    {
        _context = context;
    }

    public IUsersRepository Users => _users ??= new UsersRepository(_context);
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}