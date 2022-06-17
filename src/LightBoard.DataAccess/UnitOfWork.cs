using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.DataAccess.Repositories;

namespace LightBoard.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly PostgreSqlContext _context;
    private IResetCodeEmailsRepository? _resetCodeEmails;
    private IUsersRepository? _users;
    private IBoardsRepository? _boards;
    private IBoardMembersRepository? _boardMembers;
    private IColumnsRepository? _columns;
    private ICardsRepository? _cards;
    private ICardAssigneeRepository? _cardAssignees;

    public UnitOfWork(PostgreSqlContext context, IResetCodeEmailsRepository resetCodeEmails)
    {
        _context = context;
        _resetCodeEmails = resetCodeEmails;
    }

    public IUsersRepository Users => _users ??= new UsersRepository(_context);
    public IResetCodeEmailsRepository ResetCodeEmails => _resetCodeEmails ??= new ResetCodeEmailsRepository(_context);
    public IBoardsRepository Boards => _boards ??= new BoardsRepository(_context);
    public IBoardMembersRepository BoardMembers => _boardMembers ??= new BoardMembersRepository(_context);
    public IColumnsRepository Columns => _columns ??= new ColumnsRepository(_context);
    public ICardsRepository Cards => _cards ??= new CardRepository(_context);
    public ICardAssigneeRepository CardAssignees => _cardAssignees ??= new CardAssigneeRepository(_context);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}