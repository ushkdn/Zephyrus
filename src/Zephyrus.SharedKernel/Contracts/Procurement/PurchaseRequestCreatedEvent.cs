namespace Zephyrus.SharedKernel.Contracts.Procurement;

public record PurchaseRequestCreatedEvent(
    Guid PurchaseRequestId,
    Guid ProductId,
    decimal Quantity,
    Guid RequestedBy
    );
