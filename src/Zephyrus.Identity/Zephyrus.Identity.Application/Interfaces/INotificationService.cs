using Zephyrus.Identity.Application.Models;

namespace Zephyrus.Identity.Application.Interfaces;

public interface INotificationService
{
    Task SendAsync(NotificationMessage message, CancellationToken cancellationToken);
}