namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetPurchaseRequestById;

public record GetPurchaseRequestByIdQueryResponse(
    Guid Id,
    Guid ProductId,
    decimal Quantity,
    string Unit,
    Guid RequestedBy,
    string Status,
    string? Comment,
    DateTime DateCreated,
    DateTime DateUpdated);
