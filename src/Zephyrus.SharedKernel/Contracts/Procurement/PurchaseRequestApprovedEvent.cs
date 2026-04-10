namespace Zephyrus.SharedKernel.Contracts.Procurement;

public record PurchaseRequestApprovedEvent(
    Guid PurchaseRequestId,
    Guid RequestedBy);
