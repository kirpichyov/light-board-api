using LightBoard.Domain.Entities.Notifications;

namespace LightBoard.DataAccess.Abstractions.Repositories;

public interface IUserNotificationsRepository : IRelationalRepositoryBase<UserNotification, Guid>
{
    Task<IReadOnlyCollection<UserNotification>> GetAll(DateTime maxNotifyAtUtc);
}