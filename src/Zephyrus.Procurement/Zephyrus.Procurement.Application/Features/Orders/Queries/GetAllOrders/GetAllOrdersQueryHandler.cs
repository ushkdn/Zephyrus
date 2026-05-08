using MediatR;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetAllOrdersQueryRequest, HandlerResponse<IEnumerable<GetAllOrdersQueryResponse>>>
{
    public async Task<HandlerResponse<IEnumerable<GetAllOrdersQueryResponse>>> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
    {
        var orders = (await orderRepository.GetAllAsync(cancellationToken)).ToList();

        if (orders.Count == 0)
            return new HandlerResponse<IEnumerable<GetAllOrdersQueryResponse>>([], null, true);

        var orderIds = orders.Select(o => o.Id);
        var allItems = await orderRepository.GetItemsByOrderIdsAsync(orderIds, cancellationToken);

        var itemsByOrderId = new Dictionary<Guid, List<OrderItemQueryResponse>>();

        foreach (var item in allItems)
        {
            var itemResponse = new OrderItemQueryResponse(item.PurchaseRequestId, item.UnitPrice, item.Currency, item.TotalPrice);

            if (!itemsByOrderId.ContainsKey(item.OrderId))
                itemsByOrderId[item.OrderId] = [];

            itemsByOrderId[item.OrderId].Add(itemResponse);
        }

        var response = new List<GetAllOrdersQueryResponse>();

        foreach (var order in orders)
        {
            itemsByOrderId.TryGetValue(order.Id, out var items);

            response.Add(new GetAllOrdersQueryResponse(
                order.Id,
                order.SupplierId,
                order.TotalPrice,
                order.Status.ToString(),
                order.DateCreated,
                items ?? []));
        }

        return new HandlerResponse<IEnumerable<GetAllOrdersQueryResponse>>(response, null, true);
    }
}
