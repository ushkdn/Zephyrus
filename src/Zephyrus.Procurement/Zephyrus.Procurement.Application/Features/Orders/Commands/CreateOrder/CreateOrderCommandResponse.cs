namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommandResponse(Guid Id, Guid PurchaseRequestId, Guid SupplierId, decimal TotalPrice, string Currency, string Status);
