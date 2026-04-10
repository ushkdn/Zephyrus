using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Notification.Application.Features.Notifications.Commands.MarkNotificationAsRead;

public record MarkNotificationAsReadCommandRequest(Guid Id) : IRequest<HandlerResponse<MarkNotificationAsReadCommandResponse>>;
