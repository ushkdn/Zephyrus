using MassTransit;
using MediatR;
using Zephyrus.Notification.Application.Features.Notifications.Commands.CreateNotification;
using Zephyrus.SharedKernel.Contracts.Procurement;

namespace Zephyrus.Notification.Infrastructure.Messaging.Consumers;

public class PurchaseRequestApprovedConsumer(ISender sender) : IConsumer<PurchaseRequestApprovedEvent>
{
    public async Task Consume(ConsumeContext<PurchaseRequestApprovedEvent> context)
    {
        var message = context.Message;

        var command = new CreateNotificationCommandRequest(
            RecipientId: message.RequestedBy,
            Title: "Purchase Request Approved",
            Message: $"Your purchase request {message.PurchaseRequestId} has been approved.",
            Type: "PurchaseRequestApproved");

        await sender.Send(command, context.CancellationToken);
    }
}
