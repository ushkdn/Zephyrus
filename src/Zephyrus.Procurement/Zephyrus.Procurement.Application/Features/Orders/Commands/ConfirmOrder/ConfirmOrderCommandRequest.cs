using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.ConfirmOrder;

public record ConfirmOrderCommandRequest(Guid Id) : IRequest<HandlerResponse<ConfirmOrderCommandResponse>>;
