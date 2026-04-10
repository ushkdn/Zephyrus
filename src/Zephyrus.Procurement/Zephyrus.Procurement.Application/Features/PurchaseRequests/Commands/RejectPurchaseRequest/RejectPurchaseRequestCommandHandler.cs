using MassTransit;
using MediatR;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Enums;
using Zephyrus.SharedKernel.Common;
using Zephyrus.SharedKernel.Contracts.Procurement;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.RejectPurchaseRequest;

public class RejectPurchaseRequestCommandHandler(
    IPurchaseRequestRepository purchaseRequestRepository,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<RejectPurchaseRequestCommandRequest, HandlerResponse<RejectPurchaseRequestCommandResponse>>
{
    public async Task<HandlerResponse<RejectPurchaseRequestCommandResponse>> Handle(RejectPurchaseRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var purchaseRequest = await purchaseRequestRepository.GetByIdAsync(request.Id, cancellationToken);

        if (purchaseRequest is null)
            return new HandlerResponse<RejectPurchaseRequestCommandResponse>(null, $"Purchase request with id: {request.Id} not found.", false);

        if (purchaseRequest.Status != PurchaseRequestStatus.Pending)
            return new HandlerResponse<RejectPurchaseRequestCommandResponse>(null, $"Only pending requests can be rejected. Current status: {purchaseRequest.Status}.", false);

        purchaseRequest.Status = PurchaseRequestStatus.Rejected;
        purchaseRequest.Comment = request.Comment.Trim();
        purchaseRequest.DateUpdated = DateTime.UtcNow;

        await purchaseRequestRepository.UpdateAsync(purchaseRequest, cancellationToken);

        await publishEndpoint.Publish(new PurchaseRequestRejectedEvent(
            purchaseRequest.Id,
            purchaseRequest.RequestedBy,
            purchaseRequest.Comment!), cancellationToken);

        return new HandlerResponse<RejectPurchaseRequestCommandResponse>(
            new RejectPurchaseRequestCommandResponse(purchaseRequest.Id, purchaseRequest.Status.ToString(), purchaseRequest.Comment),
            "Purchase request rejected.",
            true);
    }
}
