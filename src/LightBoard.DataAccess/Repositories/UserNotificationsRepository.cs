using LightBoard.DataAccess.Abstractions.Repositories;
using LightBoard.DataAccess.Connection;
using LightBoard.Domain.Entities.Notifications;

namespace LightBoard.DataAccess.Repositories;

public class UserNotificationsRepository : RelationalRepositoryBase<UserNotification, Guid>, IUserNotificationsRepository
{
    public UserNotificationsRepository(PostgreSqlContext context) 
        : base(context)
    {
    }

    public async Task<IReadOnlyCollection<UserNotification>> GetAll(DateTime maxNotifyAtUtc)
    {
        // TODO: Implement me to return notifications with NotifyAtUtc value less or equal to passed parameter.
        throw new NotImplementedException();
    }
}