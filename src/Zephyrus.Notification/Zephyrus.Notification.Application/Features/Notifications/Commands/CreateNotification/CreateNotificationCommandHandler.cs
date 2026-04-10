using MediatR;
using Zephyrus.Notification.Application.Interfaces;
using Zephyrus.Notification.Domain.Entities;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Notification.Application.Features.Notifications.Commands.CreateNotification;

public class CreateNotificationCommandHandler(INotificationRepository notificationRepository)
    : IRequestHandler<CreateNotificationCommandRequest, HandlerResponse<CreateNotificationCommandResponse>>
{
    public async Task<HandlerResponse<CreateNotificationCommandResponse>> Handle(CreateNotificationCommandRequest request, CancellationToken cancellationToken)
    {
        var notification = new NotificationEntity
        {
            Id = Guid.NewGuid(),
            RecipientId = request.RecipientId,
            Title = request.Title.Trim(),
            Message = request.Message.Trim(),
            Type = request.Type,
            IsRead = false,
            DateCreated = DateTime.UtcNow
        };

        await notificationRepository.AddAsync(notification, cancellationToken);

        return new HandlerResponse<CreateNotificationCommandResponse>(
            new CreateNotificationCommandResponse(notification.Id, notification.RecipientId, notification.Title, notification.Type),
            $"Notification created successfully with id: {notification.Id}",
            true);
    }
}
