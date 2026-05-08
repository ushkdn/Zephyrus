namespace Zephyrus.Procurement.Application.Features.Orders.Queries.GetAllOrders;

public record GetAllOrdersQueryResponse(
    Guid Id,
    Guid SupplierId,
    decimal TotalPrice,
    string Status,
    DateTime DateCreated,
    IEnumerable<OrderItemQueryResponse> Items);

public record OrderItemQueryResponse(
    Guid PurchaseRequestId,
    decimal UnitPrice,
    string Currency,
    decimal TotalPrice);
