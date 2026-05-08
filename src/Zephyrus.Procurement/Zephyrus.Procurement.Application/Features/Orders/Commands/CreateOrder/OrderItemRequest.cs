namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;

public record OrderItemRequest(
    Guid PurchaseRequestId,
    decimal UnitPrice,
    string Currency
    );