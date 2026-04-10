using MediatR;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetAllPurchaseRequests;

public record GetAllPurchaseRequestsQueryRequest : IRequest<HandlerResponse<IEnumerable<GetAllPurchaseRequestsQueryResponse>>>;
