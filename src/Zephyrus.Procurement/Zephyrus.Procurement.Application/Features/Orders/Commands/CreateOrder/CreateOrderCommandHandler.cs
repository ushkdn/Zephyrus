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
        if (!await supplierExistenceChecker.ExistsAsync(request.SupplierId, cancellationToken))
        {
            logger.LogWarning("Supplier {SupplierId} not found", request.SupplierId);
            return new HandlerResponse<CreateOrderCommandResponse>(null, $"Supplier with id: {request.SupplierId} not found.", false);
        }

        var purchaseRequests = new List<PurchaseRequestEntity>();

        foreach (var item in request.Items)
        {
            var purchaseRequest = await purchaseRequestRepository.GetByIdAsync(item.PurchaseRequestId, cancellationToken);

            if (purchaseRequest is null)
            {
                logger.LogWarning("Purchase request {PurchaseRequestId} not found", item.PurchaseRequestId);
                return new HandlerResponse<CreateOrderCommandResponse>(null, $"Purchase request with id: {item.PurchaseRequestId} not found.", false);
            }

            if (purchaseRequest.Status != PurchaseRequestStatus.Approved)
            {
                logger.LogWarning("Cannot create order for purchase request {PurchaseRequestId} with status {Status}", item.PurchaseRequestId, purchaseRequest.Status);
                return new HandlerResponse<CreateOrderCommandResponse>(null, $"Purchase request {item.PurchaseRequestId} must be approved. Current status: {purchaseRequest.Status}.", false);
            }

            if (await orderRepository.ExistsByPurchaseRequestIdAsync(item.PurchaseRequestId, cancellationToken))
            {
                logger.LogWarning("Order already exists for purchase request {PurchaseRequestId}", item.PurchaseRequestId);
                return new HandlerResponse<CreateOrderCommandResponse>(null, $"An order already exists for purchase request with id: {item.PurchaseRequestId}.", false);
            }

            purchaseRequests.Add(purchaseRequest);
        }

        var now = DateTime.UtcNow;
        var itemRequests = request.Items.ToList();

        var orderItems = purchaseRequests
            .Select((pr, i) => new OrderItemEntity
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.Empty,
                PurchaseRequestId = pr.Id,
                UnitPrice = itemRequests[i].UnitPrice,
                Currency = Enum.Parse<Currency>(itemRequests[i].Currency.Trim(), ignoreCase: true),
                TotalPrice = pr.Quantity * itemRequests[i].UnitPrice,
                DateCreated = now,
                DateUpdated = now
            })
            .ToList();

        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            SupplierId = request.SupplierId,
            TotalPrice = orderItems.Sum(i => i.TotalPrice),
            Status = OrderStatus.Created,
            CreatedBy = request.CreatedBy,
            DateCreated = now,
            DateUpdated = now
        };

        await orderRepository.AddAsync(order, cancellationToken);

        foreach (var item in orderItems)
        {
            item.OrderId = order.Id;
            await orderRepository.AddOrderItemAsync(item, cancellationToken);
        }

        foreach (var pr in purchaseRequests)
        {
            pr.Status = PurchaseRequestStatus.Ordered;
            pr.DateUpdated = now;
            await purchaseRequestRepository.UpdateAsync(pr, cancellationToken);
        }

        logger.LogInformation("Order {OrderId} created with {ItemCount} purchase requests for supplier {SupplierId}",
            order.Id, orderItems.Count, order.SupplierId);

        var itemResponses = orderItems
            .Select(i => new OrderItemResponse(i.PurchaseRequestId, i.UnitPrice, i.Currency.ToString(), i.TotalPrice));

        return new HandlerResponse<CreateOrderCommandResponse>(
            new CreateOrderCommandResponse(order.Id, order.SupplierId, itemResponses, order.TotalPrice, order.Status.ToString()),
            $"Order created successfully with id: {order.Id}",
            true);
    }
}
