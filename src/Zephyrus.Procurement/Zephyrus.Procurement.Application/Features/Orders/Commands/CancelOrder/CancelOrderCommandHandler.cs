using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Enums;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler(
    IOrderRepository orderRepository,
    ILogger<CancelOrderCommandHandler> logger)
    : IRequestHandler<CancelOrderCommandRequest, HandlerResponse<CancelOrderCommandResponse>>
{
    public async Task<HandlerResponse<CancelOrderCommandResponse>> Handle(CancelOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found", request.Id);
            return new HandlerResponse<CancelOrderCommandResponse>(null, $"Order with id: {request.Id} not found.", false);
        }

        if (order.Status is OrderStatus.Delivered or OrderStatus.Cancelled)
        {
            logger.LogWarning("Cannot cancel order {OrderId} with status {Status}", request.Id, order.Status);
            return new HandlerResponse<CancelOrderCommandResponse>(null, $"Cannot cancel an order with status: {order.Status}.", false);
        }

        order.Status = OrderStatus.Cancelled;
        order.DateUpdated = DateTime.UtcNow;

        await orderRepository.UpdateAsync(order, cancellationToken);

        logger.LogInformation("Order {OrderId} cancelled", order.Id);

        return new HandlerResponse<CancelOrderCommandResponse>(
            new CancelOrderCommandResponse(order.Id, order.Status.ToString()),
            "Order cancelled successfully.",
            true);
    }
}
