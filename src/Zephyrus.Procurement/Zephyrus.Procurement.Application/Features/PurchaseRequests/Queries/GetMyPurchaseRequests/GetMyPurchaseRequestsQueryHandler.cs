using MediatR;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetMyPurchaseRequests;

public class GetMyPurchaseRequestsQueryHandler(IPurchaseRequestRepository purchaseRequestRepository)
    : IRequestHandler<GetMyPurchaseRequestsQueryRequest, HandlerResponse<IEnumerable<GetMyPurchaseRequestsQueryResponse>>>
{
    public async Task<HandlerResponse<IEnumerable<GetMyPurchaseRequestsQueryResponse>>> Handle(GetMyPurchaseRequestsQueryRequest request, CancellationToken cancellationToken)
    {
        var purchaseRequests = await purchaseRequestRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var response = purchaseRequests.Select(pr =>
            new GetMyPurchaseRequestsQueryResponse(pr.Id, pr.ProductId, pr.Quantity, pr.RequestedBy, pr.Status.ToString(), pr.Comment, pr.DateCreated));

        return new HandlerResponse<IEnumerable<GetMyPurchaseRequestsQueryResponse>>(response, null, true);
    }
}
