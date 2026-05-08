namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetAllPurchaseRequests;

public record GetAllPurchaseRequestsQueryResponse(
    Guid Id,
    Guid ProductId,
    decimal Quantity,
    Guid RequestedBy,
    string Status,
    string? Comment,
    DateTime DateCreated
    );
