using LightBoard.DataAccess.Abstractions.Repositories;

namespace LightBoard.DataAccess.Abstractions;

public interface IUnitOfWork
{
    IUsersRepository Users { get; }
    IResetCodeEmailsRepository ResetCodeEmails { get; }
    IBoardsRepository Boards { get; }
    IBoardMembersRepository BoardMembers { get; }
    IColumnsRepository Columns { get; }
    ICardsRepository Cards { get; }
    ICardAssigneeRepository CardAssignees { get; }
    Task SaveChangesAsync();
}