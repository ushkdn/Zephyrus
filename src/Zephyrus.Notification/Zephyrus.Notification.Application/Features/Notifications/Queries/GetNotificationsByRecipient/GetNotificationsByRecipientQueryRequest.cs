using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Notification.Application.Features.Notifications.Queries.GetNotificationsByRecipient;

public record GetNotificationsByRecipientQueryRequest(Guid RecipientId) : IRequest<HandlerResponse<IEnumerable<GetNotificationsByRecipientQueryResponse>>>;
