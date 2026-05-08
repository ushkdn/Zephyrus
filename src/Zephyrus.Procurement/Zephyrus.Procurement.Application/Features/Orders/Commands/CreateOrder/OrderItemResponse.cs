namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;

public record OrderItemResponse(
    Guid PurchaseRequestId,
    decimal UnitPrice,
    string Currency,
    decimal TotalPrice
    );