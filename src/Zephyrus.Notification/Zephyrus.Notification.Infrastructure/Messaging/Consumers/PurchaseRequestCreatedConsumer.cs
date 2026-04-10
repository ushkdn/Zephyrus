using MassTransit;
using MediatR;
using Zephyrus.Notification.Application.Features.Notifications.Commands.CreateNotification;
using Zephyrus.SharedKernel.Contracts.Procurement;

namespace Zephyrus.Notification.Infrastructure.Messaging.Consumers;

public class PurchaseRequestCreatedConsumer(ISender sender) : IConsumer<PurchaseRequestCreatedEvent>
{
    public async Task Consume(ConsumeContext<PurchaseRequestCreatedEvent> context)
    {
        var message = context.Message;

        var command = new CreateNotificationCommandRequest(
            RecipientId: message.RequestedBy,
            Title: "Purchase Request Created",
            Message: $"Your purchase request {message.PurchaseRequestId} has been created successfully.",
            Type: "PurchaseRequestCreated");

        await sender.Send(command, context.CancellationToken);
    }
}
