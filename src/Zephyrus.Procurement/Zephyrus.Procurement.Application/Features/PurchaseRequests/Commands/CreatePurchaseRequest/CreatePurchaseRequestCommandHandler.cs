using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Entities;
using Zephyrus.Procurement.Domain.Enums;
using Zephyrus.SharedKernel.Common;
using Zephyrus.SharedKernel.Contracts.Procurement;

namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.CreatePurchaseRequest;

public class CreatePurchaseRequestCommandHandler(
    IPurchaseRequestRepository purchaseRequestRepository,
    IProductExistenceChecker productExistenceChecker,
    IPublishEndpoint publishEndpoint,
    ILogger<CreatePurchaseRequestCommandHandler> logger)
    : IRequestHandler<CreatePurchaseRequestCommandRequest, HandlerResponse<CreatePurchaseRequestCommandResponse>>
{
    public async Task<HandlerResponse<CreatePurchaseRequestCommandResponse>> Handle(CreatePurchaseRequestCommandRequest request, CancellationToken cancellationToken)
    {
        if (!await productExistenceChecker.ExistsAsync(request.ProductId, cancellationToken))
        {
            logger.LogWarning("Product {ProductId} not found in Catalog", request.ProductId);
            return new HandlerResponse<CreatePurchaseRequestCommandResponse>(null, $"Product with id: {request.ProductId} not found in Catalog.", false);
        }

        var purchaseRequest = new PurchaseRequestEntity
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            RequestedBy = request.RequestedBy,
            Status = PurchaseRequestStatus.Pending,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await purchaseRequestRepository.AddAsync(purchaseRequest, cancellationToken);

        await publishEndpoint.Publish(new PurchaseRequestCreatedEvent(
            purchaseRequest.Id,
            purchaseRequest.ProductId,
            purchaseRequest.Quantity,
            purchaseRequest.RequestedBy), cancellationToken);

        logger.LogInformation("Purchase request {RequestId} created by user {UserId} for product {ProductId}", purchaseRequest.Id, purchaseRequest.RequestedBy, purchaseRequest.ProductId);

        return new HandlerResponse<CreatePurchaseRequestCommandResponse>(
            new CreatePurchaseRequestCommandResponse(purchaseRequest.Id, purchaseRequest.ProductId, purchaseRequest.Quantity, purchaseRequest.Status.ToString()),
            $"Purchase request created successfully with id: {purchaseRequest.Id}",
            true);
    }
}
