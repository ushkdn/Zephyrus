namespace Zephyrus.SharedKernel.Contracts.Procurement;

public record PurchaseRequestRejectedEvent(
    Guid PurchaseRequestId,
    Guid RequestedBy,
    string Comment);
