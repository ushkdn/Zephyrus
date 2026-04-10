using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetPurchaseRequestById;

public record GetPurchaseRequestByIdQueryRequest(Guid Id) : IRequest<HandlerResponse<GetPurchaseRequestByIdQueryResponse>>;
