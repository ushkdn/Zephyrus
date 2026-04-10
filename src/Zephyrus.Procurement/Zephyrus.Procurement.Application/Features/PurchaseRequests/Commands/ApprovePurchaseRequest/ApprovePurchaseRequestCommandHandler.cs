using MassTransit;
using MediatR;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Enums;
using Zephyrus.SharedKernel.Common;
using Zephyrus.SharedKernel.Contracts.Procurement;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.ApprovePurchaseRequest;

public class ApprovePurchaseRequestCommandHandler(
    IPurchaseRequestRepository purchaseRequestRepository,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<ApprovePurchaseRequestCommandRequest, HandlerResponse<ApprovePurchaseRequestCommandResponse>>
{
    public async Task<HandlerResponse<ApprovePurchaseRequestCommandResponse>> Handle(ApprovePurchaseRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var purchaseRequest = await purchaseRequestRepository.GetByIdAsync(request.Id, cancellationToken);

        if (purchaseRequest is null)
            return new HandlerResponse<ApprovePurchaseRequestCommandResponse>(null, $"Purchase request with id: {request.Id} not found.", false);

        if (purchaseRequest.Status != PurchaseRequestStatus.Pending)
            return new HandlerResponse<ApprovePurchaseRequestCommandResponse>(null, $"Only pending requests can be approved. Current status: {purchaseRequest.Status}.", false);

        purchaseRequest.Status = PurchaseRequestStatus.Approved;
        purchaseRequest.Comment = null;
        purchaseRequest.DateUpdated = DateTime.UtcNow;

        await purchaseRequestRepository.UpdateAsync(purchaseRequest, cancellationToken);

        await publishEndpoint.Publish(new PurchaseRequestApprovedEvent(
            purchaseRequest.Id,
            purchaseRequest.RequestedBy), cancellationToken);

        return new HandlerResponse<ApprovePurchaseRequestCommandResponse>(
            new ApprovePurchaseRequestCommandResponse(purchaseRequest.Id, purchaseRequest.Status.ToString()),
            "Purchase request approved successfully.",
            true);
    }
}
