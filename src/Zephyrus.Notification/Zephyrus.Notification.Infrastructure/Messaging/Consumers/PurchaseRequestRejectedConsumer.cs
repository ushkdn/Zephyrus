using MassTransit;
using MediatR;
using Zephyrus.Notification.Application.Features.Notifications.Commands.CreateNotification;
using Zephyrus.SharedKernel.Contracts.Procurement;

namespace Zephyrus.Notification.Infrastructure.Messaging.Consumers;

public class PurchaseRequestRejectedConsumer(ISender sender) : IConsumer<PurchaseRequestRejectedEvent>
{
    public async Task Consume(ConsumeContext<PurchaseRequestRejectedEvent> context)
    {
        var message = context.Message;

        var command = new CreateNotificationCommandRequest(
            RecipientId: message.RequestedBy,
            Title: "Purchase Request Rejected",
            Message: $"Your purchase request {message.PurchaseRequestId} has been rejected. Reason: {message.Comment}",
            Type: "PurchaseRequestRejected");

        await sender.Send(command, context.CancellationToken);
    }
}
