using MediatR;
using Microsoft.Extensions.Logging;
using Zephyrus.Procurement.Application.Interfaces;
using Zephyrus.Procurement.Domain.Entities;
using Zephyrus.Procurement.Domain.Enums;
using Zephyrus.SharedKernel.Common;

namespace Zephyrus.Procurement.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(
    IPurchaseRequestRepository purchaseRequestRepository,
    IOrderRepository orderRepository,
    ISupplierExistenceChecker supplierExistenceChecker,
    ILogger<CreateOrderCommandHandler> logger)
    : IRequestHandler<CreateOrderCommandRequest, HandlerResponse<CreateOrderCommandResponse>>
{
    public async Task<HandlerResponse<CreateOrderCommandResponse>> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        var purchaseRequest = await purchaseRequestRepository.GetByIdAsync(request.PurchaseRequestId, cancellationToken);

        if (purchaseRequest is null)
        {
            logger.LogWarning("Purchase request {PurchaseRequestId} not found", request.PurchaseRequestId);
            return new HandlerResponse<CreateOrderCommandResponse>(null, $"Purchase request with id: {request.PurchaseRequestId} not found.", false);
        }

        if (purchaseRequest.Status != PurchaseRequestStatus.Approved)
        {
            logger.LogWarning("Cannot create order for purchase request {PurchaseRequestId} with status {Status}", request.PurchaseRequestId, purchaseRequest.Status);
            return new HandlerResponse<CreateOrderCommandResponse>(null, $"Only approved purchase requests can be ordered. Current status: {purchaseRequest.Status}.", false);
        }

        if (await orderRepository.ExistsByPurchaseRequestIdAsync(request.PurchaseRequestId, cancellationToken))
        {
            logger.LogWarning("Order already exists for purchase request {PurchaseRequestId}", request.PurchaseRequestId);
            return new HandlerResponse<CreateOrderCommandResponse>(null, $"An order already exists for purchase request with id: {request.PurchaseRequestId}.", false);
        }

        if (!await supplierExistenceChecker.ExistsAsync(request.SupplierId, cancellationToken))
        {
            logger.LogWarning("Supplier {SupplierId} not found", request.SupplierId);
            return new HandlerResponse<CreateOrderCommandResponse>(null, $"Supplier with id: {request.SupplierId} not found.", false);
        }

        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            PurchaseRequestId = request.PurchaseRequestId,
            SupplierId = request.SupplierId,
            ProductId = purchaseRequest.ProductId,
            Quantity = purchaseRequest.Quantity,
            UnitPrice = request.UnitPrice,
            Currency = request.Currency.Trim().ToUpper(),
            TotalPrice = purchaseRequest.Quantity * request.UnitPrice,
            Status = OrderStatus.Created,
            CreatedBy = request.CreatedBy,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await orderRepository.AddAsync(order, cancellationToken);

        purchaseRequest.Status = PurchaseRequestStatus.Ordered;
        purchaseRequest.DateUpdated = DateTime.UtcNow;
        await purchaseRequestRepository.UpdateAsync(purchaseRequest, cancellationToken);

        logger.LogInformation("Order {OrderId} created for purchase request {PurchaseRequestId} with supplier {SupplierId}", order.Id, order.PurchaseRequestId, order.SupplierId);

        return new HandlerResponse<CreateOrderCommandResponse>(
            new CreateOrderCommandResponse(order.Id, order.PurchaseRequestId, order.SupplierId, order.TotalPrice, order.Currency, order.Status.ToString()),
            $"Order created successfully with id: {order.Id}",
            true);
    }
}
