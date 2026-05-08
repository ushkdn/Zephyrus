using MediatR;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetAllPurchaseRequests;

public class GetAllPurchaseRequestsQueryHandler(IPurchaseRequestRepository purchaseRequestRepository)
    : IRequestHandler<GetAllPurchaseRequestsQueryRequest, HandlerResponse<IEnumerable<GetAllPurchaseRequestsQueryResponse>>>
{
    public async Task<HandlerResponse<IEnumerable<GetAllPurchaseRequestsQueryResponse>>> Handle(GetAllPurchaseRequestsQueryRequest request, CancellationToken cancellationToken)
    {
        var purchaseRequests = await purchaseRequestRepository.GetAllAsync(cancellationToken);

        var response = purchaseRequests.Select(pr =>
            new GetAllPurchaseRequestsQueryResponse(pr.Id, pr.ProductId, pr.Quantity, pr.RequestedBy, pr.Status.ToString(), pr.Comment, pr.DateCreated));

        return new HandlerResponse<IEnumerable<GetAllPurchaseRequestsQueryResponse>>(response, null, true);
    }
}
