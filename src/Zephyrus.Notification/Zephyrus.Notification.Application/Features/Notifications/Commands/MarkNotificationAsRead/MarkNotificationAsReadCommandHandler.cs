using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Notification.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Notification.Application.Features.Notifications.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommandHandler(
    INotificationRepository notificationRepository,
    ILogger<MarkNotificationAsReadCommandHandler> logger)
    : IRequestHandler<MarkNotificationAsReadCommandRequest, HandlerResponse<MarkNotificationAsReadCommandResponse>>
{
    public async Task<HandlerResponse<MarkNotificationAsReadCommandResponse>> Handle(MarkNotificationAsReadCommandRequest request, CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (notification is null)
        {
            logger.LogWarning("Notification {NotificationId} not found", request.Id);
            return new HandlerResponse<MarkNotificationAsReadCommandResponse>(null, $"Notification with id: {request.Id} not found.", false);
        }

        if (notification.IsRead)
        {
            logger.LogWarning("Notification {NotificationId} is already read", request.Id);
            return new HandlerResponse<MarkNotificationAsReadCommandResponse>(null, $"Notification with id: {request.Id} is already read.", false);
        }

        await notificationRepository.MarkAsReadAsync(request.Id, cancellationToken);

        logger.LogInformation("Notification {NotificationId} marked as read", request.Id);

        return new HandlerResponse<MarkNotificationAsReadCommandResponse>(
            new MarkNotificationAsReadCommandResponse(request.Id),
            "Notification marked as read.",
            true);
    }
}
