namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.RejectPurchaseRequest;

public record RejectPurchaseRequestCommandResponse(Guid Id, string Status, string Comment);
