namespace Zephyrus.Notification.Application.Features.Notifications.Commands.CreateNotification;

public record CreateNotificationCommandResponse(
    Guid Id,
    Guid RecipientId,
    string Title,
    string Type);
