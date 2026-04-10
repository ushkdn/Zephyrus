namespace Zephyrus.Catalog.Application.Features.Products.Queries.GetAllProducts;

public record GetAllProductsQueryResponse(Guid Id, string Name, string Unit, Guid CategoryId, bool IsActive);