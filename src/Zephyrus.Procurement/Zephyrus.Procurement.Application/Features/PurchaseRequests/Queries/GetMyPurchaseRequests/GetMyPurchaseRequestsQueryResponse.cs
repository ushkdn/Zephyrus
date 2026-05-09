namespace Zephyrus.Procurement.Application.Features.PurchaseRequests.Queries.GetMyPurchaseRequests;

public record GetMyPurchaseRequestsQueryResponse(
    Guid Id,
    Guid ProductId,
    decimal Quantity,
    Guid RequestedBy,
    string Status,
    string? Comment,
    DateTime DateCreated
    );
