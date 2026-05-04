using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(
    IOrderRepository orderRepository,
    ILogger<GetOrderByIdQueryHandler> logger)
    : IRequestHandler<GetOrderByIdQueryRequest, HandlerResponse<GetOrderByIdQueryResponse>>
{
    public async Task<HandlerResponse<GetOrderByIdQueryResponse>> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found", request.Id);
            return new HandlerResponse<GetOrderByIdQueryResponse>(null, $"Order with id: {request.Id} not found.", false);
        }

        return new HandlerResponse<GetOrderByIdQueryResponse>(
            new GetOrderByIdQueryResponse(order.Id, order.PurchaseRequestId, order.SupplierId, order.ProductId, order.Quantity, order.UnitPrice, order.Currency, order.TotalPrice, order.Status.ToString(), order.CreatedBy, order.DateCreated, order.DateUpdated),
            null,
            true);
    }
}
