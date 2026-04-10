using MediatR;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetAllOrdersQueryRequest, HandlerResponse<IEnumerable<GetAllOrdersQueryResponse>>>
{
    public async Task<HandlerResponse<IEnumerable<GetAllOrdersQueryResponse>>> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetAllAsync(cancellationToken);

        var response = orders.Select(o =>
            new GetAllOrdersQueryResponse(o.Id, o.PurchaseRequestId, o.SupplierId, o.ProductId, o.Quantity, o.UnitPrice, o.Currency, o.TotalPrice, o.Status.ToString(), o.DateCreated));

        return new HandlerResponse<IEnumerable<GetAllOrdersQueryResponse>>(response, null, true);
    }
}
