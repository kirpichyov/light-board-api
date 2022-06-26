using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.DataAccess.Repositories;

namespace LightBoard.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly PostgreSqlContext _context;
    private IGeneratedCodesRepository? _generatedCodes;
    private IUsersRepository? _users;
    private IBoardsRepository? _boards;
    private IBoardMembersRepository? _boardMembers;
    private IColumnsRepository? _columns;
    private ICardsRepository? _cards;
    private ICardAssigneeRepository? _cardAssignees;
    private IAttachmentRepository? _attachments;
    private ICardCommentRepository? _cardComments;
    private IUserNotificationsRepository? _userNotifications;
    private IUserSessionsRepository? _userSessions;

    public UnitOfWork(PostgreSqlContext context)
    {
        _context = context;
    }

    public IUsersRepository Users => _users ??= new UsersRepository(_context);
    public IGeneratedCodesRepository GeneratedCodes => _generatedCodes ??= new GeneratedCodesRepository(_context);
    public IBoardsRepository Boards => _boards ??= new BoardsRepository(_context);
    public IBoardMembersRepository BoardMembers => _boardMembers ??= new BoardMembersRepository(_context);
    public IColumnsRepository Columns => _columns ??= new ColumnsRepository(_context);
    public ICardsRepository Cards => _cards ??= new CardRepository(_context);
    public ICardAssigneeRepository CardAssignees => _cardAssignees ??= new CardAssigneeRepository(_context);
    public IAttachmentRepository Attachments => _attachments ??= new AttachmentRepository(_context);
    public ICardCommentRepository CardComments => _cardComments ??= new CardCommentRepository(_context);
    public IUserNotificationsRepository UserNotifications => _userNotifications ??= new UserNotificationsRepository(_context);
    public IUserSessionsRepository UserSessions => _userSessions ??= new UserSessionsRepository(_context);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}