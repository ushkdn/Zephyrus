namespace Zephyrus.Procurement.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQueryResponse(
    Guid Id,
    Guid SupplierId,
    decimal TotalPrice,
    string Status,
    Guid CreatedBy,
    DateTime DateCreated,
    DateTime DateUpdated,
    IEnumerable<OrderItemQueryResponse> Items);

public record OrderItemQueryResponse(
    Guid PurchaseRequestId,
    decimal UnitPrice,
    string Currency,
    decimal TotalPrice);
