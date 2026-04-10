namespace Zephyrus.Notification.Domain.Entities;

public class NotificationEntity
{
    public Guid Id { get; set; }

    public Guid RecipientId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime DateCreated { get; set; }
}
