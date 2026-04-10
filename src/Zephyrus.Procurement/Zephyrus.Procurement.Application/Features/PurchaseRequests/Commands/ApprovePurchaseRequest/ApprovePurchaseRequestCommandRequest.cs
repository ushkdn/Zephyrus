using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.ApprovePurchaseRequest;

public record ApprovePurchaseRequestCommandRequest(Guid Id) : IRequest<HandlerResponse<ApprovePurchaseRequestCommandResponse>>;
