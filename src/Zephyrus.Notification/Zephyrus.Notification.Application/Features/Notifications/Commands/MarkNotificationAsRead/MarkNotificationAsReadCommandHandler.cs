using MediatR;
using Zephyrus.Notification.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Notification.Application.Features.Notifications.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository)
    : IRequestHandler<MarkNotificationAsReadCommandRequest, HandlerResponse<MarkNotificationAsReadCommandResponse>>
{
    public async Task<HandlerResponse<MarkNotificationAsReadCommandResponse>> Handle(MarkNotificationAsReadCommandRequest request, CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (notification is null)
            return new HandlerResponse<MarkNotificationAsReadCommandResponse>(null, $"Notification with id: {request.Id} not found.", false);

        if (notification.IsRead)
            return new HandlerResponse<MarkNotificationAsReadCommandResponse>(null, $"Notification with id: {request.Id} is already read.", false);

        await notificationRepository.MarkAsReadAsync(request.Id, cancellationToken);

        return new HandlerResponse<MarkNotificationAsReadCommandResponse>(
            new MarkNotificationAsReadCommandResponse(request.Id),
            "Notification marked as read.",
            true);
    }
}
