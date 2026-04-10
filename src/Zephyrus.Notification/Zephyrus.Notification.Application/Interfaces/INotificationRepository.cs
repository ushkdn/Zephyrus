using Zephyrus.Notification.Domain.Entities;

namespace Zephyrus.Notification.Application.Interfaces;

public interface INotificationRepository
{
    Task<NotificationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<NotificationEntity>> GetByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken);
    Task AddAsync(NotificationEntity notification, CancellationToken cancellationToken);
    Task MarkAsReadAsync(Guid id, CancellationToken cancellationToken);
}
