using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Enums;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler(
    IOrderRepository orderRepository,
    ILogger<ConfirmOrderCommandHandler> logger)
    : IRequestHandler<ConfirmOrderCommandRequest, HandlerResponse<ConfirmOrderCommandResponse>>
{
    public async Task<HandlerResponse<ConfirmOrderCommandResponse>> Handle(ConfirmOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found", request.Id);
            return new HandlerResponse<ConfirmOrderCommandResponse>(null, $"Order with id: {request.Id} not found.", false);
        }

        if (order.Status != OrderStatus.Created)
        {
            logger.LogWarning("Cannot confirm order {OrderId} with status {Status}", request.Id, order.Status);
            return new HandlerResponse<ConfirmOrderCommandResponse>(null, $"Only created orders can be confirmed. Current status: {order.Status}.", false);
        }

        order.Status = OrderStatus.Confirmed;
        order.DateUpdated = DateTime.UtcNow;

        await orderRepository.UpdateAsync(order, cancellationToken);

        logger.LogInformation("Order {OrderId} confirmed", order.Id);

        return new HandlerResponse<ConfirmOrderCommandResponse>(
            new ConfirmOrderCommandResponse(order.Id, order.Status.ToString()),
            "Order confirmed successfully.",
            true);
    }
}
