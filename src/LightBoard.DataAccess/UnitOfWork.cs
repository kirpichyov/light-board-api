using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.DataAccess.Repositories;

namespace LightBoard.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly PostgreSqlContext _context;
    private IUsersRepository? _users;
    private IBoardsRepository? _boards;
    private IBoardMembersRepository? _boardMembers;
    private IColumnsRepository? _columns;

    public UnitOfWork(PostgreSqlContext context)
    {
        _context = context;
    }

    public IUsersRepository Users => _users ??= new UsersRepository(_context);
    public IBoardsRepository Boards => _boards ??= new BoardsRepository(_context);
    public IBoardMembersRepository BoardMembers => _boardMembers ??= new BoardMembersRepository(_context);
    public IColumnsRepository Columns => _columns ??= new ColumnsRepository(_context);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}