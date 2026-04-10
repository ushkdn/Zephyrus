using MediatR;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetPurchaseRequestById;

public class GetPurchaseRequestByIdQueryHandler(IPurchaseRequestRepository purchaseRequestRepository)
    : IRequestHandler<GetPurchaseRequestByIdQueryRequest, HandlerResponse<GetPurchaseRequestByIdQueryResponse>>
{
    public async Task<HandlerResponse<GetPurchaseRequestByIdQueryResponse>> Handle(GetPurchaseRequestByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var pr = await purchaseRequestRepository.GetByIdAsync(request.Id, cancellationToken);

        if (pr is null)
            return new HandlerResponse<GetPurchaseRequestByIdQueryResponse>(null, $"Purchase request with id: {request.Id} not found.", false);

        return new HandlerResponse<GetPurchaseRequestByIdQueryResponse>(
            new GetPurchaseRequestByIdQueryResponse(pr.Id, pr.ProductId, pr.Quantity, pr.Unit, pr.RequestedBy, pr.Status.ToString(), pr.Comment, pr.DateCreated, pr.DateUpdated),
            null,
            true);
    }
}
