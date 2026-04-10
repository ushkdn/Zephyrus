namespace Zephyrus.Catalog.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQueryResponse(Guid Id, string Name, string Description, string Unit, Guid CategoryId, bool IsActive, DateTime DateCreated, DateTime DateUpdated);