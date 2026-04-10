namespace Zephyrus.Procurement.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQueryResponse(
    Guid Id,
    Guid PurchaseRequestId,
    Guid SupplierId,
    Guid ProductId,
    decimal Quantity,
    decimal UnitPrice,
    string Currency,
    decimal TotalPrice,
    string Status,
    Guid CreatedBy,
    DateTime DateCreated,
    DateTime DateUpdated);
