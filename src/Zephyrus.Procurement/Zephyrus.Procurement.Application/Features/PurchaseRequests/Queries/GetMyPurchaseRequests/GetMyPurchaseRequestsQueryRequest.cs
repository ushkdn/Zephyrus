using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetMyPurchaseRequests;

public record GetMyPurchaseRequestsQueryRequest(Guid UserId)
    : IRequest<HandlerResponse<IEnumerable<GetMyPurchaseRequestsQueryResponse>>>;
