namespace Zephyrus.Notification.Application.Features.Notifications.Queries.GetNotificationsByRecipient;

public record GetNotificationsByRecipientQueryResponse(
    Guid Id,
    Guid RecipientId,
    string Title,
    string Message,
    string Type,
    bool IsRead,
    DateTime DateCreated);
