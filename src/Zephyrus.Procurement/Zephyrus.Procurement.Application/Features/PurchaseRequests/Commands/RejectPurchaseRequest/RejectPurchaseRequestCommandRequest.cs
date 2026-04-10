using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.RejectPurchaseRequest;

public record RejectPurchaseRequestCommandRequest(Guid Id, string Comment) : IRequest<HandlerResponse<RejectPurchaseRequestCommandResponse>>;
