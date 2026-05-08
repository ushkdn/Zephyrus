using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommandRequest(
    Guid SupplierId,
    IEnumerable<OrderItemRequest> Items,
    Guid CreatedBy
    ) : IRequest<HandlerResponse<CreateOrderCommandResponse>>;
