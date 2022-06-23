namespace LightBoard.Domain.Entities.Notifications;

public class UserNotification : EntityBase<Guid>
{
    public UserNotification(string email, DateTime notifyAtUtc, string notificationText)
        : base(Guid.NewGuid())
    {
        Email = email;
        NotifyAtUtc = notifyAtUtc;
        Text = notificationText;
    }

    private UserNotification()
    {
    }
    
    public string Email { get; }
    public DateTime NotifyAtUtc { get; }
    public string Text { get; }
}