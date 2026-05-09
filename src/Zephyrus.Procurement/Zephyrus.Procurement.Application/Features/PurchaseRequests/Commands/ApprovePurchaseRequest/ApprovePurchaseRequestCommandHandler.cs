using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Enums;
using Zephyrus.SharedKernel.Common;
using Zephyrus.SharedKernel.Contracts.Procurement;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.ApprovePurchaseRequest;

public class ApprovePurchaseRequestCommandHandler(
    IPurchaseRequestRepository purchaseRequestRepository,
    IPublishEndpoint publishEndpoint,
    ILogger<ApprovePurchaseRequestCommandHandler> logger)
    : IRequestHandler<ApprovePurchaseRequestCommandRequest, HandlerResponse<ApprovePurchaseRequestCommandResponse>>
{
    public async Task<HandlerResponse<ApprovePurchaseRequestCommandResponse>> Handle(ApprovePurchaseRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var purchaseRequest = await purchaseRequestRepository.GetByIdAsync(request.Id, cancellationToken);

        if (purchaseRequest is null)
        {
            logger.LogWarning("Purchase request {RequestId} not found", request.Id);
            return new HandlerResponse<ApprovePurchaseRequestCommandResponse>(null, $"Purchase request with id: {request.Id} not found.", false);
        }

        if (purchaseRequest.Status != PurchaseRequestStatus.Pending)
        {
            logger.LogWarning("Cannot approve purchase request {RequestId} with status {Status}", request.Id, purchaseRequest.Status);
            return new HandlerResponse<ApprovePurchaseRequestCommandResponse>(null, $"Only pending requests can be approved. Current status: {purchaseRequest.Status}.", false);
        }

        purchaseRequest.Status = PurchaseRequestStatus.Approved;
        purchaseRequest.Comment = null;
        purchaseRequest.DateUpdated = DateTime.UtcNow;

        await purchaseRequestRepository.UpdateAsync(purchaseRequest, cancellationToken);

        await publishEndpoint.Publish(new PurchaseRequestApprovedEvent(
            purchaseRequest.Id,
            purchaseRequest.RequestedBy), cancellationToken);

        logger.LogInformation("Purchase request {RequestId} approved", purchaseRequest.Id);

        return new HandlerResponse<ApprovePurchaseRequestCommandResponse>(
            new ApprovePurchaseRequestCommandResponse(purchaseRequest.Id, purchaseRequest.Status.ToString()),
            "Purchase request approved successfully.",
            true);
    }
}
