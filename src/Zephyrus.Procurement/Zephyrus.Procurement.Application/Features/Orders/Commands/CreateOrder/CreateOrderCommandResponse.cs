namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommandResponse(
    Guid Id,
    Guid SupplierId,
    IEnumerable<OrderItemResponse> Items,
    decimal TotalPrice,
    string Status
    );
