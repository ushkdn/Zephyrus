using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Notification.Application.Features.Notifications.Commands.CreateNotification;

public record CreateNotificationCommandRequest(
    Guid RecipientId,
    string Title,
    string Message,
    string Type) : IRequest<HandlerResponse<CreateNotificationCommandResponse>>;
