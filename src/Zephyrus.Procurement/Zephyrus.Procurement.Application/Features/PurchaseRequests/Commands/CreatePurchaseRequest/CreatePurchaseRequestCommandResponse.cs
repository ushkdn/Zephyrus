namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Commands.CreatePurchaseRequest;

public record CreatePurchaseRequestCommandResponse(Guid Id, Guid ProductId, decimal Quantity, string Unit, string Status);
