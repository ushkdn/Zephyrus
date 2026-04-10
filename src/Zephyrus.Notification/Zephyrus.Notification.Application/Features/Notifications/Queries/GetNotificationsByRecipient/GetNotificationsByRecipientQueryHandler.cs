using MediatR;
using Zephyrus.Notification.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Notification.Application.Features.Notifications.Queries.GetNotificationsByRecipient;

public class GetNotificationsByRecipientQueryHandler(INotificationRepository notificationRepository)
    : IRequestHandler<GetNotificationsByRecipientQueryRequest, HandlerResponse<IEnumerable<GetNotificationsByRecipientQueryResponse>>>
{
    public async Task<HandlerResponse<IEnumerable<GetNotificationsByRecipientQueryResponse>>> Handle(GetNotificationsByRecipientQueryRequest request, CancellationToken cancellationToken)
    {
        var notifications = await notificationRepository.GetByRecipientIdAsync(request.RecipientId, cancellationToken);

        var response = notifications.Select(n => new GetNotificationsByRecipientQueryResponse(
            n.Id,
            n.RecipientId,
            n.Title,
            n.Message,
            n.Type,
            n.IsRead,
            n.DateCreated));

        return new HandlerResponse<IEnumerable<GetNotificationsByRecipientQueryResponse>>(response, null, true);
    }
}
