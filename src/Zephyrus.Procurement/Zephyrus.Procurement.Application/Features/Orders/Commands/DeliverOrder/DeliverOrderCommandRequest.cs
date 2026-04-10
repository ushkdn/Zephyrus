using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.DeliverOrder;

public record DeliverOrderCommandRequest(Guid Id) : IRequest<HandlerResponse<DeliverOrderCommandResponse>>;
