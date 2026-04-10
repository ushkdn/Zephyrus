namespace Zephyrus.Procurement.Application.Features.Orders.Queries.GetAllOrders;

public record GetAllOrdersQueryResponse(
    Guid Id,
    Guid PurchaseRequestId,
    Guid SupplierId,
    Guid ProductId,
    decimal Quantity,
    decimal UnitPrice,
    string Currency,
    decimal TotalPrice,
    string Status,
    DateTime DateCreated);
