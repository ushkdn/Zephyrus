using MediatR;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Enums;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.DeliverOrder;

public class DeliverOrderCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<DeliverOrderCommandRequest, HandlerResponse<DeliverOrderCommandResponse>>
{
    public async Task<HandlerResponse<DeliverOrderCommandResponse>> Handle(DeliverOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
            return new HandlerResponse<DeliverOrderCommandResponse>(null, $"Order with id: {request.Id} not found.", false);

        if (order.Status != OrderStatus.Confirmed)
            return new HandlerResponse<DeliverOrderCommandResponse>(null, $"Only confirmed orders can be marked as delivered. Current status: {order.Status}.", false);

        order.Status = OrderStatus.Delivered;
        order.DateUpdated = DateTime.UtcNow;

        await orderRepository.UpdateAsync(order, cancellationToken);

        return new HandlerResponse<DeliverOrderCommandResponse>(
            new DeliverOrderCommandResponse(order.Id, order.Status.ToString()),
            "Order marked as delivered.",
            true);
    }
}
