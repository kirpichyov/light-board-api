using LightBoard.DataAccess.Abstractions.Repositories;

namespace LightBoard.DataAccess.Abstractions;

public interface IUnitOfWork
{
    IUsersRepository Users { get; }
    IGeneratedCodesRepository GeneratedCodes { get; }
    IBoardsRepository Boards { get; }
    IBoardMembersRepository BoardMembers { get; }
    IColumnsRepository Columns { get; }
    ICardsRepository Cards { get; }
    ICardAssigneeRepository CardAssignees { get; }
    IAttachmentRepository Attachments { get; }
    ICardCommentRepository CardComments { get; }
    IUserNotificationsRepository UserNotifications { get; }
    Task SaveChangesAsync();
}