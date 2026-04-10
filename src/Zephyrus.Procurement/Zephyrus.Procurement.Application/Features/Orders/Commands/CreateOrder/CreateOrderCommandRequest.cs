using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommandRequest(
    Guid PurchaseRequestId,
    Guid SupplierId,
    decimal UnitPrice,
    string Currency,
    Guid CreatedBy) : IRequest<HandlerResponse<CreateOrderCommandResponse>>;
