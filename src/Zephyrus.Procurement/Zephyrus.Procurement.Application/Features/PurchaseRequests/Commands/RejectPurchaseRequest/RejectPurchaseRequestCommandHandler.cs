using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Enums;
using Zephyrus.SharedKernel.Common;
using Zephyrus.SharedKernel.Contracts.Procurement;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.RejectPurchaseRequest;

public class RejectPurchaseRequestCommandHandler(
    IPurchaseRequestRepository purchaseRequestRepository,
    IPublishEndpoint publishEndpoint,
    ILogger<RejectPurchaseRequestCommandHandler> logger)
    : IRequestHandler<RejectPurchaseRequestCommandRequest, HandlerResponse<RejectPurchaseRequestCommandResponse>>
{
    public async Task<HandlerResponse<RejectPurchaseRequestCommandResponse>> Handle(RejectPurchaseRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var purchaseRequest = await purchaseRequestRepository.GetByIdAsync(request.Id, cancellationToken);

        if (purchaseRequest is null)
        {
            logger.LogWarning("Purchase request {RequestId} not found", request.Id);
            return new HandlerResponse<RejectPurchaseRequestCommandResponse>(null, $"Purchase request with id: {request.Id} not found.", false);
        }

        if (purchaseRequest.Status != PurchaseRequestStatus.Pending)
        {
            logger.LogWarning("Cannot reject purchase request {RequestId} with status {Status}", request.Id, purchaseRequest.Status);
            return new HandlerResponse<RejectPurchaseRequestCommandResponse>(null, $"Only pending requests can be rejected. Current status: {purchaseRequest.Status}.", false);
        }

        purchaseRequest.Status = PurchaseRequestStatus.Rejected;
        purchaseRequest.Comment = request.Comment.Trim();
        purchaseRequest.DateUpdated = DateTime.UtcNow;

        await purchaseRequestRepository.UpdateAsync(purchaseRequest, cancellationToken);

        await publishEndpoint.Publish(new PurchaseRequestRejectedEvent(
            purchaseRequest.Id,
            purchaseRequest.RequestedBy,
            purchaseRequest.Comment!), cancellationToken);

        logger.LogInformation("Purchase request {RequestId} rejected", purchaseRequest.Id);

        return new HandlerResponse<RejectPurchaseRequestCommandResponse>(
            new RejectPurchaseRequestCommandResponse(purchaseRequest.Id, purchaseRequest.Status.ToString(), purchaseRequest.Comment),
            "Purchase request rejected.",
            true);
    }
}
