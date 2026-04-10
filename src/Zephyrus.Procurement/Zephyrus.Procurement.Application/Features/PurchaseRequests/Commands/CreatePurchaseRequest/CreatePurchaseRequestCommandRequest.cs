using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.CreatePurchaseRequest;

public record CreatePurchaseRequestCommandRequest(
    Guid ProductId,
    decimal Quantity,
    string Unit,
    Guid RequestedBy) : IRequest<HandlerResponse<CreatePurchaseRequestCommandResponse>>;
