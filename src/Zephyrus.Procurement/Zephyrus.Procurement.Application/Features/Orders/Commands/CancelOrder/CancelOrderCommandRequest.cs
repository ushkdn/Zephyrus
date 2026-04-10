using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CancelOrder;

public record CancelOrderCommandRequest(Guid Id) : IRequest<HandlerResponse<CancelOrderCommandResponse>>;
